using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlInt16Mapping : MemberMappingBase {

		public OleDbSqlInt16Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.SmallInt);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt16.Null;
			} else if (value is Int16){
				return new SqlInt16((Int16)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt16.Null;
				}
			}
			return new SqlInt16(Convert.ToInt16(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlInt16){
				SqlInt16 sqlvalue = (SqlInt16)value;
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
