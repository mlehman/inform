/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {

	/// <summary>
	/// Summary description for SqlInt16Mapping.
	/// </summary>
	public class SqlInt16Mapping : MemberMappingBase {

		public SqlInt16Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.SmallInt);
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}
	}
}
