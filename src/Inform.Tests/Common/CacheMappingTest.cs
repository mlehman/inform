using System;

using Inform;
using NUnit.Framework;

using Inform.Tests.Common.PlainObjects;

namespace Inform.Tests.Common {
	/// <summary>
	/// Summary description for CacheMappingTest.
	/// </summary>
	public abstract class CacheMappingTest {
		protected DataStore dataStore;

		public abstract DataStore CreateDataStore();
		[SetUp]
		public void InitializeConnection() {

			dataStore = CreateDataStore();
			dataStore.Settings.AutoGenerate = true;

			dataStore.CreateStorage(typeof(Manager));
			dataStore.CreateStorage(typeof(Employee));
			dataStore.CreateStorage(typeof(Project));
			dataStore.CreateStorage(typeof(Task));
			dataStore.CreateStorage(typeof(Posting));

			InsertTestData();

		}

		private void InsertTestData(){
			Employee e = new Employee();
			e.EmployeeID = "2";
			e.FirstName = "Sam";
			e.LastName = "Donaldson";
			e.Title = "Director";
			e.Salary = 70000;
			dataStore.Insert(e);

			e = new Employee();
			e.EmployeeID = "3";
			e.FirstName = "Dave";
			e.LastName = "Donner";
			e.Title = "Engineer";
			e.Salary = 50000;
			dataStore.Insert(e);

			e = new Employee();
			e.EmployeeID = "4";
			e.FirstName = "Bill";
			e.LastName = "Stevenson";
			e.Title = "Analyst";
			e.Salary = 40000;
			dataStore.Insert(e);

			Manager m = new Manager();
			m.EmployeeID = "5";
			m.FirstName = "Sam";
			m.LastName = "Smith";
			m.Certified = true;
			m.Title = "Director";
			m.Salary = 90000;
			dataStore.Insert(m);

			m = new Manager();
			m.EmployeeID = "6";
			m.FirstName = "Ralph";
			m.LastName = "Johnson";
			m.Certified = true;
			m.Title = "Director";
			m.Salary = 95000;
			dataStore.Insert(m);

			Project p = new Project();
			p.Name = "ProjectX";
			p.ProjectID = "1";
			p.ManagerID = "5";
			dataStore.Insert(p);

			Task t = new Task();
			t.ProjectID = p.ProjectID;
			t.Description = "Task 1";
			dataStore.Insert(t);

			t.Description = "Task 2";
			dataStore.Insert(t);

			Posting ps = new Posting();
			ps.Topic = "Topic 1";
			ps.Details = "Some details.";
			dataStore.Insert(ps);

			ps = new Posting();
			ps.Topic = "Topic w";
			ps.Details = "Some details.";
			dataStore.Insert(ps);
		}

		[TearDown]
		public void DeleteStorage() {
			
			
			
			try{
				dataStore.DeleteStorage(typeof(Task));
			} catch(Exception e){
				Console.Write(e.Message);
			}

			try{
				dataStore.DeleteStorage(typeof(Project));
			} catch(Exception e){
				Console.Write(e.Message);
			}

			try{
				dataStore.DeleteStorage(typeof(Manager)); //TODO? Constraint on employee here?
			} catch(Exception e){
				Console.Write(e.Message);
			}

			try{
				dataStore.DeleteStorage(typeof(Employee));
			} catch(Exception e){
				Console.Write(e.Message);
			}

			try{
				dataStore.DeleteStorage(typeof(Posting));
			} catch(Exception e){
				Console.Write(e.Message);
			}
		}

		[Test]
		public void SetContext(){

			Project p = new Project();
			p.ProjectID = "1";
			dataStore.SetContext(p);
			Assert.AreEqual(2, p.Tasks.Count,"Count Task Items");
		}

		[Test]
		public void FindCachedObject(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Project), "WHERE ProjectID = '1'");
			Project p = (Project)command.Execute();

			//test
			Assert.AreEqual("Smith", p.Manager.LastName, "Verify LastName");
		}

		[Test]
		public void FindCachedCollection(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Project), "WHERE ProjectID = '1'");
			Project p = (Project)command.Execute();

			Assert.AreEqual(2, p.Tasks.Count,"Count Task Items");
		}

		[Test]
		public void FindBidirectionalCaches(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Project), "WHERE ProjectID = '1'");
			Project p = (Project)command.Execute();
			Task t =  (Task)p.Tasks[0];
			Assert.AreEqual(p.ProjectID, t.Project.ProjectID,"Check Project ID's");
		}

		
		[Test]
		public void SetCachedObject(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Project), "WHERE ProjectID = '1'");
			Project p = (Project)command.Execute();

			Manager m1 = p.Manager;

			//test
			Assert.AreEqual("Smith", p.Manager.LastName, "Verify LastName");

			command = dataStore.CreateFindObjectCommand(
				typeof(Manager), "WHERE EmployeeID = '6'");
			Manager m2 = (Manager)command.Execute();

			p.Manager = m2;

			Assert.AreEqual("Johnson", p.Manager.LastName, "Verify LastName after update");

			//try update and refresh too
			dataStore.Update(p);
			command = dataStore.CreateFindObjectCommand(
				typeof(Project), "WHERE ProjectID = '1'");
			p = (Project)command.Execute();

			Assert.AreEqual("Johnson", p.Manager.LastName, "Verify LastName");

			p.Manager = m1;

			Assert.AreEqual("Smith", p.Manager.LastName, "Verify LastName");
		}

		[Test]
		public void AddCachedCollection(){

			Project p = new Project();
			p.Name = "ProjectZ";
			p.ProjectID = "2";
			p.ManagerID = "5";
			dataStore.Insert(p);

			Task t = new Task();
			t.Description = "Task 1";
			p.Tasks.Add(t);

			t = new Task();
			t.Description = "Task 2";
			p.Tasks.Add(t);

			Assert.AreEqual(2, p.Tasks.Count,"Count Task Items");
		}


	}
}
