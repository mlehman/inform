using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {
	/// <summary>
	/// Summary description for OleDbGuidMapping.
	/// </summary>
	public class OleDbGuidMapping : MemberMappingBase {

		public OleDbGuidMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Guid);
		}
	
		override public bool HasIdentity(object value){
			return GetValue(value) != null;
		}
	}
}
