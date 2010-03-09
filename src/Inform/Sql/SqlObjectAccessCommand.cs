/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Inform.Sql {
	/// <summary>
	/// Summary description for SqlObjectAccessCommand.
	/// </summary>
	public class SqlObjectAccessCommand : IObjectAccessCommand {

        internal SqlDataStore dataStore;
		private Type dynamicType;
		private string cmdText;
		private bool polymorphic;
		private CommandType commandType;
		internal SqlDataAccessCommand command;

		public SqlObjectAccessCommand(SqlDataStore dataStore, Type dynamicType, string filter) : this(dataStore, dynamicType, filter, false)  {
		}

		public SqlObjectAccessCommand(SqlDataStore dataStore, Type dynamicType, string filter, bool polymorphic) : this(dataStore, dynamicType, filter, false, CommandType.Text) {
		}

		public SqlObjectAccessCommand(SqlDataStore dataStore, Type dynamicType, string cmdText, bool polymorphic, CommandType commandType) {
			this.dataStore = dataStore;
			this.cmdText = cmdText;
			this.polymorphic = polymorphic;
			this.dynamicType = dynamicType;
			this.commandType = commandType;

			if(commandType == CommandType.Text){
				command = new SqlDataAccessCommand(dataStore, dataStore.DataStorageManager.CreateQuery(dynamicType, cmdText, polymorphic), commandType);
			} else {
				command = new SqlDataAccessCommand(dataStore, cmdText, commandType);
			}
		}

		
		/// <summary>
		/// Gets the SqlParameterCollection.
		/// </summary>
		public IDataParameterCollection Parameters {
			get {
				return this.command.Parameters;
			}
		}

		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute the command and generating an error.
		/// </summary>
		public int CommandTimeout {
			get { return this.command.CommandTimeout; }
			set { this.command.CommandTimeout = value; }
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
			return command.CreateInputParameter(parameterName, parameterValue);
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
			return command.CreateInputParameter(parameterName, sqlDbType, parameterValue);
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
			return command.CreateInputParameter(parameterName, sqlDbType, size, parameterValue);
		}

		/// <summary>
		/// Creates a new output parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName) {
			return command.CreateOutputParameter(parameterName);
		}

		/// <summary>
		/// Creates a new output parameter for the command.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType) {
			return command.CreateOutputParameter(parameterName,sqlDbType);
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
			return command.CreateOutputParameter(parameterName,sqlDbType,size);
		}


		/// <summary>
		/// Gets the SqlObjectReader that is the result of excuting a SqlCommand. The associated SqlConnection is closed
		/// when the SqlObjectReader is close.
		/// </summary>
		public IObjectReader ExecuteObjectReader() {
			return ExecuteObjectReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Gets the SqlObjectReader that is the result of excuting a SqlCommand.
		/// </summary>
		public IObjectReader ExecuteObjectReader(CommandBehavior commandBehavior) {

            Debug.WriteLine("Executing SQL: " + command.command.CommandText);

			SqlObjectReader reader = new SqlObjectReader(this.dataStore, (SqlDataReader)command.ExecuteReader(commandBehavior), dynamicType, polymorphic);
			return reader;
		}

		#region Implementation of ICloneable
		public object Clone() {
			SqlObjectAccessCommand cmd = (SqlObjectAccessCommand)this.MemberwiseClone();
			this.command = (SqlDataAccessCommand)command.Clone();
			return cmd;
		}
		#endregion
	}
}
