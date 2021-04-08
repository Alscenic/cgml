// Code by Kyle Lamothe
// from current.gen Studios

namespace CGenStudios.CGML
{
	using System.Collections.Generic;
	using System.Text;

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

		public Attribute this[int index]
		{
			get
			{
				return this.m_Attributes[index];
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
		/// Adds a list of attributes.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public void Set(List<Attribute> attributes)
		{
			Set(attributes.ToArray());
		}

		/// <summary>
		/// Adds an array of attributes.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public void Set(params Attribute[] attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				Set(attributes[i]);
			}
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
		public void Remove(List<Attribute> attributes)
		{
			foreach (Attribute attribute in attributes)
			{
				Remove(attribute);
			}
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
