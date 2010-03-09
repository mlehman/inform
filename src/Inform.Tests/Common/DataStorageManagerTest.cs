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
using Inform.Common;
using Inform.Sql;

namespace Inform.Tests.Common {

	/// <summary>
	/// Summary description for SqlDataStorageManagerTest.
	/// </summary>
	[TestFixture]
	public abstract class DataStorageManagerTest {

		protected DataStore dataStore;

		public abstract DataStore CreateDataStore();

		[SetUp]
		public void InitializeConnection() {
			dataStore = CreateDataStore();
		}

		[Test]
		public void GenerateCreatePropertiesSql(){

			DataStorageManager mngr = DataStorageManager.GetDataStorageManager(dataStore);

			TypeMapping t = mngr.CreateTypeMappingFromAttributes(typeof(Employee));
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));

			Console.Out.WriteLine("-- Inform Generated Stored Procedures");
			Console.Out.WriteLine(mngr.GenerateCreateInsertProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateUpdateProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateDeleteProcedureSql(t));

			Console.Out.WriteLine("-- Inform Generated Queries");
			Console.Out.WriteLine(mngr.GenerateInsertSql(t));
			Console.Out.WriteLine(mngr.GenerateUpdateSql(t));
			Console.Out.WriteLine(mngr.GenerateDeleteSql(t));
			

		}

		[Test]
		public void GenerateCreateFieldsSql(){

			DataStorageManager mngr = DataStorageManager.GetDataStorageManager(dataStore);

			TypeMapping t = mngr.CreateTypeMappingFromAttributes(typeof(Posting));
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));

			Console.Out.WriteLine("-- Inform Generated Stored Procedures");
			Console.Out.WriteLine(mngr.GenerateCreateInsertProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateUpdateProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateDeleteProcedureSql(t));

			Console.Out.WriteLine("-- Inform Generated Queries");
			Console.Out.WriteLine(mngr.GenerateInsertSql(t));
			Console.Out.WriteLine(mngr.GenerateUpdateSql(t));
			Console.Out.WriteLine(mngr.GenerateDeleteSql(t));

			t = mngr.CreateTypeMappingFromAttributes(typeof(Comment));
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));
			
		}

		[Test]
		public void GenerateCreateRelationshipsSql(){

			DataStorageManager mngr = DataStorageManager.GetDataStorageManager(dataStore);

			TypeMapping t = mngr.CreateTypeMappingFromAttributes(typeof(Manager));
			mngr.AddTypeMapping(t);

			t = mngr.CreateTypeMappingFromAttributes(typeof(Project));
			mngr.AddTypeMapping(t);
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));

			Console.Out.WriteLine("-- Inform Generated Stored Procedures");
			Console.Out.WriteLine(mngr.GenerateCreateInsertProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateUpdateProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateDeleteProcedureSql(t));

			Console.Out.WriteLine("-- Inform Generated Queries");
			Console.Out.WriteLine(mngr.GenerateInsertSql(t));
			Console.Out.WriteLine(mngr.GenerateUpdateSql(t));
			Console.Out.WriteLine(mngr.GenerateDeleteSql(t));

			t = mngr.CreateTypeMappingFromAttributes(typeof(Task));
			mngr.AddTypeMapping(t);
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));

			Console.Out.WriteLine("-- Inform Generated Stored Procedures");
			Console.Out.WriteLine(mngr.GenerateCreateInsertProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateUpdateProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateDeleteProcedureSql(t));

			Console.Out.WriteLine("-- Inform Generated Queries");
			Console.Out.WriteLine(mngr.GenerateInsertSql(t));
			Console.Out.WriteLine(mngr.GenerateUpdateSql(t));
			Console.Out.WriteLine(mngr.GenerateDeleteSql(t));

			Console.Out.WriteLine("-- Inform Generated Relationships");
			foreach(RelationshipMapping rm in mngr.RelationshipMappings){
				Console.Out.WriteLine(mngr.GenerateCreateRelationshipSql(rm));
			}

			
		}

		[Test]
		public void GenerateQueryInheritedTable(){

			DataStorageManager mngr = DataStorageManager.GetDataStorageManager(dataStore);

			mngr.AddTypeMapping(mngr.CreateTypeMappingFromAttributes(typeof(Shape)));
			mngr.AddTypeMapping(mngr.CreateTypeMappingFromAttributes(typeof(Circle)));
			mngr.AddTypeMapping(mngr.CreateTypeMappingFromAttributes(typeof(Rectangle)));
			
			Console.Out.WriteLine(mngr.CreateQuery(typeof(Shape), "WHERE Shapes.Color = @Color", true));
			
		}

		[Test]
		public void GenerateCreateInheritedTable(){

			DataStorageManager mngr = DataStorageManager.GetDataStorageManager(dataStore);

			TypeMapping t = mngr.CreateTypeMappingFromAttributes(typeof(Circle));
			
			Console.Out.WriteLine("-- Inform Generated Table");
			Console.Out.WriteLine(mngr.GenerateCreateTableSql(t));

			Console.Out.WriteLine("-- Inform Generated Stored Procedures");
			Console.Out.WriteLine(mngr.GenerateCreateInsertProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateUpdateProcedureSql(t));
			Console.Out.WriteLine(mngr.GenerateCreateDeleteProcedureSql(t));

			Console.Out.WriteLine("-- Inform Generated Queries");
			Console.Out.WriteLine(mngr.GenerateInsertSql(t));
			Console.Out.WriteLine(mngr.GenerateUpdateSql(t));
			Console.Out.WriteLine(mngr.GenerateDeleteSql(t));
			
		}


	}
}
