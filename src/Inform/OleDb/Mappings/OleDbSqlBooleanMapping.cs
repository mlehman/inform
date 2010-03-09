using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlSqlDateTimeMapping.
	/// </summary>
	public class OleDbSqlBooleanMapping : MemberMappingBase {

		public OleDbSqlBooleanMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Boolean);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlBoolean.Null;
			} else if (value is Boolean){
				return new SqlBoolean((bool)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlBoolean.Null;
				}
			} 
			return new SqlBoolean(Convert.ToBoolean(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlBoolean){
				SqlBoolean sqlvalue = (SqlBoolean)value;
				if(sqlvalue.IsNull){
					return DBNull.Value;
				} else {
					return sqlvalue.Value;
				}
			} else {
				return base.ConvertToStorageType(value);
			}
		}

	}
}
