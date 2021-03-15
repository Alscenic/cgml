// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGMLUnity
{
	using UnityEngine;

	/// <summary>
	/// Unity-specific utilities.
	/// </summary>
	public class UnityUtilities
	{
		/// <summary>
		/// Converts a Vector3 to a readable string.
		/// </summary>
		/// <param name="vector3">The vector3.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Vector3 vector3)
		{
			return vector3.x + "," + vector3.y + "," + vector3.z;
		}

		/// <summary>
		/// Converts a Quaternion to a readable string.
		/// </summary>
		/// <param name="vector3">The vector3.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Quaternion quaternion)
		{
			return quaternion.x + "," + quaternion.y + "," + quaternion.z + "," + quaternion.w;
		}

		/// <summary>
		/// Converts a Vector3 from a readable string.
		/// </summary>
		/// <param name="vector3">The vector3.</param>
		/// <returns>A string.</returns>
		public static bool FromReadable(string str,out Vector3 vector3)
		{
			vector3 = new Vector3();

			string[] split = str.Split(',');
			if (split.Length != 3)
				return false;

			if (float.TryParse(split[0],out float x)
				&& float.TryParse(split[1],out float y)
				&& float.TryParse(split[2],out float z))
			{
				vector3 = new Vector3(x,y,z);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Converts a Quaternion from a readable string.
		/// </summary>
		/// <param name="vector3">The vector3.</param>
		/// <returns>A string.</returns>
		public static bool FromReadable(string str,out Quaternion quaternion)
		{
			quaternion = Quaternion.identity;

			string[] split = str.Split(',');
			if (split.Length != 4)
				return false;

			if (float.TryParse(split[0],out float x)
				&& float.TryParse(split[1],out float y)
				&& float.TryParse(split[2],out float z)
				&& float.TryParse(split[3],out float w))
			{
				quaternion = new Quaternion(x,y,z,w);
				return true;
			}

			return false;
		}
	}
}
