using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {
	/// <summary>
	/// Summary description for SqlStringMapping.
	/// </summary>
	public class OleDbDateTimeMapping : MemberMappingBase {

		public OleDbDateTimeMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Date);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				string msg = string.Format("Exception getting Property value '{0}' from column '{1}', DateTime is not nullable.", Name, ColumnName);
				throw new ApplicationException(msg);
			}
			return base.ConvertFromStorageType(value);
		}
	
	}
}
