using System;

using Inform.Tests.Common;
using Inform;
using Inform.OleDb;
using NUnit.Framework;


namespace Inform.Tests.OleDb {

	[TestFixture]
	public class OleDbDataStoreTest : DataStoreTest  {
	
		override public DataStore CreateDataStore() {
			OleDbDataStore dataStore = new OleDbDataStore();
			dataStore.Name = "Main";
			dataStore.Connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User Id=Admin;" +
				@"Data source= D:\work\cvsroot\FluentComponents\Inform\db\Inform.mdb";
			return dataStore;
		}
	}
}
