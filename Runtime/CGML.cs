// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Main class.
	/// </summary>
	public class CGML
	{

		#region Public Fields

		/// <summary>
		/// Begins a node section.
		/// </summary>
		public const char NODE_BEGIN = '<';

		/// <summary>
		/// Ends a node section.
		/// </summary>
		public const char NODE_END = '>';

		/// <summary>
		/// Indicates a closing node.
		/// </summary>
		public const char NODE_ENDMARK = '/';

		/// <summary>
		/// Begins a node value.
		/// </summary>
		public const char NODE_VALUE_BEGIN = '[';

		/// <summary>
		/// Ends a node value.
		/// </summary>
		public const char NODE_VALUE_END = ']';

		/// <summary>
		/// Begins and ends a string.
		/// </summary>
		public const char STRING_BEGIN_END = '"';

		/// <summary>
		/// Used between key and value pairs.
		/// </summary>
		public const char EQUAL_OPERATOR = '=';

		#endregion

	}

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
		/// Converts a <see cref="Node"/> to a string.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="pretty">If true, makes the string more readable.</param>
		/// <returns>A string.</returns>
		public static string Node2String(Node node,bool pretty)
		{
			StringBuilder str = new StringBuilder();

			str.Append(CGML.NODE_BEGIN);
			str.Append(node.Key);

			if (!string.IsNullOrEmpty(node.Value))
			{
				str.Append(CGML.NODE_VALUE_BEGIN);
				str.Append(CGML.STRING_BEGIN_END);
				str.Append(Clean(node.Value));
				str.Append(CGML.STRING_BEGIN_END);
				str.Append(CGML.NODE_VALUE_END);
			}

			if (node.Attributes.Count > 0)
				str.Append(" ");
			str.Append(node.Attributes.ToString());
			str.Append(CGML.NODE_END);

			for (int i = 0; i < node.Count; i++)
			{
				str.Append(Node2String(node[i],pretty));
			}

			str.Append(CGML.NODE_BEGIN);
			str.Append(CGML.NODE_ENDMARK);
			str.Append(node.Key);
			str.Append(CGML.NODE_END);

			return str.ToString();
		}

		/// <summary>
		/// Converts a CGML string to an object.
		/// </summary>
		/// <param name="str">The str.</param>
		/// <returns>A CGMLObject.</returns>
		public static CGMLObject String2Object(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			CGMLObject obj = null;

			bool inString = false;
			bool wasInString = false;
			bool inNode = false;
			bool inEndNode = false;
			bool inValue = false;
			bool inKey = false;
			bool inAttribute = false;

			char lastChar = (char)0;
			Node thisNode = null;
			StringBuilder thisKey = new StringBuilder();
			StringBuilder thisValue = new StringBuilder();
			(int level, int child) depth = (0, 0);

			for (int i = 0; i <= str.Length; i++)
			{
				if (i == str.Length)
				{

				}
				else
				{
					char c = str[i];
					char nextChar = i < str.Length - 1 ? str[i + 1] : (char)0;

					//bool isLetter = char.IsLetter(c);
					//bool isDigit = char.IsDigit(c);
					//bool isLetterOrDigit = isLetter || isDigit;

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
								if (inValue)
								{
									if (inAttribute)
									{
										thisNode.Attributes.Set(new Attribute(thisKey.ToString(),thisValue.ToString()));

										inValue = false;
										inAttribute = false;
									}
									else
									{
										newNode = new Node(thisKey.ToString(),thisValue.ToString());
									}
								}
							}
						}

						switch (c)
						{
							case CGML.NODE_BEGIN:
								if (nextChar == CGML.NODE_ENDMARK)
								{
									inNode = false;
									inEndNode = true;
								}
								else
								{
									inNode = true;
									inKey = true;
									thisKey = new StringBuilder();
								}
								break;

							case CGML.NODE_END:
								if (inEndNode)
								{
									thisNode = thisNode.Parent;
								}
								else if (!wasInString)
								{
									newNode = new Node(thisKey.ToString());
								}

								inEndNode = false;
								inNode = false;
								break;

							case CGML.STRING_BEGIN_END:
								inString = true;
								thisValue = new StringBuilder();
								break;

							case CGML.NODE_VALUE_BEGIN:
								inKey = false;
								inValue = true;
								thisValue = new StringBuilder();
								break;

							case CGML.NODE_VALUE_END:
								inValue = false;
								break;

							case CGML.EQUAL_OPERATOR:
								inKey = false;
								inValue = true;
								break;

							case ' ':
								inKey = true;
								inAttribute = true;
								thisKey = new StringBuilder();
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
							if (obj == null)
							{
								obj = new CGMLObject(newNode);
							}
							else
							{
								thisNode.Add(newNode);
							}

							thisNode = newNode;
						}
					}

					lastChar = c;
				}
			}

			return obj;
		}

		#endregion
	}

	/// <summary>
	/// A CGML object.
	/// </summary>
	public class CGMLObject
	{

		#region Public Indexers + Properties

		/// <summary>
		/// Gets the root node.
		/// </summary>
		public Node Root { get; } = null;

		#endregion

		#region Public Constructors + Destructors

		/// <summary>
		/// Initializes a new <see cref="CGMLObject"/>.
		/// </summary>
		/// <param name="root">The root node.</param>
		public CGMLObject(Node root)
		{
			Root = root;
		}

		/// <summary>
		/// Creates a deep copy of a <see cref="CGMLObject"/>.
		/// </summary>
		/// <param name="original">The original object.</param>
		public CGMLObject(CGMLObject original)
		{

		}

		/// <summary>
		/// Initializes a new <see cref="CGMLObject"/> with an empty root node.
		/// </summary>
		public CGMLObject()
		{
			Root = new Node("Root");
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Converts this <see cref="CGMLObject"/> to a string.
		/// </summary>
		/// <param name="pretty">If true, makes the string more readable.</param>
		/// <returns>A string.</returns>
		public string ToCGML(bool pretty)
		{
			return Utilities.Node2String(Root,pretty);
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			return ToCGML(false);
		}

		#endregion

	}

	/// <summary>
	/// A CGML node.
	/// </summary>
	public class Node
	{

		#region Public Indexers + Properties

		public Node this[int index] => Children[index];

		public Node this[string key]
		{
			get
			{
				foreach (Node child in Children)
				{
					if (child.Key == key)
					{
						return child;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the parent node.
		/// </summary>
		public Node Parent { get; private set; } = null;

		/// <summary>
		/// Gets the node's key.
		/// </summary>
		public string Key { get; } = null;

		/// <summary>
		/// Gets or sets the node's value.
		/// </summary>
		public string Value { get; set; } = null;

		/// <summary>
		/// Gets the node's attributes.
		/// </summary>
		public AttributeList Attributes { get; } = new AttributeList();

		/// <summary>
		/// Gets the child count.
		/// </summary>
		public int Count => Children.Count;

		/// <summary>
		/// Gets a value indicating whether this node has a value.
		/// </summary>
		public bool HasValue => !string.IsNullOrEmpty(Value);

		/// <summary>
		/// Gets a value indicating whether this node has children.
		/// </summary>
		public bool HasChildren => Count > 0;

		#endregion

		#region Private Indexers + Properties

		/// <summary>
		/// Gets the node list.
		/// </summary>
		private List<Node> Children { get; } = new List<Node>();

		#endregion

		#region Public Constructors + Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Children"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public Node(string key,string value = "")
		{
			Key = key;
			Value = value;
		}

		#endregion

		#region Private Constructors + Destructors

		/// <summary>
		/// Prevents a default instance of the <see cref="Children"/> class from being created.
		/// </summary>
		private Node() { }

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a child node. Also useful for "moving" nodes between parents.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>A CGMLNode.</returns>
		public Node Add(Node node)
		{
			node.Disconnect();
			node.SetParent(this);
			Children.Add(node);
			return this;
		}

		private void SetParent(Node newParent)
		{
			Parent = newParent;
		}

		/// <summary>
		/// Removes this node from its parent.
		/// </summary>
		public Node Disconnect()
		{
			if (Parent != null)
				Parent.Remove(this);
			return this;
		}

		/// <summary>
		/// Removes a child node.
		/// </summary>
		/// <param name="node">The node.</param>
		public Node Remove(Node node)
		{
			if (Children.Contains(node))
			{
				node.Parent = null;
				Children.Remove(node);
			}
			return this;
		}

		/// <summary>
		/// Clears the children.
		/// </summary>
		public Node DisconnectChildren()
		{
			ForEach(new System.Action<Node>((node) =>
			{
				node.Disconnect();
			}));

			return this;
		}

		/// <summary>
		/// Iterates through each child node.
		/// </summary>
		/// <param name="action">The action.</param>
		public Node ForEach(System.Action<Node> action)
		{
			foreach (Node child in Children)
			{
				action?.Invoke(child);
			}
			return this;
		}

		/// <summary>
		/// Recursively iterates through each child.
		/// </summary>
		/// <param name="action">The action.</param>
		public Node ForEachRecursive(System.Action<Node> action)
		{
			throw new System.NotImplementedException();
		}

		#endregion

	}

	/// <summary>
	/// A CGML attribute.
	/// </summary>
	public class Attribute
	{

		#region Public Indexers + Properties

		/// <summary>
		/// Gets the key.
		/// </summary>
		public string Key { get; } = null;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public object Value { get; set; } = null;

		/// <summary>
		/// Gets or sets a value indicating whether to serialize this attribute.
		/// </summary>
		public bool Serialize { get; set; } = true;

		#endregion

		#region Public Constructors + Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Attribute"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public Attribute(string key,object value = null,bool serialize = true)
		{
			Key = key;
			Value = value;
			Serialize = serialize;
		}

		#endregion

		#region Private Constructors + Destructors

		/// <summary>
		/// Prevents a default instance of the <see cref="Attribute"/> class from being created.
		/// </summary>
		private Attribute() { }

		#endregion

		#region Public Methods

		/// <summary>
		/// WARNING: does not respect the <see cref="Serialize"/> property.
		/// </summary>
		public override string ToString()
		{
			return Key + CGML.EQUAL_OPERATOR + CGML.STRING_BEGIN_END + Utilities.Clean(Value.ToString()) + CGML.STRING_BEGIN_END;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Attribute))
				return false;
			return ((Attribute)obj).Key == Key && ((Attribute)obj).Value.Equals(Value);
		}

		#endregion

	}

	/// <summary>
	/// An attribute list.
	/// </summary>
	public class AttributeList
	{

		#region Public Indexers + Properties

		public Attribute this[string key]
		{
			get
			{
				foreach (Attribute attribute in this.m_Attributes)
				{
					if (attribute.Key == key)
						return attribute;
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the total number of attributes in this list.
		/// </summary>
		public int Count => this.m_Attributes.Count;

		#endregion

		#region Private Fields

		private List<Attribute> m_Attributes = new List<Attribute>();

		#endregion

		#region Public Constructors + Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeList"/> class.
		/// </summary>
		public AttributeList() { }

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds an attribute.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		public void Set(Attribute attribute)
		{
			for (int i = 0; i < this.m_Attributes.Count; i++)
			{
				if (this.m_Attributes[i].Key == attribute.Key)
				{
					this.m_Attributes[i] = attribute;
					return;
				}
			}

			this.m_Attributes.Add(attribute);
		}

		/// <summary>
		/// Removes an attribute.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		public void Remove(Attribute attribute)
		{
			if (attribute != null)
				this.m_Attributes.Remove(attribute);
		}

		/// <summary>
		/// Removes an attribute.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		public void Remove(string key)
		{
			Attribute attributeToRemove = null;
			foreach (Attribute attribute in this.m_Attributes)
			{
				if (attribute.Key == key)
				{
					attributeToRemove = attribute;
					break;
				}
			}

			Remove(attributeToRemove);
		}

		/// <summary>
		/// Removes an attribute at index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void RemoveAt(int index)
		{
			if (index > 0 && index < this.m_Attributes.Count)
				this.m_Attributes.RemoveAt(index);
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			this.m_Attributes.Clear();
		}

		/// <summary>
		/// Whether this <see cref="AttributeList"/> contains a key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A bool.</returns>
		public bool ContainsKey(string key)
		{
			foreach (Attribute attribute in this.m_Attributes)
			{
				if (attribute.Key == key)
					return true;
			}

			return false;
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			bool hasAppended = false;
			for (int i = 0; i < Count; i++)
			{
				if (this.m_Attributes[i].Serialize)
				{
					if (hasAppended)
					{
						str.Append(" ");
					}
					str.Append(this.m_Attributes[i].ToString());
					hasAppended = true;
				}
			}

			return str.ToString();
		}

		#endregion

	}
}
