using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for SqlInt32Mapping.
	/// </summary>
	public class OleDbInt32Mapping : MemberMappingBase {

		public OleDbInt32Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.Integer);
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}
	}
}
