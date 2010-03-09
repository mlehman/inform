using System;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbSqlByteMapping.
	/// </summary>
	public class OleDbSqlByteMapping : MemberMappingBase {

		public OleDbSqlByteMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.UnsignedTinyInt);
		} 

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlByte.Null;
			} else if (value is Byte){
				return new SqlByte((Byte)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlByte.Null;
				}
			} 
			return new SqlByte(Convert.ToByte(value));
		}

		public override object ConvertToStorageType(object value){
			
			if(value is SqlByte){
				SqlByte sqlvalue = (SqlByte)value;
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
