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
	/// Summary description for SqlSqlGuidMapping.
	/// </summary>
	public class SqlSqlGuidMapping : MemberMappingBase {
		public SqlSqlGuidMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.UniqueIdentifier);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlGuid.Null;
			} else if (value is Guid){
				return new SqlGuid((Guid)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlGuid.Null;
				}
			}
			return Convert.ChangeType(value, typeof(SqlGuid));
		}
	}
}
