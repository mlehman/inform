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
	/// Summary description for SqlSqlSingleMapping.
	/// </summary>
	public class SqlSqlSingleMapping : MemberMappingBase {

		public SqlSqlSingleMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Real);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlSingle.Null;
			} else if (value is Single){
				return new SqlSingle(Convert.ToSingle(value));
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlSingle.Null;
				}
				return new SqlSingle(Convert.ToSingle(value));
			} else {
				return Convert.ChangeType(value, MappedType);
			}
		}
	}
}
