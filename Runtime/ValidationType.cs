// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	/// <summary>
	/// Node tree validation type.
	/// </summary>
	[System.Flags]
	public enum ValidationType
	{
		AddMissing,

		RemoveExtra,

		OverwriteExisting,
	}
}
