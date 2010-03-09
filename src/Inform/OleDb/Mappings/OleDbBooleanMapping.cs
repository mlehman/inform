using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {
	/// <summary>
	/// Summary description for SqlStringMapping.
	/// </summary>
	public class OleDbBooleanMapping : MemberMappingBase {

		public OleDbBooleanMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Boolean);
		}
	
	}
}
