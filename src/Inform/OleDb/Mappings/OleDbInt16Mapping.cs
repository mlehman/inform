using System;
using System.Data.OleDb;

using Inform.ProviderBase;

namespace Inform.OleDb.Mappings {

	/// <summary>
	/// Summary description for OleDbInt16Mapping.
	/// </summary>
	public class OleDbInt16Mapping : MemberMappingBase {

		public OleDbInt16Mapping() {
			this.MemberDbType = new OleDbMemberDbType(OleDbType.SmallInt);
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}
	}
}
