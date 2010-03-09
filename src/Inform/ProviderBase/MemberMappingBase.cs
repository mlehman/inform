/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Reflection;

using Inform.Common;

namespace Inform.ProviderBase {

	/// <summary>
	/// Aids implementation of the IMemberMapping interface. Inheritors of MemberMappingBase 
	/// implement a set of functions to specific to their MappedType, but inherit most of the 
	/// functionality needed to fully implement a MemberMapping.
	/// </summary>
	public class MemberMappingBase : IMemberMapping {

		private MemberInfo memberInfo;
		private String columnName;
		private MemberDbType memberDbType;
		private bool primaryKey;
		private bool identity;

		public MemberMappingBase() {
		}


		/// <summary>
		/// Creates a default mapping of to property with same name for ColumnName.
		/// </summary>
		/// <param name="property"></param>
		public MemberMappingBase(MemberInfo memberInfo, MemberDbType memberDbType) {
			this.memberInfo = memberInfo;
			this.memberDbType = memberDbType;
		}

		/// <summary>
		/// The PropertyInfo for the property being mapped.
		/// </summary>
		public MemberInfo MemberInfo{
			get { return memberInfo; }
			set { memberInfo = value; }
		}

		/// <summary>
		/// The PropertyInfo for the property being mapped.
		/// </summary>
		public Type MappedType{
			get {
				if(memberInfo is FieldInfo){
					return ((FieldInfo)memberInfo).FieldType; 
				} else {
					return ((PropertyInfo)memberInfo).PropertyType; 
				}
			}
		}

		/// <summary>
		/// The column name for the property.
		/// </summary>
		public string Name {
			get { return memberInfo.Name; }
		}

		/// <summary>
		/// The column name for the property.
		/// </summary>
		public string ColumnName {
			get { return columnName; }
			set { columnName = value; }
		}
	
		public MemberDbType MemberDbType {
			get { return memberDbType; }
			set { memberDbType = value; }
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

		virtual public bool HasIdentity(object value){
			throw new NotImplementedException("Can not determine set identity on " + this.MappedType.FullName);
		}

		virtual public object ConvertFromStorageType(object value){
			if (value is DBNull){
				return null;
			} else {
				return System.Convert.ChangeType(value, MappedType);
			}
		}

		/// <summary>
		/// Convert the object for storage into the database.
		/// </summary>
		virtual public object ConvertToStorageType(object value){
			if(value == null){
				value = DBNull.Value;
			}
			return value;
		}

		/// <summary>
		/// Retrieve a value from object in Storage Type
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public object GetValue(object target){
			object value;
			
			if(memberInfo is FieldInfo){
				value = ((FieldInfo)memberInfo).GetValue(target);
			} else {
				value = ((PropertyInfo)memberInfo).GetValue(target, null);
			}
			value = ConvertToStorageType(value);
			return value;
		}

		/// <summary>
		/// Set the value on the object with this value with Storage Type.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="value"></param>
		public void SetValue(object target, object value){
			try {
				value = ConvertFromStorageType(value);
				if(memberInfo is FieldInfo){
					((FieldInfo)memberInfo).SetValue(target,value);
				} else {
					((PropertyInfo)memberInfo).SetValue(target,value,null);
				}
			} 
			catch (Exception e) {
				string msg = string.Format("Exception setting Property value '{0}' from column '{1}'", Name, columnName);
				throw new ApplicationException(msg, e);
			}
		}

	}
}
