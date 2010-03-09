using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlDecimalMapping.
	/// </summary>
	public class OleDbSqlDoubleMapping : MemberMappingBase {

		public OleDbSqlDoubleMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Double);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDouble.Null;
			} else if (value is Double){
				return new SqlDouble((Double)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDouble.Null;
				}
			}
			return new SqlDouble(Convert.ToDouble(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlDouble){
				SqlDouble sqlvalue = (SqlDouble)value;
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
