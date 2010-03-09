/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {

	/// <summary>
	/// Summary description for SqlCharMapping.
	/// </summary>
	public class SqlCharMapping : MemberMappingBase {

		public SqlCharMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.NChar);
		}
	}
}
