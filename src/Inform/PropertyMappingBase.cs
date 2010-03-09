/*
 * SqlDataAccessCommand.cs	12/26/2002
 *
 * Copyright 2002 Screen Show, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Reflection;

namespace DataCommand {

	/// <summary>
	/// Summary description for PropertyMapping.
	/// </summary>
	public class PropertyMappingBase : IPropertyMapping {

		protected PropertyInfo property;
		protected String columnName;
		protected PropertyDbType propertyDbType;

		public PropertyMappingBase() {
		}


		/// <summary>
		/// Creates a default mapping of to property with same name for ColumnName.
		/// </summary>
		/// <param name="property"></param>
		public PropertyMappingBase(PropertyInfo property, PropertyDbType propertyDbType) {
			this.property = property;
			this.propertyDbType = propertyDbType;
		}

		/// <summary>
		/// The PropertyInfo for the property being mapped.
		/// </summary>
		public PropertyInfo PropertyInfo{
			get { return property; }
			set { property = value; }
		}

		/// <summary>
		/// The column name for the property.
		/// </summary>
		public string ColumnName {
			get { return columnName; }
			set { columnName = value; }
		}
	
		public PropertyDbType PropertyDbType {
			get { return propertyDbType; }
			set { propertyDbType = value; }
		}


		virtual public object ConvertFromStorageType(object value){
			if (value is DBNull){
				return null;
			} else {
				return System.Convert.ChangeType(value, PropertyInfo.PropertyType);
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
			object value = property.GetValue(target, null);
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
				property.SetValue(target,value,null);
			} 
			catch (Exception e) {
				string msg = string.Format("Exception setting Property value '{0}' from column '{1}'",property.Name, columnName);
				throw new ApplicationException(msg, e);
			}
		}

	}
}
