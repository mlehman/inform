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
	/// Summary description for SqlSqlDoubleMapping.
	/// </summary>
	public class SqlSqlDoubleMapping : MemberMappingBase {

		public SqlSqlDoubleMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Float);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDouble.Null;
			} else if (value is Double){
				return new SqlDouble(Convert.ToDouble(value));
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDouble.Null;
				}
				return new SqlDouble(Convert.ToDouble(value));
			} else {
				return Convert.ChangeType(value, MappedType);
			}
		}
	}
}
