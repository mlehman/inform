/*
 * SqlFindObjectCommandTest.cs	12/26/2002
 *
 * Copyright 2002 Screen Show, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using NUnit.Framework;
using Inform;
using Inform.Sql;
using Inform.Tests.Common;

namespace Inform.Tests.Sql {

	public class SqlDataStorageManagerTest : DataStorageManagerTest {

		override public DataStore CreateDataStore(){
			SqlDataStore dataStore = new SqlDataStore();
            dataStore.Connection.ConnectionString = "server=localhost;database=Inform;uid=development;pwd=d3v3lopm3nt";
			return dataStore;
		}

	}
}
