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

namespace Inform.Tests.Common {

	/// <summary>
	/// Summary description for SqlFindCollectionCommand.
	/// </summary>
	[TestFixture]
	public abstract class FindCollectionCommandTest {

		DataStore dataStore;

		public abstract DataStore CreateDataStore();
		

		[SetUp]
		public void InitializeConnection() {
			dataStore = CreateDataStore();
			dataStore.Settings.AutoGenerate = true;

			dataStore.CreateStorage(typeof(Employee));

            Employee e = new Employee();
            e.EmployeeID = "1";
            e.FirstName = "Don";
            e.LastName = "Paulson";
            e.Title = "Director";
            e.Salary = 70000;
            dataStore.Insert(e);

			e = new Employee();
			e.EmployeeID = "2";
			e.FirstName = "Sam";
			e.LastName = "Donaldson";
			e.Title = "Director";
			e.Salary = 70000;
			dataStore.Insert(e);

			e = new Employee();
			e.EmployeeID = "3";
			e.FirstName = "Jim";
			e.LastName = "Davis";
			e.Title = "Director";
			e.Salary = 65000;
			dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "4";
            e.FirstName = "Ralph";
            e.LastName = "Emerson";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "5";
            e.FirstName = "Steve";
            e.LastName = "Johnson";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "6";
            e.FirstName = "Bonnie";
            e.LastName = "Illes";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "7";
            e.FirstName = "Tim";
            e.LastName = "Valdez";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "8";
            e.FirstName = "Peter";
            e.LastName = "Vanders";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "9";
            e.FirstName = "Sallie";
            e.LastName = "Johnston";
            e.Title = "Director";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "10";
            e.FirstName = "Mary";
            e.LastName = "Stucker";
            e.Title = "Marketing";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "11";
            e.FirstName = "Tim";
            e.LastName = "Valdez";
            e.Title = "Sales";
            e.Salary = 65000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "12";
            e.FirstName = "Tim";
            e.LastName = "Valdez";
            e.Title = "Technician";
            e.Salary = 70000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "13";
            e.FirstName = "Tim";
            e.LastName = "Valdez";
            e.Title = "Sales";
            e.Salary = 45000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "14";
            e.FirstName = "Patti";
            e.LastName = "Rollins";
            e.Title = "Secretary";
            e.Salary = 25000;
            dataStore.Insert(e);

            e = new Employee();
            e.EmployeeID = "15";
            e.FirstName = "Betty";
            e.LastName = "Alexander";
            e.Title = "Developer";
            e.Salary = 75000;
            dataStore.Insert(e);
		}

		[TearDown]
		public void DeleteStorage() {
			dataStore.DeleteStorage(typeof(Employee));
		}

		
		[Test]
		public void FindAllEmployee(){

			//Create command
			IList list  = dataStore.CreateFindCollectionCommand( typeof(Employee), "ORDER BY LastName").Execute();

            Console.Out.WriteLine("-- All");
            foreach (Employee e in list) {
                Console.Out.WriteLine(e.LastName);
            }

			//test command
			Assert.IsTrue(list.Count == 15,"Verify found 15 Employees");
            Assert.AreEqual("Davis", ((Employee)list[1]).LastName, "Verify Last Name");
		}

        [Test]
        public void FindEmployeesPaged() {

            //Create command
            int count = 0;

            IList list = dataStore.CreateFindCollectionCommand(typeof(Employee), "ORDER BY LastName").Execute(0, 5, out count);

            //test command
            Assert.AreEqual(5, list.Count, "Returned 5 Employees");
            Assert.AreEqual("Alexander", ((Employee)list[0]).LastName, "Verify Last Name");
            Assert.AreEqual(15, count, "Verify found 15 Employees");

            list = dataStore.CreateFindCollectionCommand(typeof(Employee), "ORDER BY LastName").Execute(1, 5, out count);

            //test command
            Assert.AreEqual(5, list.Count, "Returned 5 Employees");
            Assert.AreEqual("Johnston", ((Employee)list[1]).LastName, "Verify Last Name");
            Assert.AreEqual(15, count, "Verify found 15 Employees");

            list = dataStore.CreateFindCollectionCommand(typeof(Employee), "ORDER BY LastName").Execute(3, 5, out count);

            //test command
            Assert.AreEqual(0, list.Count, "Returned 0 Employees");
            Assert.AreEqual(15, count, "Verify found 15 Employees");

            list = dataStore.CreateFindCollectionCommand(typeof(Employee), "ORDER BY LastName").Execute(3, 4, out count);

            Console.Out.WriteLine("-- Page 4, pagesize 4");
            foreach (Employee e in list) {
                Console.Out.WriteLine(e.LastName);
            }

            //test command
            Assert.AreEqual(3, list.Count, "Returned 3 Employees");
            Assert.AreEqual("Valdez", ((Employee)list[1]).LastName, "Verify Last Name");
            Assert.AreEqual(15, count, "Verify found 15 Employees");
        }

		[Test]
		public void PolymorphicQuery(){
            dataStore = CreateDataStore();
            dataStore.Settings.AutoGenerate = true;
		
			try {
				dataStore.DeleteStorage(typeof(Circle));
			} catch {}
			dataStore.CreateStorage(typeof(Circle));

			try {
				dataStore.DeleteStorage(typeof(Rectangle));
			} catch {}
			dataStore.CreateStorage(typeof(Rectangle));

			Circle c = new Circle();
			c.Color = "Green";
			c.Radius = 5;
			dataStore.Insert(c);

			Rectangle r = new Rectangle();
			r.Color = "Green";
			r.Width = 5;
			r.Length = 2;
			dataStore.Insert(r);

			IList shapes = dataStore.CreateFindCollectionCommand(typeof(Shape), null, true).Execute();
	
			Assert.IsTrue(shapes.Count == 2,"Returned Items = 2" + shapes.Count);
			Assert.IsTrue(((Shape)shapes[0]).GetArea() == 2 * Math.PI * Math.Pow(5,2),"Circle Area");
			Assert.IsTrue(((Shape)shapes[1]).GetArea() == 10, "Rectangle Area");

			dataStore.DeleteStorage(typeof(Circle));
			dataStore.DeleteStorage(typeof(Rectangle));
			dataStore.DeleteStorage(typeof(Shape));

		}

		[Test]
		public void PolymorphicQueryWithFilter(){
            dataStore = CreateDataStore();
            dataStore.Settings.AutoGenerate = true;
		
			try {
				dataStore.DeleteStorage(typeof(Circle));
			} catch {}
			dataStore.CreateStorage(typeof(Circle));

			try {
				dataStore.DeleteStorage(typeof(Rectangle));
			} catch {}
			dataStore.CreateStorage(typeof(Rectangle));

			Circle c = new Circle();
			c.Color = "Green";
			c.Radius = 5;
			dataStore.Insert(c);

			Rectangle r = new Rectangle();
			r.Color = "Green";
			r.Width = 5;
			r.Length = 2;
			dataStore.Insert(r);

			IList shapes = dataStore.CreateFindCollectionCommand(typeof(Shape), null, true).Execute();
	
			Assert.IsTrue(shapes.Count == 2,"Returned Items = " + shapes.Count);
			Assert.IsTrue(((Shape)shapes[0]).GetArea() == 2 * Math.PI * Math.Pow(5,2),"Circle Area");
			Assert.IsTrue(((Shape)shapes[1]).GetArea() == 10,"Rectangle Area");

			dataStore.DeleteStorage(typeof(Circle));
			dataStore.DeleteStorage(typeof(Rectangle));
			dataStore.DeleteStorage(typeof(Shape));

		}

	}
}
