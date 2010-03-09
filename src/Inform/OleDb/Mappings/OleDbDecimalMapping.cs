using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbDecimalMapping.
	/// </summary>
	public class OleDbDecimalMapping : MemberMappingBase {

		public OleDbDecimalMapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Decimal);
		}
	}
}
