using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlSqlDateTimeMapping.
	/// </summary>
	public class OleDbSqlDateTimeMapping : MemberMappingBase {

		public OleDbSqlDateTimeMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.DBDate);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDateTime.Null;
			} else if (value is DateTime){
				return new SqlDateTime((DateTime)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDateTime.Null;
				}
				return new SqlDateTime(Convert.ToDateTime(value));
			} else {
				return Convert.ChangeType(value, MappedType);
			}
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlDateTime){
				SqlDateTime dateTime = (SqlDateTime)value;
				if(dateTime.IsNull){
					return DBNull.Value;
				} else {
					return dateTime.Value;
				}
			} else {
				return base.ConvertToStorageType(value);
			}
		}

	}
}
