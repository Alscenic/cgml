// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	using System.Collections.Generic;

	/// <summary>
	/// A CGML node.
	/// </summary>
	public class Node
	{

		#region Public Indexers + Properties

		/// <summary>
		/// Gets the child at the specified index.
		/// </summary>
		public Node this[int index] => Children[index];

		/// <summary>
		/// Gets the first child node with the specified key, or creates a new one if it doesn't exist.
		/// </summary>
		public Node this[string key]
		{
			get
			{
				Node foundNode = FindChild(key);
				if (foundNode == null)
				{
					foundNode = new Node(key);
					Push(foundNode);
				}

				return foundNode;
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
		public object Value { get; set; } = null;

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
		public bool HasValue => Value != null;

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
		public Node(string key,object value = null)
		{
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Recursively creates a deep copy of another <see cref="Node"/>.
		/// </summary>
		/// <param name="original">The <paramref name="original"/>.</param>
		public Node(Node original)
		{
			Key = original.Key;
			Value = original.Value;

			original.ForEach(new System.Action<Node>((otherChild) =>
			{
				Push(new Node(otherChild));
			}));
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
		/// Disconnects node from any existing connections and adds it to this node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>A CGMLNode.</returns>
		public void Push(Node node)
		{
			Push(Children.Count,node);
		}

		/// <summary>
		/// Disconnects node from any existing connections and inserts it into this node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>A CGMLNode.</returns>
		public void Push(int index,Node node)
		{
			node.Disconnect();
			node.Parent = this;
			Children.Insert(index,node);
		}

		/// <summary>
		/// Removes this node from its parent.
		/// </summary>
		public void Disconnect()
		{
			if (Parent != null)
			{
				Parent.Remove(this);
			}
		}

		/// <summary>
		/// Removes a child node.
		/// </summary>
		/// <param name="node">The node.</param>
		public void Remove(Node node)
		{
			if (node != null && Children.Contains(node))
			{
				Children.Remove(node);
				node.Parent = null;
			}
		}

		/// <summary>
		/// Removes a child node at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The <paramref name="index"/>.</param>
		/// <returns>A Node.</returns>
		public void RemoveAt(int index)
		{
			if (index > 0 && index < Count)
			{
				Children.RemoveAt(index);
			}
			else
			{
				throw new System.IndexOutOfRangeException();
			}
		}

		/// <summary>
		/// Clears the children.
		/// </summary>
		public void DisconnectChildren(bool recursive)
		{
			ForEach(new System.Action<Node>((node) =>
			{
				node.Disconnect();
				if (recursive)
					node.DisconnectChildren(true);
			}));
		}

		/// <summary>
		/// Iterates through each child node.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ForEach(System.Action<Node> action)
		{
			foreach (Node child in Children)
			{
				action?.Invoke(child);
			}
		}

		/// <summary>
		/// Recursively iterates through each child.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ForEachRecursive(System.Action<Node> action,bool invokeSelf = false)
		{
			if (invokeSelf)
			{
				action?.Invoke(this);
			}

			foreach (Node child in Children)
			{
				child.ForEachRecursive(action,true);
			}
		}

		/// <summary>
		/// Whether this <see cref="Node"/> contains a child with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A bool.</returns>
		public bool ContainsKey(string key)
		{
			foreach (Node child in Children)
			{
				if (child.Key == key)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the first child with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A Node.</returns>
		public Node FindChild(string key)
		{
			Node foundChild = null;
			ForEach(new System.Action<Node>((child) =>
			{
				if (child.Key == key)
				{
					foundChild = child;
					return;
				}
			}));

			return foundChild;
		}

		/// <summary>
		/// Gets a list of children with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A list of Nodes.</returns>
		public List<Node> FindChildren(string key)
		{
			List<Node> nodes = new List<Node>();

			ForEach(new System.Action<Node>((child) =>
			{
				if (child.Key == key)
				{
					nodes.Add(child);
				}
			}));

			return nodes;
		}

		/// <summary>
		/// Gets a list of unique keys used by children.
		/// </summary>
		/// <returns>A list of string.</returns>
		public List<string> GetKeys()
		{
			List<string> keys = new List<string>();

			ForEach(new System.Action<Node>((child) =>
			{
				if (!keys.Contains(child.Key))
				{
					keys.Add(child.Key);
				}
			}));

			return keys;
		}

		/// <summary>
		/// Gets the number of children with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>An int.</returns>
		public int KeyCount(string key)
		{
			return KeyCount(key,false);
		}

		/// <summary>
		/// Gets the number of children with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="recursive">If true, recursively search through all child nodes.</param>
		/// <returns>An int.</returns>
		public int KeyCount(string key,bool recursive = false)
		{
			int count = 0;

			System.Action<Node> action = new System.Action<Node>((node) =>
			{
				if (node.Key == key)
					count++;
			});

			if (recursive)
			{
				ForEachRecursive(action);
			}
			else
			{
				ForEach(action);
			}

			return count;
		}

		//// Strictly validates this node and its children. Recursive. Order of children and attributes must be identical.
		/// <summary>
		/// NOT IMPLEMENTED.
		/// </summary>
		/// <param name="other">The <paramref name="other"/>.</param>
		/// <param name="ignoreRoot">If true, ignore root node.</param>
		/// <returns>A bool.</returns>
		public bool Validate(Node other,bool ignoreSelf)
		{
			bool valid = true;

			if (Count != other.Count ||
				Key != other.Key ||
				Attributes.Count != other.Attributes.Count)
			{
				return false;
			}

			for (int i = 0; i < Attributes.Count; i++)
			{
				if (Attributes[i].Key != other.Attributes[i].Key)
				{
					return false;
				}
			}

			for (int i = 0; i < Count; i++)
			{
				valid &= this[i].Validate(other[i],false);
				if (!valid)
				{
					return false;
				}
			}

			return true;
		}

		//// Validates this node and its children. Recursive. Order of children and attributes do not matter.
		/// <summary>
		/// NOT IMPLEMENTED.
		/// </summary>
		/// <param name="other">The <paramref name="other"/>.</param>
		/// <param name="update">If true, copy missing nodes and attributes from <paramref name="other"/>.</param>
		/// <param name="trim">If true, remove nodes and attributes that don't appear in <paramref name="other"/>.</param>
		/// <param name="ignoreRoot">If true, ignore root node.</param>
		/// <returns>A bool.</returns>
		public bool Validate(Node other,bool update,bool trim,bool ignoreSelf)
		{
			bool valid = true;

			valid &= Key == other.Key;

			// Attribute check
			List<Attribute> missingAttributes = new List<Attribute>();
			for (int i = 0; i < other.Attributes.Count; i++)
			{
				if (!Attributes.ContainsKey(other.Attributes[i].Key))
				{
					missingAttributes.Add(new Attribute(other.Attributes[i]));
				}
			}

			// Add missing attributes
			if (update)
			{
				Attributes.Set(missingAttributes);
			}

			// Trim extra attributes
			if (trim)
			{
				List<Attribute> extraAttributes = new List<Attribute>();
				for (int i = 0; i < Attributes.Count; i++)
				{
					if (!other.Attributes.ContainsKey(Attributes[i].Key))
					{
						valid = false;
						extraAttributes.Add(new Attribute(Attributes[i]));
					}
				}

				Attributes.Remove(extraAttributes);
			}

			// Children
			List<string> otherKeys = other.GetKeys();
			foreach (string otherKey in otherKeys)
			{
				List<Node> nodesWithKey = FindChildren(otherKey);
				foreach (Node child in nodesWithKey)
				{

				}
			}

			return valid;
		}

		/// <summary>
		/// Returns whether another node shares the same key and value.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is Node)
			{
				return Key == ((Node)obj).Key && Value == ((Node)obj).Value;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// </summary>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			return Utilities.ToCGML(this,false);
		}

		#endregion
	}
}
