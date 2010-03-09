/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {
	/// <summary>
	/// Summary description for SqlDoubleMapping.
	/// </summary>
	public class SqlDoubleMapping : MemberMappingBase {

		public SqlDoubleMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Float);
		}
	}
}
