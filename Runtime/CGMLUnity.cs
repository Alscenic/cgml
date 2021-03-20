// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGMLUnity
{
#if UNITY_EDITOR || UNITY_STANDALONE

	using UnityEngine;

	/// <summary>
	/// Unity-specific utilities.
	/// </summary>
	public class UnityUtilities
	{

		#region Public Methods

		/// <summary>
		/// Converts a <see cref="Vector2"/> to a readable string.
		/// </summary>
		/// <param name="vector2">The <paramref name="vector2"/>.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Vector2 vector2)
		{
			return vector2.x + "," + vector2.y;
		}

		/// <summary>
		/// Attempts to convert a readable string to a <see cref="Vector2"/>.
		/// </summary>
		/// <param name="vector2">The <paramref name="vector2"/>.</param>
		/// <returns>A string.</returns>
		public static bool FromReadable(string str,out Vector2 vector2)
		{
			vector2 = new Vector2();

			string[] split = str.Split(',');
			if (split.Length != 2)
				return false;

			if (float.TryParse(split[0],out float x)
				&& float.TryParse(split[1],out float y))
			{
				vector2 = new Vector2(x,y);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Converts a <see cref="Vector3"/> to a readable string.
		/// </summary>
		/// <param name="vector3">The <paramref name="vector3"/>.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Vector3 vector3)
		{
			return vector3.x + "," + vector3.y + "," + vector3.z;
		}

		/// <summary>
		/// Attempts to convert a readable string to a <see cref="Vector3"/>.
		/// </summary>
		/// <param name="vector3">The <paramref name="vector3"/>.</param>
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
		/// Converts a <see cref="Vector4"/> to a readable string.
		/// </summary>
		/// <param name="vector4">The <paramref name="vector4"/>.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Vector4 vector4)
		{
			return vector4.x + "," + vector4.y + "," + vector4.z + "," + vector4.w;
		}

		/// <summary>
		/// Attempts to convert a readable string to a <see cref="Vector4"/>.
		/// </summary>
		/// <param name="vector4">The <paramref name="vector4"/>.</param>
		/// <returns>A string.</returns>
		public static bool FromReadable(string str,out Vector4 vector4)
		{
			vector4 = new Vector4();

			string[] split = str.Split(',');
			if (split.Length != 4)
				return false;

			if (float.TryParse(split[0],out float x)
				&& float.TryParse(split[1],out float y)
				&& float.TryParse(split[2],out float z)
				&& float.TryParse(split[2],out float w))
			{
				vector4 = new Vector4(x,y,z,w);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Converts a <see cref="Quaternion"/> to a readable string.
		/// </summary>
		/// <param name="vector3">The <paramref name="quaternion"/>.</param>
		/// <returns>A string.</returns>
		public static string ToReadable(Quaternion quaternion)
		{
			return quaternion.x + "," + quaternion.y + "," + quaternion.z + "," + quaternion.w;
		}

		/// <summary>
		/// Attempts to convert a readable string to a <see cref="Quaternion"/>.
		/// </summary>
		/// <param name="vector3">The <paramref name="quaternion"/>.</param>
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

		#endregion

	}

#else

	/// <summmary>
	/// Unity not detected.
	/// </summary>
	public class Disabled() { }

#endif
}
