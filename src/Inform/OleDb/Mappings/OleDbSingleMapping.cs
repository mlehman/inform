using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlInt32Mapping.
	/// </summary>
	public class OleDbSingleMapping : MemberMappingBase {

		public OleDbSingleMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Single);
		}
	}
}
