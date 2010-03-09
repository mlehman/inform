/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {

	/// <summary>
	/// Summary description for SqlInt32Mapping.
	/// </summary>
	public class SqlInt64Mapping : MemberMappingBase {

		public SqlInt64Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.BigInt);
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}
	}
}
