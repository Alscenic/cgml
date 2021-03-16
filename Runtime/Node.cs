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
		public Node ForEachRecursive(System.Action<Node> action,bool invokeSelf = false)
		{
			if (invokeSelf)
			{
				action?.Invoke(this);
			}

			foreach (Node child in Children)
			{
				action?.Invoke(child);
				child.ForEachRecursive(action,false);
			}

			return this;
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
		/// <param name="recursive">If true, recursive.</param>
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

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets this node's parent.
		/// </summary>
		/// <param name="newParent">The new parent.</param>
		private void SetParent(Node newParent)
		{
			Parent = newParent;
		}

		#endregion
	}
}
