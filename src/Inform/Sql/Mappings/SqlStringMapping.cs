/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {
	/// <summary>
	/// Summary description for SqlStringMapping.
	/// </summary>
	public class SqlStringMapping : MemberMappingBase {

		public SqlStringMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.NVarChar);
		}
	
	}
}
