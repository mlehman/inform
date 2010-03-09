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
	public class SqlInt32Mapping : MemberMappingBase {

		public SqlInt32Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Int);
		}

		override public bool HasIdentity(object value){
			return (int)GetValue(value) > 0;
		}

	}
}
