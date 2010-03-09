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
	/// Summary description for SqlDateTimeMapping.
	/// </summary>
	public class SqlDateTimeMapping : MemberMappingBase {

		public SqlDateTimeMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.DateTime);
		}

		public override object ConvertFromStorageType(object value) {
			if(value is DBNull){
				string msg = string.Format("Exception getting Property value '{0}' from column '{1}', DateTime is not nullable.", Name, ColumnName);
				throw new ApplicationException(msg);
			}
			return base.ConvertFromStorageType(value);
		}
	}
}
