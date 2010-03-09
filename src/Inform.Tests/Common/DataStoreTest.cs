using System;

using Inform;
using Inform.Tests.Common.PlainObjects;
using NUnit.Framework;

namespace Inform.Tests.Common {
	/// <summary>
	/// Summary description for DataStoreTest.
	/// </summary>
	public abstract class DataStoreTest {

		protected DataStore dataStore;

		public abstract DataStore CreateDataStore();

		[SetUp]
		public void InitializeConnection() {
			dataStore = CreateDataStore();
			dataStore.Settings.AutoGenerate = true;
		}

		[Test]
		public void CreateStorage() {
			dataStore.CreateStorage(typeof(Posting));
			Assert.IsTrue(dataStore.ExistsStorage(typeof(Posting)),"Check if Storage was created" );
		}

		[Test]
		public void EmptyTypeMappingTest() {
			try{
				dataStore.CreateStorage(typeof(Customer));
				Assert.Fail("Should throw TypeMappingException.");
			} catch (TypeMappingException te){
			} catch {
				Assert.Fail("Should throw TypeMappingException.");
			}
		}

//		[Test]
//		public void DataStoreServicesTest() {
//			DataStoreServices.RegisterDataStore(dataStore);
//			DataStore registeredDataStore = DataStoreServices.GetDataStore(dataStore.Name);
//			Assert.IsTrue(dataStore != registeredDataStore);
//			DataStoreServices.UnregisterDataStore(dataStore.Name);
//		}


		[Test]
		public void ExistsStorage() {
			dataStore.Settings.AutoGenerate = true;
			Assert.IsTrue(!dataStore.ExistsStorage(typeof(Posting)), "Table should not exist");
			dataStore.CreateStorage(typeof(Posting));
			Assert.IsTrue(dataStore.ExistsStorage(typeof(Posting)),"Table should exist");
		}

		[Test]
		public void InsertOrUpdate() {
			dataStore.Settings.AutoGenerate = true;
			dataStore.CreateStorage(typeof(Posting));

			Posting posting = new Posting();
			posting.Topic = "Insert";
			posting.Details = "InsertOrUpdate";

			dataStore.InsertOrUpdate(posting);

			int id =  posting.ID;

			posting = (Posting)dataStore.FindByPrimaryKey(typeof(Posting), posting.ID);
			Assert.AreEqual("Insert", posting.Topic,"Check Topic");

			posting.Topic = "Update";
			dataStore.InsertOrUpdate(posting);

			posting = (Posting)dataStore.FindByPrimaryKey(typeof(Posting), posting.ID);
			Assert.AreEqual("Update", posting.Topic,"Check Topic");
			Assert.AreEqual(id, posting.ID,"Check ID");

			dataStore.DeleteStorage(typeof(Posting));
		}


		[Test]
		public void Transaction() {
			dataStore.Settings.AutoGenerate = true;
			dataStore.CreateStorage(typeof(Posting));

			Posting posting = new Posting();
			posting.Topic = "Transaction Test";
			posting.Details = "Testing support of transactions.";

			dataStore.BeginTransaction();
			dataStore.Insert(posting);
			dataStore.RollbackTransaction();

			int count = dataStore.CreateFindCollectionCommand(typeof(Posting), null).Execute().Count;
			Assert.AreEqual(0,count,"Insert Rollback Test");

			dataStore.BeginTransaction();
			dataStore.Insert(posting);
			dataStore.CommitTransaction();

			count = dataStore.CreateFindCollectionCommand(typeof(Posting), null).Execute().Count;
			Assert.AreEqual(1,count,"Insert Rollback Test");

			posting.Topic = "Updated Transaction";

			dataStore.BeginTransaction();
			dataStore.Update(posting);
			dataStore.Update(posting);
			dataStore.RollbackTransaction();

			Posting updatedPosting = (Posting)dataStore.CreateFindObjectCommand(typeof(Posting),"WHERE ID = " + posting.ID).Execute();;
			Assert.AreEqual(updatedPosting.Topic,"Transaction Test","Update Rollback Test");

			dataStore.DeleteStorage(typeof(Posting));
		}

		[Test]
		public void CreateInheritenceStorage(){

			try {
				dataStore.DeleteStorage(typeof(Circle));
			} catch {}
			dataStore.CreateStorage(typeof(Circle));

			try {
				dataStore.DeleteStorage(typeof(Rectangle));
			} catch {}
			dataStore.CreateStorage(typeof(Rectangle));

			dataStore.DeleteStorage(typeof(Circle));
			dataStore.DeleteStorage(typeof(Rectangle));
			dataStore.DeleteStorage(typeof(Shape));

		}

		[TearDown]
		public void DeleteStorage(){
			//ensure deleted
			if(dataStore.ExistsStorage(typeof(Posting))){
				dataStore.DeleteStorage(typeof(Posting));
			}
		}
	}
}
