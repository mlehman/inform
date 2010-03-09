/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace Inform {

	/// <summary>
	/// Specifies a mapping to a data source for a Type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true) ]
	public class TypeMappingAttribute : Attribute {

		private string tableName;
		private Type baseType;

		public TypeMappingAttribute() {
		}

		/// <summary>
		/// Allows the setting of a specific name for the table for a type.
		/// </summary>
		public string TableName {
			get { return tableName; }
			set { tableName = value; }
		}

		/// <summary>
		/// The Base Type used for inherited mappings. 
		/// </summary>
		public Type BaseType{
			get { return baseType; }
			set { baseType = value; }
		}
	}
}
