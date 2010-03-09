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
	/// Summary description for SqlSqlInt64Mapping.
	/// </summary>
	public class SqlSqlInt64Mapping : MemberMappingBase {

		public SqlSqlInt64Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.BigInt);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt64.Null;
			} else if (value is Int64){
				return new SqlInt64((Int64)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt64.Null;
				}
			}
			return new SqlInt32(Convert.ToInt32(value));
		}
	}
}
