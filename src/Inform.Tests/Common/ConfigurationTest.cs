using System;

using Inform;
using NUnit.Framework;

using Inform.Tests.Common.PlainObjects;

namespace Inform.Tests.Common {

	/// <summary>
	/// Summary description for ConfigurationTest.
	/// </summary>
	public abstract class ConfigurationTest {

		protected DataStore dataStore;

		public abstract DataStore CreateDataStore();

		[SetUp]
		public void InitializeConnection() {
			dataStore = CreateDataStore();
		}

		[Test]
		public void ExistsStorage() {
			Assert.IsTrue(dataStore.ExistsStorage(typeof(Customer)),"Table should exist");
		}

		[TearDown]
		public void DeleteStorage(){
			//ensure deleted
			if(dataStore.ExistsStorage(typeof(Customer))){
				dataStore.DeleteStorage(typeof(Customer));
			}
			DataStoreServices.UnregisterDataStore("Main");
		}
	}
}
