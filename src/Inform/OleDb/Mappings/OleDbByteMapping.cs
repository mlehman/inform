using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbByteMapping.
	/// </summary>
	public class OleDbByteMapping : MemberMappingBase {

		public OleDbByteMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.SmallInt);
		}
	}
}
