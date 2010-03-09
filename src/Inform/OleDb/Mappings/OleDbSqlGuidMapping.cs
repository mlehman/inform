using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlGuidMapping.
	/// </summary>
	public class OleDbSqlGuidMapping : MemberMappingBase {

		public OleDbSqlGuidMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Guid);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlGuid.Null;
			} else if (value is Guid){
				return new SqlGuid((Guid)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlGuid.Null;
				}
			} 
			return Convert.ChangeType(value, typeof(SqlGuid));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlGuid){
				SqlGuid sqlvalue = (SqlGuid)value;
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
