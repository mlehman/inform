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
	public class SqlSqlDecimalMapping : MemberMappingBase {

		public SqlSqlDecimalMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Decimal);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				return SqlDecimal.Null;
			} else if (value is Decimal){
				return new SqlDecimal(Convert.ToDecimal(value));
			} else if (value is string){
				if (((string)value).Length < 1){
					return SqlDecimal.Null;
				}
				return new SqlDecimal(Convert.ToDecimal(value));
			} else {
				return Convert.ChangeType(value, MappedType);
			}
		}

		/// <summary>
		/// Convert the object for storage into the database.
		/// </summary>
		public override object ConvertToStorageType(object value){
			if(value == null){
				value = DBNull.Value;
			} else if(value is SqlDecimal){
				SqlDecimal sqlDecimal = (SqlDecimal)value;
				if(sqlDecimal.IsNull){
					value = DBNull.Value;
				}
			}
			return value;
		}
	}
}
