/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data.SqlTypes;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {

	/// <summary>
	/// Summary description for SqlSqlStringMapping.
	/// </summary>
	public class SqlSqlStringMapping : MemberMappingBase {

		public SqlSqlStringMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.NVarChar);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlString.Null;
			} else if (value is string){
				return new SqlString((string)value);
			} else {
				return Convert.ChangeType(value, typeof(SqlString));
			}
		}
	}
}
