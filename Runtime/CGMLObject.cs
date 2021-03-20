// Code by Kyle Lamothe
// from current.gen Studios

using System.Collections.Generic;

namespace CGenStudios.CGML
{
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

		/// <summary>
		/// Gets the version number.
		/// </summary>
		public int Version { get; } = CGML.VERSION_LATEST;

		#endregion

		#region Public Constructors + Destructors

		/// <summary>
		/// Initializes a new <see cref="CGMLObject"/>. Does not create a copy of <paramref name="root"/>.
		/// </summary>
		/// <param name="root">The root node.</param>
		public CGMLObject(Node root,int version)
		{
			Root = root;
			Version = version;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CGMLObject"/> class.
		/// </summary>
		/// <param name="cgml">The CGML string.</param>
		public CGMLObject(string cgml)
		{
			Root = Utilities.Import(cgml);
		}

		/// <summary>
		/// Creates a deep copy of another <see cref="CGMLObject"/>.
		/// </summary>
		/// <param name="original">The original object.</param>
		public CGMLObject(CGMLObject original)
		{
			Root = new Node(original.Root);
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
			return Utilities.Export(Root,pretty);
		}

		/// <summary>
		/// Returns whether this <see cref="CGMLObject"/> contains the same hierarchy structure as <paramref name="other"/>.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <param name="update">If true, copy missing nodes and attributes from <paramref name="other"/>.</param>
		/// <param name="trim">If true, remove nodes and attributes that don't appear in <paramref name="other"/>.</param>
		/// <param name="ignoreRoot">If true, ignore root node.</param>
		/// <returns>A bool.</returns>
		public bool ValidateAgainst(CGMLObject other,bool update,bool trim,bool ignoreRoot)
		{
			return Root.Validate(other.Root,update,trim,ignoreRoot);
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			return ToCGML(false);
		}

		/// <summary>
		/// </summary>
		public override bool Equals(object obj)
		{
			return obj is CGMLObject @object &&
				   EqualityComparer<Node>.Default.Equals(Root,@object.Root);
		}

		/// <summary>
		/// </summary>
		public override int GetHashCode()
		{
			return -1490287827 + EqualityComparer<Node>.Default.GetHashCode(Root);
		}

		#endregion

	}
}
