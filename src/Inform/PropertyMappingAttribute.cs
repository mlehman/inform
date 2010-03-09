using System;

namespace DataCommand {
	/// <summary>
	/// Summary description for PropertyMappingAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true) ]
	public class PropertyMappingAttribute : Attribute {

		private string columnName;
		private bool allowNulls;
		private int size;
		private int precision;
		private int scale;
		private bool ignore;



		public PropertyMappingAttribute() {
			this.AllowNulls = true;
		}

		public PropertyMappingAttribute(int size) {
			this.AllowNulls = true;
			this.Size = size;
		}

		public PropertyMappingAttribute(bool allowNulls) {
			this.AllowNulls = allowNulls;
		}

		public PropertyMappingAttribute(int size, bool allowNulls) {
			this.Size = size;
			this.AllowNulls = allowNulls;
		}

		/// <summary>
		/// The Size (or Length) of a primitive data type.
		/// </summary>
		//TODO: Rename to Length
		public int Size {
			get {
				return this.size;
			}
			set {
				this.size = value;
			}
		}

		public bool UseSize {
			get { return size != 0; }
		}

		/// <summary>
		/// The Precision of a primitive data type.
		/// </summary>
		public int Precision {
			get {
				return this.precision;
			}
			set {
				this.precision = value;
			}
		}

		public bool UsePrecision {
			get { return precision != 0; }
		}

		/// <summary>
		/// The Scale of a primitive data type.
		/// </summary>
		public int Scale {
			get {
				return this.scale;
			}
			set {
				this.scale = value;
			}
		}

		public bool UseScale {
			get { return scale != 0; }
		}

		/// <summary>
		/// Whether the primitive data type is nullable.
		/// </summary>
		public bool AllowNulls {
			get {
				return this.allowNulls;
			}
			set {
				this.allowNulls = value;
			}
		}

		/// <summary>
		/// The column name to map to the property.
		/// </summary>
		public string ColumnName {
			get {
				return this.columnName;
			}
			set {
				this.columnName = value;
			}
		}

		/// <summary>
		/// Ignore this property for typemapping.
		/// </summary>
		public bool Ignore {
			get {
				return this.ignore;
			}
			set {
				this.ignore = value;
			}
		}
	}
}
