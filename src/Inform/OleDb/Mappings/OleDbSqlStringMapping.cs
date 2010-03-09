using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlStringMapping : MemberMappingBase {

		public OleDbSqlStringMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.VarWChar,50);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlString.Null;
			} else if (value is string){
				return new SqlString((string)value);
			} else {
				return Convert.ChangeType(value, typeof(SqlString));
			}
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlString){
				SqlString sqlvalue = (SqlString)value;
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
