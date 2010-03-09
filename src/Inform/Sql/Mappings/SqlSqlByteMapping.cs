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
	public class SqlSqlByteMapping : MemberMappingBase {
		public SqlSqlByteMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.TinyInt);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlByte.Null;
			} else if (value is Byte){
				return new SqlByte((Byte)value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlByte.Null;
				}
			}
			return new SqlByte(Convert.ToByte(value));
		}
	}
}
