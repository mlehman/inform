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
	/// Summary description for SqlSqlDateTimeMapping.
	/// </summary>
	public class SqlSqlDateTimeMapping : MemberMappingBase {

		public SqlSqlDateTimeMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.DateTime);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDateTime.Null;
			} else if (value is DateTime){
				return new SqlDateTime((DateTime)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDateTime.Null;
				}
				return new SqlDateTime(Convert.ToDateTime(value));
			} else {
				return Convert.ChangeType(value, MappedType);
			}
		}

	}
}
