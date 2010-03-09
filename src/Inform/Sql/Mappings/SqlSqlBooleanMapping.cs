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
	/// Summary description for SqlSqlBooleanMapping.
	/// </summary>
	public class SqlSqlBooleanMapping : MemberMappingBase {
		public SqlSqlBooleanMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Bit);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlBoolean.Null;
			} else if (value is bool){
				return new SqlBoolean((bool)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlBoolean.Null;
				}
			}
			return new SqlBoolean(Convert.ToBoolean(value));
		}
	}
}
