/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Inform.Sql {

	/// <summary>
	/// Summary description for SqlFindObjectCommand.
	/// </summary>
	public class SqlFindObjectCommand : IFindObjectCommand {

		protected internal SqlObjectAccessCommand m_command;
		private SqlDataStore dataStore;

		protected SqlObjectAccessCommand Command {
			get { return m_command; }
		}

		/// <summary>
		/// Creates a new SqlFindObject Command.
		/// </summary>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public SqlFindObjectCommand(SqlDataStore dataStore, Type dynamicType, string filter) : this(dataStore, dynamicType, filter, false) {
		}

		/// <summary>
		/// Creates a new SqlFindObject Command.
		/// </summary>
		public SqlFindObjectCommand(SqlDataStore dataStore, Type dynamicType, string filter, bool polymorphic) : this(dataStore, dynamicType, filter, polymorphic, CommandType.Text) {
		}

		/// <summary>
		/// Creates a new SqlFindObject Command.
		/// </summary>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public SqlFindObjectCommand(SqlDataStore dataStore, Type dynamicType, string cmdText, bool polymorphic, System.Data.CommandType commandType) {
			this.dataStore = dataStore;
			this.m_command = new SqlObjectAccessCommand(dataStore, dynamicType, cmdText, polymorphic, commandType);
		}

		/// <summary>
		/// Gets the SqlParameterCollection.
		/// </summary>
		public IDataParameterCollection Parameters {
			get {
				return this.m_command.Parameters;
			}
		}

		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute the command and generating an error.
		/// </summary>
		public int CommandTimeout {
			get { return this.m_command.CommandTimeout; }
			set { this.m_command.CommandTimeout = value; }
		}

		/// <summary>
		/// Creates a new input parameter for the stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the input parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		/// <example>CreateInputParameter(command, "@UserID",SqlDbType.Int, userID);</example>
		public IDbDataParameter CreateInputParameter(string parameterName, object parameterValue) {
			return m_command.CreateInputParameter(parameterName, parameterValue);
		}

		/// <summary>
		/// Creates a new input parameter for the command.
		/// </summary>
		/// <param name="parameterName">The name of the input parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		/// <example>CreateInputParameter(command, "@UserID",SqlDbType.Int, userID);</example>
		public IDbDataParameter CreateInputParameter(string parameterName, SqlDbType sqlDbType, object parameterValue) {
			return m_command.CreateInputParameter(parameterName, sqlDbType, parameterValue);
		}

		/// <summary>
		/// Creates a new input parameter for the command.
		/// </summary>
		/// <param name="parameterName">The name of the input parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="size">The size of the underlying column.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		/// <example>CreateInputParameter(command, "@UserName",SqlDbType.NVarChar,20,userName);</example>
		public IDbDataParameter CreateInputParameter(string parameterName, SqlDbType sqlDbType, int size, object parameterValue) {
			return m_command.CreateInputParameter(parameterName, sqlDbType, size, parameterValue);
		}

		/// <summary>
		/// Creates a new output parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName) {
			return m_command.CreateOutputParameter(parameterName);
		}

		/// <summary>
		/// Creates a new output parameter for the command.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType) {
			return m_command.CreateOutputParameter(parameterName,sqlDbType);
		}

		/// <summary>
		/// Creates a new output parameter the command.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="size">The size of the underlying column.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType, int size) {
			return m_command.CreateOutputParameter(parameterName,sqlDbType,size);
		}

		/// <summary>
		/// Executes the FindObjectCommand.
		/// </summary>
		/// <returns></returns>
		virtual public object Execute() {

			IObjectReader reader = m_command.ExecuteObjectReader();

			try {
				//is there a record?
				if(reader.Read()) {
					return reader.GetObject();
				} else if(!dataStore.Settings.FindObjectReturnsNull) {
					//no record was returned, so throw error
					throw new ObjectNotFoundException();
				} else {
					return null;
				}
			} 
			finally {
				if (reader != null) {
					reader.Close();
				}
			}
		}

		#region Implementation of ICloneable
		public object Clone() {
			SqlFindObjectCommand cmd = (SqlFindObjectCommand)this.MemberwiseClone();
			this.m_command = (SqlObjectAccessCommand)m_command.Clone();
			return cmd;
		}
		#endregion

	}
}
