using System;

using Inform.Tests.Common;
using Inform;
using Inform.Common;
using Inform.Sql;
using NUnit.Framework;

namespace Inform.Tests.Sql {

	[TestFixture]
	public class SqlConfigurationTest : ConfigurationTest  {
	
		override public DataStore CreateDataStore() {
			DataStoreServices.Initialize(@"D:\work\cvsroot\FluentComponents\Inform\src\Inform.Tests\Sql\Database.config");
			return DataStoreServices.Default;
		}
	}
}
