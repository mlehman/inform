using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlSingleMapping : MemberMappingBase {

		public OleDbSqlSingleMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Single);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlSingle.Null;
			} else if (value is Single){
				return new SqlSingle((Single)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlSingle.Null;
				}
			}
			return new SqlSingle(Convert.ToSingle(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlSingle){
				SqlSingle sqlvalue = (SqlSingle)value;
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
