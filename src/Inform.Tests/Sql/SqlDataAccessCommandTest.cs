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
using Inform.Sql;

namespace Inform.Tests.Sql {

	/// <summary>
	/// Summary description for SqlDataAccessCommand.
	/// </summary>
	[TestFixture]
	public class SqlDataAccessCommandTest {

		SqlDataStore dataStore;
		

		[SetUp]
		public void InitializeConnection() {
			dataStore = new SqlDataStore();
            dataStore.Connection.ConnectionString = "server=localhost;database=Inform;uid=development;pwd=d3v3lopm3nt";
			dataStore.Settings.AutoGenerate = true;
			dataStore.Settings.UseStoredProcedures = true;
			
			dataStore.CreateStorage(typeof(Employee));

			Employee e = new Employee();
			e.EmployeeID = "2";
			e.FirstName = "Sam";
			e.LastName = "Donaldson";
			e.Title = "Director";
			e.Salary = 70000;
			dataStore.Insert(e);

			Employee e2 = new Employee();
			e2.EmployeeID = "3";
			e2.FirstName = "Jim";
			e2.LastName = "Davis";
			e2.Title = "Director";
			e2.Salary = 65000;
			dataStore.Insert(e2);
		}

		[TearDown]
		public void DeleteStorage() {
			dataStore.DeleteStorage(typeof(Employee));
		}

		[Test]
		public void UpdateEmployee(){

			//Get Employee
			SqlFindObjectCommand findCommand = (SqlFindObjectCommand)dataStore.CreateFindObjectCommand(typeof(Employee), "WHERE EmployeeID = 2");
			Employee original = (Employee)findCommand.Execute();

			//Create command
			IDataAccessCommand updateCommand = dataStore.CreateDataAccessCommand("Employees_Update", CommandType.StoredProcedure);
			updateCommand.CreateInputParameter("@EmployeeID", 2);
			updateCommand.CreateInputParameter("@FirstName", "Jim");
			updateCommand.CreateInputParameter("@LastName", "Denvers");
			updateCommand.CreateInputParameter("@Title","Engineer");
			updateCommand.CreateInputParameter("@Salary", 90000);
			updateCommand.CreateInputParameter("@Contract", false);
			updateCommand.CreateInputParameter("@DateOfBirth", DateTime.Parse("5/10/1964"));
			updateCommand.ExecuteNonQuery();

			//test command
			Employee updated = (Employee)findCommand.Execute();
			Assert.AreEqual("Engineer",updated.Title,"Verify Title");

			//Create command
			updateCommand = dataStore.CreateDataAccessCommand("Employees_Update", CommandType.StoredProcedure);
			updateCommand.CreateInputParameter("@EmployeeID", 2);
			updateCommand.CreateInputParameter("@FirstName", original.FirstName);
			updateCommand.CreateInputParameter("@LastName", original.LastName);
			updateCommand.CreateInputParameter("@Title", original.Title);
			updateCommand.CreateInputParameter("@Salary", original.Salary);
			updateCommand.CreateInputParameter("@Contract", false);
			updateCommand.CreateInputParameter("@DateOfBirth", DateTime.Parse("5/10/1964"));
			updateCommand.ExecuteNonQuery();

			
		}

	}
}
