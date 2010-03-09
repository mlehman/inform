using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlInt64Mapping : MemberMappingBase {

		public OleDbSqlInt64Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Decimal);
			this.MemberDbType.Precision = 19;

		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt64.Null;
			} else if (value is Int64){
				return new SqlInt64((Int64)value);
			} else if (value is Int64){
				return new SqlInt64((Int64)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt64.Null;
				}
			}
			return new SqlInt64(Convert.ToInt64(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlInt64){
				SqlInt64 sqlvalue = (SqlInt64)value;
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
