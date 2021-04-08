// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	using System;
	using System.Text;

	/// <summary>
	/// Utilities.
	/// </summary>
	public class Utilities
	{

		#region Public Methods

		/// <summary>
		/// Cleans the string.
		/// </summary>
		/// <param name="str">The str.</param>
		/// <returns>A string.</returns>
		public static string Clean(string str)
		{
			return Strip(str,CGML.STRING_BEGIN_END);
		}

		/// <summary>
		/// Strips the string of any invalidCharacters.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="invalidCharacter">The invalid character.</param>
		/// <returns>A string.</returns>
		public static string Strip(string str,char invalidCharacter)
		{
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (c == invalidCharacter)
				{
					str = str.Insert(i++,"\\");
				}
			}

			return str;
		}

		/// <summary>
		/// Reverses a stripped string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="invalidCharacter">The invalid character.</param>
		/// <returns>A string.</returns>
		public static string ReverseStrip(string str,char invalidCharacter)
		{
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (c == invalidCharacter)
				{
					if (str[i - 1] == '\\' && str[i - 2] != '\\')
					{
						str = str.Remove(--i,1);
					}
				}
			}

			return str;
		}

		/// <summary>
		/// Converts a <see cref="Node"/> to a string. Recursive.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="pretty">If true, makes the string more readable.</param>
		/// <returns>A string.</returns>
		public static string ToCGML(Node node,bool pretty)
		{
			return ToCGML(node,pretty,0);
		}

		/// <summary>
		/// Converts a CGML string to a node. Recursive.
		/// </summary>
		/// <param name="str">The data.</param>
		/// <returns>A CGMLObject.</returns>
		public static Node FromCGML(string str)
		{
			return FromCGML(str,CGML.VERSION_LATEST);
		}

		/// <summary>
		/// Converts a CGML string to a node. Recursive.
		/// </summary>
		/// <param name="str">The data.</param>
		/// <param name="version">The CGML version you're importing.</param>
		/// <returns>A CGMLObject.</returns>
		public static Node FromCGML(string str,int version)
		{
			switch (version)
			{
				case CGML.VERSION_0_4_X:
				{
					return FromCGML_0_4_0(str);
				}

				case CGML.VERSION_LEGACY:
				{
					return FromCGML_Legacy(str);
				}

				default:
				{
					goto case CGML.VERSION_LATEST;
				}
			}
		}

		#endregion

		#region Private Methods

		// TODO: add comments (ez)
		// <! this is a comment !>

		/// <summary>
		/// </summary>
		private static Node FromCGML_0_4_0(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			Node root = null;

			ReadElement elementType = ReadElement.None;
			bool reading = false;
			ReadScope scope = ReadScope.Default;

			Node thisNode = null;
			StringBuilder element = new StringBuilder();
			char lastChar = (char)0;

			Func<string> commitElement = new Func<string>(() =>
			{
				string s = element.ToString();
				element.Clear();
				return s;
			});

			Action commitNodeKey = new Action(() =>
			{
				Node newNode = new Node(commitElement());

				if (root == null)
				{
					root = newNode;
				}
				else
				{
					thisNode.Push(newNode);
				}

				thisNode = newNode;
			});

			Action commitAttributeKey = new Action(() =>
			{
				thisNode.Attributes.Set(new Attribute(commitElement()));
				elementType = ReadElement.None;
			});

			Action commitValue = new Action(() =>
			{
				switch (scope)
				{
					case ReadScope.Node:
					{
						thisNode.Value = commitElement();

						break;
					}
					case ReadScope.Attribute:
					{
						thisNode.Attributes[thisNode.Attributes.Count - 1].Value = commitElement();

						break;
					}
				}
				elementType = ReadElement.None;
			});

			for (int i = 0; i < str.Length; i++)
			{
				if (i < str.Length)
				{
					char c = str[i];
					char nextChar = i < str.Length - 1 ? str[i + 1] : (char)0;

					if (reading)
					{
						switch (c)
						{
							case CGML.STRING_BEGIN_END:
							{
								if (lastChar == '\\')
								{
									break;
								}

								commitValue();
								reading = false;

								break;
							}

							default:
							{
								element.Append(c);
								break;
							}
						}
					}
					else
					{
						switch (c)
						{
							// Ignore newlines
							case '\n':
								break;

							// Ignore carriage return
							case '\r':
								break;

							// Ignore tabs
							case '\t':
								break;

							case CGML.NODE_BEGIN:
							{
								scope = ReadScope.Node;
								elementType = ReadElement.Key;

								break;
							}

							case CGML.NODE_END:
							{
								scope = ReadScope.Default;
								elementType = ReadElement.None;

								break;
							}

							case CGML.STRING_BEGIN_END:
							{
								elementType = ReadElement.Value;
								reading = true;

								break;
							}

							case CGML.EQUAL_OPERATOR:
							{
								if (elementType == ReadElement.Key && scope == ReadScope.Attribute)
								{
									commitAttributeKey();
									break;
								}

								break;
							}

							case ' ':
							{
								scope = ReadScope.Attribute;
								elementType = ReadElement.Key;

								break;
							}

							case CGML.NODE_ENDMARK:
							{
								thisNode = thisNode.Parent;

								break;
							}

							default:
							{
								if (elementType != ReadElement.None)
								{
									element.Append(c);
								}

								break;
							}
						}
					}

					if (scope == ReadScope.Node && elementType == ReadElement.Key)
					{
						if (nextChar == CGML.NODE_END || nextChar == CGML.EQUAL_OPERATOR || nextChar == ' ')
						{
							commitNodeKey();
						}
					}

					lastChar = c;
				}
			}

			return root;
		}

		/// <summary>
		/// </summary>
		private static Node FromCGML_Legacy(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			Node root = null;

			bool inString = false;
			bool wasInString = false;
			bool inNode = false;
			bool inValue = false;
			bool inKey = false;
			bool inAttribute = false;

			char lastChar = (char)0;
			Node thisNode = null;
			StringBuilder thisKey = new StringBuilder();
			StringBuilder thisValue = null;

			for (int i = 0; i <= str.Length; i++)
			{
				if (i < str.Length)
				{
					char c = str[i];
					char nextChar = i < str.Length - 1 ? str[i + 1] : (char)0;

					if (inString)
					{
						if (c != CGML.STRING_BEGIN_END || (str[i - 1] == '\\' && str[i - 2] != '\\'))
						{
							thisValue.Append(c);

						}
						else
						{
							inString = false;
							wasInString = true;
						}
					}
					else
					{
						Node newNode = null;
						if (wasInString)
						{
							if (inNode)
							{
								if (inAttribute)
								{
									thisNode.Attributes.Set(new Attribute(thisKey.ToString(),thisValue.ToString()));
									thisValue = null;

									inAttribute = false;
								}
							}
						}

						switch (c)
						{
							// Ignore newlines
							case '\n':
								break;

							// Ignore carriage return
							case '\r':
								break;

							// Ignore tabs
							case '\t':
								break;

							case CGML.NODE_BEGIN:
								inNode = true;
								inKey = true;
								thisKey = new StringBuilder();
								break;

							case CGML.NODE_END:
								newNode = new Node(thisKey.ToString());

								inNode = false;
								inKey = false;
								inValue = false;
								inAttribute = false;
								break;

							case CGML.NODE_ENDMARK:
								thisNode = thisNode.Parent;
								break;

							case CGML.STRING_BEGIN_END:
								inString = true;
								thisValue = new StringBuilder();
								break;

							case CGML.NODE_VALUE_BEGIN:
								inKey = false;
								inValue = true;
								break;

							case CGML.NODE_VALUE_END:
								inValue = false;
								break;

							case CGML.EQUAL_OPERATOR:
								inValue = true;
								break;

							case ' ':
								inKey = false;
								inAttribute = true;
								break;

							case (char)0:
								throw new System.Exception("[CGML] Could not parse string");

							default:
								if (inNode)
								{
									if (inKey)
									{
										thisKey.Append(c);
									}
									else if (inValue)
									{
										thisValue.Append(c);
									}
								}
								break;
						}

						wasInString = false;

						if (newNode != null)
						{
							if (root == null)
							{
								root = newNode;
							}
							else
							{
								thisNode.Push(newNode);
							}

							thisNode = newNode;
						}
					}

					lastChar = c;
				}
			}

			return root;
		}

		/// <summary>
		/// Converts a <see cref="Node"/> to a string. Recursive.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="pretty">If true, makes the string more readable.</param>
		/// <param name="level">Recursion level.</param>
		/// <returns>A string.</returns>
		private static string ToCGML(Node node,bool pretty,int level)
		{
			StringBuilder str = new StringBuilder();

			str.Append(CGML.NODE_BEGIN);
			str.Append(node.Key);

			if (node.HasValue)
			{
				str.Append(CGML.EQUAL_OPERATOR);
				str.Append(CGML.STRING_BEGIN_END);
				str.Append(Clean(node.Value.ToString()));
				str.Append(CGML.STRING_BEGIN_END);
			}

			if (node.Attributes.Count > 0)
			{
				str.Append(" ");
			}

			str.Append(node.Attributes.ToString());
			str.Append(CGML.NODE_END);

			for (int i = 0; i < node.Count; i++)
			{
				if (pretty)
				{
					AddNewLine(str,level + 1);
				}

				str.Append(ToCGML(node[i],pretty,level + 1));
			}

			if (node.Count > 0)
			{
				if (pretty)
				{
					AddNewLine(str,level);
				}
			}

			str.Append(CGML.NODE_ENDMARK);

			return str.ToString();
		}

		/// <summary>
		/// Adds a new line to the StringBuilder.
		/// </summary>
		/// <param name="str">The StringBuilder.</param>
		/// <param name="level">The indentation level.</param>
		private static void AddNewLine(StringBuilder str,int level)
		{
			str.Append("\n");
			for (int i = 0; i < level; i++)
			{
				str.Append("\t");
			}
		}

		#endregion

	}
}
