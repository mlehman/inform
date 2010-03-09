/*
 * OleDbFindObjectCommandTest.cs	12/26/2002
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
using Inform.OleDb;
using Inform.Tests.Common;

namespace Inform.Tests.OleDb {

	public class OleDbFindObjectCommandTest : FindObjectCommandTest {

		override public DataStore CreateDataStore(){
			OleDbDataStore dataStore = new Inform.OleDb.OleDbDataStore();
			dataStore.Connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User Id=Admin;" +
				@"Data source= D:\work\cvsroot\FluentComponents\Inform\db\Inform.mdb;Mode=ReadWrite;";
			return dataStore;
		}
			
	
	}

	
}
