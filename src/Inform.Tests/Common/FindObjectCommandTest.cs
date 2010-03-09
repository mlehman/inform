/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using NUnit.Framework;
using Inform;

namespace Inform.Tests.Common
{
	/// <summary>
	/// Summary description for SqlFindObjectCommandTest.
	/// </summary>
	[TestFixture]
	public abstract class FindObjectCommandTest {

		DataStore dataStore;

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
//
//		[Test]
//		public void FindEmployeeCustomPopulate(){
//
//			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
//				typeof(Employee), "WHERE EmployeeID = '2'");
//			command.CustomPopulate = new CustomPopulate(PopulateEmployee);
//			Employee e = (Employee)command.Execute();
//			
//			//test
//			Assert.AreEqual("Donaldson", e.LastName,"Verify LastName");
//		}

		[Test]
		public void FindEmployeeFullSelect(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Employee), "SELECT * FROM Employees WHERE EmployeeID = '4'");
			Employee e = (Employee)command.Execute();
			
			//test
			Assert.AreEqual("Stevenson", e.LastName, "Verify LastName");

			command = dataStore.CreateFindObjectCommand(
				typeof(Employee), " SELECT * FROM Employees WHERE EmployeeID = '4'");
			e = (Employee)command.Execute();
			
			//test
			Assert.AreEqual("Stevenson", e.LastName, "Verify LastName");
		}

		[Test]
		public void UpdateEmployee(){


			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Employee), "WHERE EmployeeID = @EmployeeID");
			command.CreateInputParameter("@EmployeeID", "2");
			Employee e = (Employee)command.Execute();
			e.Salary = 120000;

			dataStore.Update(e);

			command = dataStore.CreateFindObjectCommand( typeof(Employee), "WHERE EmployeeID = @EmployeeID");
			command.CreateInputParameter("@EmployeeID", "2");
			e = (Employee)command.Execute();

			//test
			Assert.AreEqual(120000, e.Salary,"Verify LastName");


		}

//		/// <summary>
//		/// Custom population method for tests.
//		/// </summary>
//		private object PopulateEmployee(IDataReader reader){
//			
//			Employee e = new Employee();
//			e.FirstName = (string)reader["FirstName"];
//			e.LastName = (string)reader["LastName"];
//			e.Title = (string)reader["Title"];
//			e.Salary = (int)reader["Salary"];
//			return e;
//		}

		[Test]
		public void ObjectNotFound(){

			dataStore.Settings.FindObjectReturnsNull = false;

			IFindObjectCommand command = dataStore.CreateFindObjectCommand( typeof(Employee), "WHERE EmployeeID = '0'");
			
			//test command
			try {
				Employee e = (Employee)command.Execute();
				Assert.Fail("Should have thrown Exception");
			} catch (ObjectNotFoundException ofne) {
				//test passed
			} finally {
				dataStore.Settings.FindObjectReturnsNull = true;
			}
		}

		[Test]
		public void FindEmployee(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand( typeof(Employee), "WHERE EmployeeID = '2'");
			Employee e = (Employee)command.Execute();
			
			//test command
			Assert.AreEqual("Donaldson", e.LastName, "Verify LastName");
		}


		[Test]
		public void EmployeeReader(){

			IObjectAccessCommand cmd = dataStore.CreateObjectAccessCommand( typeof(Employee), "");
			IObjectReader reader = cmd.ExecuteObjectReader();
			Employee e;
			int count = 0;
			while(reader.Read()){
				e = (Employee)reader.GetObject();
				Console.WriteLine(e.LastName);
				count++;
			}
			reader.Close();
			Assert.AreEqual( 3, count,"Empoyees found");
		}

		[Test]
		public void EmployeeReaderEnumeration(){

			IObjectAccessCommand cmd = dataStore.CreateObjectAccessCommand( typeof(Employee), "");
			IObjectReader reader = cmd.ExecuteObjectReader();
			int count = 0;
			foreach(Employee e in reader){
				Console.WriteLine(e.LastName);
				count++;
			}
			Assert.AreEqual( 3, count,"Empoyees found");
			Assert.IsTrue(reader.IsClosed);
		}

		[Test]
		public void FindSimpleObject(){

			IFindObjectCommand command = dataStore.CreateFindObjectCommand(
				typeof(Posting), "WHERE ID = 1");
			Posting p = (Posting)command.Execute();

			//test
			Assert.AreEqual("Topic 1", p.Topic, "Verify Topic");
		}



		[Test]
		public void FindObjectPolymorphic(){

			try {
				dataStore.DeleteStorage(typeof(Circle));
			} catch {}
			dataStore.CreateStorage(typeof(Circle));

			try {
				dataStore.DeleteStorage(typeof(Rectangle));
			} catch {}
			dataStore.CreateStorage(typeof(Rectangle));

			try{
				dataStore.DeleteStorage(typeof(Shape));
			} catch {}


			Circle c = new Circle();
			c.Color = "Green";
			c.Radius = 5;
			dataStore.Insert(c);

			int shapeID = c.ShapeID;

			IFindObjectCommand cmd;
			cmd = dataStore.CreateFindObjectCommand( typeof(Shape), "WHERE Shapes.Color = @Color", true);
			cmd.CreateInputParameter("@Color", c.Color);
			c = (Circle)cmd.Execute();

			Assert.IsTrue( c.Color == "Green" && c.ShapeID == shapeID, "Checking Shape: {0}, {1} ",c.Color, c.ShapeID);

			cmd = dataStore.CreateFindObjectCommand(typeof(Circle), "WHERE Shapes.Color = @Color", true);
			cmd.CreateInputParameter("@Color", c.Color);
			c = (Circle)cmd.Execute();

			Assert.IsTrue( c.Color == "Green" && c.ShapeID == shapeID, "Checking Shape: {0}, {1} ",c.Color, c.ShapeID);

			dataStore.DeleteStorage(typeof(Circle));
			dataStore.DeleteStorage(typeof(Rectangle));
			dataStore.DeleteStorage(typeof(Shape));
			Console.WriteLine("Storage Deleted");

		}

		


		[Test]
		public void FindPrivateConstructor(){

			try{
				dataStore.DeleteStorage(typeof(Comment));
			} catch {}

			dataStore.CreateStorage(typeof(Comment));

			Comment c1 = new Comment("Test Comment");
			dataStore.Insert(c1);

			IFindObjectCommand command = dataStore.CreateFindObjectCommand( typeof(Comment), "WHERE Text LIKE @Text");
			command.CreateInputParameter("@Text",c1.Text);
			Comment c2 = (Comment)command.Execute();
			
			//test command
			Assert.AreEqual(c1.Text, c2.Text, "Verify Text");

			dataStore.DeleteStorage(typeof(Comment));
		}
	
	}

	
}
