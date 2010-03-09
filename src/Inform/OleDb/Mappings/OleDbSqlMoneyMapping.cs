using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlMoneyMapping.
	/// </summary>
	public class OleDbSqlMoneyMapping : MemberMappingBase {

		public OleDbSqlMoneyMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Currency);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlMoney.Null;
			} else if (value is double){
				return new SqlMoney((double) value);
			} else if (value is decimal){
				return new SqlMoney((decimal) value);
			} else if (value is long){
				return new SqlMoney((long) value);
			} else if (value is int){
				return new SqlMoney((int) value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlMoney.Null;
				}
			}
			return new SqlMoney(Convert.ToDouble(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlMoney){
				SqlMoney sqlvalue = (SqlMoney)value;
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
