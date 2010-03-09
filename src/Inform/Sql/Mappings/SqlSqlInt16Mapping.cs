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
	/// Summary description for SqlSqlInt16Mapping.
	/// </summary>
	public class SqlSqlInt16Mapping : MemberMappingBase {

		public SqlSqlInt16Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.SmallInt);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt16.Null;
			} else if (value is Int16){
				return new SqlInt16((Int16)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt16.Null;
				}
			}
			return new SqlInt16(Convert.ToInt16(value));
		}
	}
}
