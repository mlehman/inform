using System;

using Inform.Tests.Common;
using Inform;
using Inform.Sql;
using NUnit.Framework;

namespace Inform.Tests.Sql {

	[TestFixture]
	public class SqlDataStoreTest : DataStoreTest  {
	
		override public DataStore CreateDataStore() {
			SqlDataStore dataStore = new Inform.Sql.SqlDataStore();
			dataStore.Name = "Main";
            dataStore.Connection.ConnectionString = "server=localhost;database=Inform;uid=development;pwd=d3v3lopm3nt";
			return dataStore;
		}
	}
}
