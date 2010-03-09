using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {
	/// <summary>
	/// Summary description for SqlStringMapping.
	/// </summary>
	public class OleDbStringMapping : MemberMappingBase {

		public OleDbStringMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.VarWChar, 50);
		}

		override public bool HasIdentity(object value){
			return GetValue(value) != null;
		}
	
	}
}
