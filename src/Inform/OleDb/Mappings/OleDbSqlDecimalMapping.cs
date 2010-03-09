using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlDecimalMapping : MemberMappingBase {

		public OleDbSqlDecimalMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Decimal);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDecimal.Null;
			} else if (value is Decimal){
				return new SqlDecimal((Decimal)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDecimal.Null;
				}
			}
			return new SqlDecimal(Convert.ToDecimal(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlDecimal){
				SqlDecimal sqlvalue = (SqlDecimal)value;
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
