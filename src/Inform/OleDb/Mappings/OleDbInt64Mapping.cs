using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlInt32Mapping.
	/// </summary>
	public class OleDbInt64Mapping : MemberMappingBase {

		public OleDbInt64Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Decimal);
			this.MemberDbType.Precision = 19;
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}
	}
}
