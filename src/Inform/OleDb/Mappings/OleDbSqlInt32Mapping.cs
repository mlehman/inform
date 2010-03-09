using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlInt32Mapping : MemberMappingBase {

		public OleDbSqlInt32Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Integer);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt32.Null;
			} else if (value is Int32){
				return new SqlInt32((Int32)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt32.Null;
				}
			}
			return new SqlInt32(Convert.ToInt32(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlInt32){
				SqlInt32 sqlvalue = (SqlInt32)value;
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
