using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbCharMapping.
	/// </summary>
	public class OleDbCharMapping : MemberMappingBase {

		public OleDbCharMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.WChar, 1);
		}
	}
}
