/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {
	/// <summary>
	/// Summary description for SqlEnumMapping.
	/// </summary>
	public class SqlEnumMapping : MemberMappingBase {

		public SqlEnumMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.Int);
		}

		public override object ConvertFromStorageType(object value) {
			return Enum.ToObject(this.MappedType, Convert.ToInt32(value));
		}
	}
}
