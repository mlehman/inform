/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

//TODO: Rename file

using System;
using System.Data;

namespace Inform {
	/// <summary>
	/// Specifies a mapping to a data source for a Type's field or property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true) ]
	public class MemberMappingAttribute : Attribute {

		private string columnName;
		private bool allowNulls;
		private string dbType;
		private int length;
		private int precision;
		private int scale;
		private bool ignore;
		private bool primaryKey;
		private bool identity;

		public MemberMappingAttribute() {
			this.AllowNulls = true;
		}

		public MemberMappingAttribute(int length) {
			this.AllowNulls = true;
			this.Length = length;
		}

		public MemberMappingAttribute(bool allowNulls) {
			this.AllowNulls = allowNulls;
		}

		public MemberMappingAttribute(int length, bool allowNulls) {
			this.Length = length;
			this.AllowNulls = allowNulls;
		}

		/// <summary>
		/// The primitive data type.
		/// </summary>
		public string DbType {
			get { return this.dbType; }
			set { dbType = value; }
		}

		public bool UseDbType {
			get { return dbType != null; }
		}

		/// <summary>
		/// The Length (or Size) of a primitive data type.
		/// </summary>
		public int Length {
			get { return this.length; }
			set { length = value; }
		}

		public bool UseLength { 
			get { return length != 0; }
		}

		/// <summary>
		/// The Precision of a primitive data type.
		/// </summary>
		public int Precision {
			get { return this.precision; }
			set { precision = value; }
		}

		public bool UsePrecision {
			get { return precision != 0; }
		}

		/// <summary>
		/// The Scale of a primitive data type.
		/// </summary>
		public int Scale {
			get { return this.scale; }
			set { scale = value; }
		}

		public bool UseScale { 
			get { return scale != 0; } 
		}

		/// <summary>
		/// Whether the primitive data type is nullable.
		/// </summary>
		public bool AllowNulls {
			get { return allowNulls; }
			set { allowNulls = value; }
		}

		/// <summary>
		/// The column name to map to the property.
		/// </summary>
		public string ColumnName {
			get { return columnName; }
			set { columnName = value; }
		}

		/// <summary>
		/// Whether this member is a primary key.
		/// </summary>
		public bool PrimaryKey {
			get { return primaryKey; }
			set { primaryKey = value; }
		}

		/// <summary>
		/// Whether this member is an auto-incrementing.
		/// </summary>
		/// <remarks>
		/// Setting Identity to true enables the Member to be populated with the value after an insert.
		/// </remarks>
		public bool Identity {
			get { return identity; }
			set { identity = value; }
		}

		/// <summary>
		/// Ignore this property for typemapping.
		/// </summary>
		public bool Ignore {
			get { return ignore; }
			set { ignore = value; }
		}
	}
}
