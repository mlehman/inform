/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Inform;
using System.Diagnostics;

namespace Inform.Sql {
	
	/// <summary>
	/// Summary description for SqlFindCollectionCommand.
	/// </summary>
	public class SqlFindCollectionCommand : SqlFindObjectCommand, IFindCollectionCommand {

		/// <summary>
		/// Creates a new SqlFindCollectionCommand Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public SqlFindCollectionCommand(SqlDataStore dataStore, Type dynamicType, String filter ) : base(dataStore, dynamicType, filter){
		}
			
		/// <summary>
		/// Creates a new SqlFindCollectionCommand Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public SqlFindCollectionCommand(SqlDataStore dataStore,  Type dynamicType, String cmdText, CommandType commandType) : base(dataStore, dynamicType, cmdText, false, commandType){
		}

		/// <summary>
		/// Creates a new SqlFindObject Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		public SqlFindCollectionCommand(SqlDataStore dataStore, Type dynamicType, string filter, bool polymorphic) : base(dataStore, dynamicType, filter, polymorphic) {
		}

		/// <summary>
		/// Executes the Data Access Command.
		/// </summary>
		/// <returns></returns>
		public new IList Execute(){

			IObjectReader reader = Command.ExecuteObjectReader();
			ArrayList collection = new ArrayList();

			try {
				while (reader.Read()){
					collection.Add(reader.GetObject());
				}  
			} finally {
				if (reader != null){
					reader.Close();
				}
			}

			return ArrayList.ReadOnly(collection);
		}

        /// <summary>
        /// Executes the Data Access Command.
        /// </summary>
        /// <returns></returns>
        public IList Execute(int pageIndex, int pageSize, out int totalCount) {

            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnpag/html/scalenethowto05.asp

            
            Debug.WriteLine("Original SQL: " + Command.command.command.CommandText);

            string sql = Command.command.command.CommandText;

            string orderby = ExtractOrderBy(sql);
            if (orderby == null) {
                throw new ApplicationException("A order by clause is required for paging.");
            }

            string countSql = "SELECT COUNT(*) " + ExtractFrom(sql);

            Trace.Write("Check Count");
            SqlDataAccessCommand cmd = new SqlDataAccessCommand(this.Command.dataStore, countSql);
            totalCount = (int)cmd.ExecuteScalar();

            sql = "SELECT * FROM ( SELECT TOP " + Math.Min(pageSize, Math.Max(0,totalCount - pageSize * pageIndex))  + " * FROM ( SELECT TOP " + pageSize * (pageIndex + 1) + " " + ExtractQuery(sql) + " ) AS Top1 ORDER BY " + ReverseOrderBy(orderby) + " ) AS Top2 ORDER BY " + orderby;

            Command.command.command.CommandText = sql;

            Trace.Write("Execute Search");
            return Execute();
        }

        private object ReverseOrderBy(string orderby) {
            orderby = orderby.ToUpper();

            string[] fieldsOrders = orderby.Split(',');
            for (int i = 0; i < fieldsOrders.Length; i++) {
                string fieldOrder = fieldsOrders[i];

                if(fieldOrder.Contains(" ASC")){
                    fieldOrder = fieldOrder.Replace(" ASC", " DESC");
                } else  if(fieldOrder.Contains(" DESC")){
                    fieldOrder = fieldOrder.Replace(" DESC", " ASC");
                } else {
                    fieldOrder = fieldOrder + " DESC";
                }

                fieldsOrders[i] = fieldOrder;
            }

            return String.Join(",", fieldsOrders);
        }

        private string ExtractOrderBy(string sql) {
            int index = sql.ToUpper().LastIndexOf("ORDER BY");
            if (index > 0) {
                return sql.Substring(index + 8);
            } else return null;
        }

        private string ExtractFrom(string sql) {
            int index = sql.ToUpper().LastIndexOf("FROM");
            if (index > 0) {
                sql =  sql.Substring(index);
                index = sql.ToUpper().LastIndexOf("ORDER BY");
                if (index > 0) {
                    return sql.Substring(0,index);
                } else return null;
            } else return null;
        }

        private string ExtractQuery(string sql) {
           return sql.Substring(7);
        }

		#region Implementation of ICloneable
		public new object Clone() {
			SqlFindCollectionCommand cmd = (SqlFindCollectionCommand)this.MemberwiseClone();
			this.m_command = (SqlObjectAccessCommand)m_command.Clone();
			return cmd;
		}
		#endregion
	}
}
