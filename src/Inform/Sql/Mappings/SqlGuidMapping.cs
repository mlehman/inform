/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.ProviderBase;

namespace Inform.Sql.Mappings {

	/// <summary>
	/// Summary description for SqlGuidMapping.
	/// </summary>
	public class SqlGuidMapping : MemberMappingBase {

		public SqlGuidMapping() {
			this.MemberDbType = new SqlMemberDbType(SqlDbType.UniqueIdentifier);
		}

		override public bool HasIdentity(object value){
			return GetValue(value) != null;
		}
	}
}
