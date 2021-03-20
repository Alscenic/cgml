// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
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

		/// <summary>
		/// Pass this when dealing with version numbers.
		/// </summary>
		public const int VERSION_AUTO = -1;

		/// <summary>
		/// Pass this when dealing with version numbers.
		/// </summary>
		public const int VERSION_0_X_X = 0;

		/// <summary>
		/// Pass this when dealing with version numbers.
		/// </summary>
		public const int VERSION_1_0_X = 1;

		/// <summary>
		/// The latest version. Do not use - pick the version number you want and stick with it.
		/// </summary>
		public const int VERSION_LATEST = VERSION_0_X_X;

		#endregion

	}
}
