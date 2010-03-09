/*
 * SqlQuery.cs	12/29/2002
 *
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;


namespace DataCommand.Sql {

	/// <summary>
	/// Summary description for SqlQuery.
	/// </summary>
	internal class SqlFindObjectQuery {

		private DataStore objectStore;
		private Type t;
		private ArrayList criteriaList;


		public SqlFindObjectQuery( DataStore objectStore, Type t) {
			this.objectStore = objectStore;
			this.t = t;
			this.criteriaList = new ArrayList();
		}


		public void Add(ICriteria criteria){
			this.criteriaList.Add(criteria);
		}

		public Object ExecuteQuery(){

			string sqlCommand = getSqlCommand();

			//create object to find
			Object foundObject = t.GetConstructor(new Type[]{}).Invoke(null);
			
			return foundObject;
		}

		private string getSqlCommand(){
			StringBuilder sqlCommand = new StringBuilder();
			//sqlCommand.Append("SELECT * FROM " + ObjectTable.GetTableName(t));
			if (criteriaList.Count > 0 ) {
				sqlCommand.Append(" WHERE ");

				bool appendComa = false;
				foreach(ICriteria c in criteriaList){
					if(appendComa){
						sqlCommand.Append(", ");
					} else {
						appendComa = true;
					}
					sqlCommand.Append(c.GetSQL());
				}
			}

			return sqlCommand.ToString();
		}

	}
}
