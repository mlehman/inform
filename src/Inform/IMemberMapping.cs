/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Reflection;

using Inform.Common;


namespace Inform {

	/// <summary>
	/// The mapping to a data source for a Type's field or property.
	/// </summary>
	public interface IMemberMapping {

		/// <summary>
		/// The member name.
		/// </summary>
		string Name {
			get;
		}

		/// <summary>
		/// The member name.
		/// </summary>
		Type MappedType {
			get;
		}

		/// <summary>
		/// The MemberInfo for the property being mapped.
		/// </summary>
		MemberInfo MemberInfo{
			get;
			set;
		}


		/// <summary>
		/// The column name for the member.
		/// </summary>
		string ColumnName {
			get;
			set;
		}

		MemberDbType MemberDbType {
			get;
			set;
		}

		/// <summary>
		/// Whether this member is a primary key.
		/// </summary>
		bool PrimaryKey {
			get;
			set;
		}

		/// <summary>
		/// Whether this member is an auto-incrementing.
		/// </summary>
		/// <remarks>
		/// Setting Identity to true enables the Member to be populated with the value after an insert.
		/// </remarks>
		bool Identity {
			get;
			set;
		}

		object ConvertFromStorageType(object value);
		object ConvertToStorageType(object value);
		object GetValue(object target);
		void SetValue(object target, object value);
		bool HasIdentity(object value);

	}
}
