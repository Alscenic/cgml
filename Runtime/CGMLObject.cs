// Code by Kyle Lamothe
// from current.gen Studios

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
		/// Returns whether this <see cref="CGMLObject"/> contains the same hierarchy structure as <paramref name="other"/> and copies any missing keys if not.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns>A bool.</returns>
		public bool ValidateKeysAgainst(CGMLObject other)
		{
			return ValidateKeysAgainst(other,true);
		}

		/// <summary>
		/// Returns whether this <see cref="CGMLObject"/> contains the same hierarchy structure as <paramref name="other"/>.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <param name="update">If true, copy keys to this <see cref="CGMLObject"/>.</param>
		/// <returns>A bool.</returns>
		public bool ValidateKeysAgainst(CGMLObject other,bool update)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// </summary>
		public override string ToString()
		{
			return ToCGML(false);
		}

		#endregion

	}
}
