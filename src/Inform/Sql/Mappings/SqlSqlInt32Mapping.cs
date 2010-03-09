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
	/// Summary description for SqlSqlInt32Mapping.
	/// </summary>
	public class SqlSqlInt32Mapping : MemberMappingBase {

		public SqlSqlInt32Mapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Int);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlInt32.Null;
			} else if (value is Int32){
				return new SqlInt32((int)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlInt32.Null;
				}
			}
			return new SqlInt32(Convert.ToInt32(value));
		}
	}
}
