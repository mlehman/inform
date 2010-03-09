/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

using Inform.Tests.Common;
using Inform;
using Inform.OleDb;
using NUnit.Framework;

namespace Inform.Tests.OleDb {

	[TestFixture]
	public class OleDbMappingTest : MappingTest  {
	
		override public DataStore CreateDataStore() {
			OleDbDataStore dataStore = new Inform.OleDb.OleDbDataStore();
			dataStore.Connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User Id=Admin;" +
				@"Data source= D:\work\cvsroot\FluentComponents\Inform\db\Inform.mdb;Mode=ReadWrite;";
			return dataStore;
		}
	}
}