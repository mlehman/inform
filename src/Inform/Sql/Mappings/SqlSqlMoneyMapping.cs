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
	public class SqlSqlMoneyMapping : MemberMappingBase {

		public SqlSqlMoneyMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Money);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlMoney.Null;
			} else if (value is double){
				return new SqlMoney((double) value);
			} else if (value is decimal){
				return new SqlMoney((decimal) value);
			} else if (value is long){
				return new SqlMoney((long) value);
			} else if (value is int){
				return new SqlMoney((int) value);
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlMoney.Null;
				}
			}
			return new SqlMoney(Convert.ToDouble(value));
		}
	}
}
