// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	using System.Collections.Generic;

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

		/// <summary>
		/// Creates a copy of another <see cref="Attribute"/>.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public Attribute(Attribute original)
		{
			Key = original.Key;
			Value = original.Value;
			Serialize = original.Serialize;
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

		/// <summary>
		/// </summary>
		public override bool Equals(object obj)
		{
			if (!(obj is Attribute))
				return false;
			return ((Attribute)obj).Key == Key && ((Attribute)obj).Value.Equals(Value);
		}

		/// <summary>
		/// </summary>
		public override int GetHashCode()
		{
			int hashCode = 206514262;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
			return hashCode;
		}

		#endregion

	}
}
