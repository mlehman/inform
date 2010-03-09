using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlInt32Mapping.
	/// </summary>
	public class OleDbDoubleMapping : MemberMappingBase {

		public OleDbDoubleMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Double);
		}
	}
}
