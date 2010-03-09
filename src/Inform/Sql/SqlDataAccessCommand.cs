/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using Inform.Common;

namespace Inform.Sql {

	/// <summary>
	/// This is the base class for Data Access Commands.
	/// 
	/// A SqlDataAccessCommand can be used for queries that do not return result sets.  They may however still 
	/// return data by registering an output parameter with CreateOutputParameter.  The result can be read 
	/// after calling ExecuteNonQuery.
	/// 
	/// Example usage:
	/// <code>
	/// SqlDataAccessCommand updateCommand = dataStore.createDataAccessCommand("Employees_UpdateEmployeeSalary");
	/// updateCommand.CreateInputParameter("@EmployeeID",SqlDbType.Int, 2);
	/// updateCommand.CreateInputParameter("@Salary",SqlDbType.Int, salary);
	/// updateCommand.ExecuteNonQuery();
	/// </code>
	/// </summary>
	public class SqlDataAccessCommand : IDataAccessCommand {

		internal SqlCommand command;
		internal SqlDataStore dataStore;

		/// <summary>
		/// Creates the command.
		/// </summary>
		public SqlDataAccessCommand(SqlDataStore dataStore, string cmdText){
			
			this.dataStore = dataStore;
			this.command = new SqlCommand(cmdText);

		}

		/// <summary>
		/// Creates the command as a specific command type.
		/// </summary>
		public SqlDataAccessCommand(SqlDataStore dataStore, string cmdText, System.Data.CommandType commandType){
			
			this.dataStore = dataStore;
			this.command = new SqlCommand(cmdText);
			this.command.CommandType = commandType;

		}

		
		/// <summary>
		/// Gets or sets the transaction in which the Command object executes.
		/// </summary>
		public IDbTransaction Transaction {
			get { return this.command.Transaction; }
			set { this.command.Transaction = (SqlTransaction)value; }
		}


		/// <summary>
		/// Gets or sets the wait time before terminating the attempt to execute the command and generating an error.
		/// </summary>
		public int CommandTimeout {
			get { return this.command.CommandTimeout; }
			set { this.command.CommandTimeout = value; }
		}

		/// <summary>
		/// Gets the SqlParameterCollection.
		/// </summary>
		public SqlParameterCollection Parameters {
			get { return this.command.Parameters; }
		}

		/// <summary>
		/// Gets the IDataParameterCollection.
		/// </summary>
		IDataParameterCollection IDataParameterCommand.Parameters {
			get { return this.command.Parameters; }
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

			SqlParameter param = command.Parameters.Add(new SqlParameter(parameterName, parameterValue));
			param.Value = parameterValue;

			return param;
		}

		/// <summary>
		/// Creates a new input parameter for the stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the input parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		/// <example>CreateInputParameter(command, "@UserID",SqlDbType.Int, userID);</example>
		public IDbDataParameter CreateInputParameter(string parameterName, SqlDbType sqlDbType, object parameterValue) {

			SqlParameter param = command.Parameters.Add(parameterName,sqlDbType);
			param.Value = parameterValue;

			return param;
		}

		/// <summary>
		/// Creates a new input parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the input parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="size">The size of the underlying column.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		/// <example>CreateInputParameter(command, "@UserName",SqlDbType.NVarChar,20,userName);</example>
		public IDbDataParameter CreateInputParameter(string parameterName, SqlDbType sqlDbType, int size, object parameterValue) {

			SqlParameter param = command.Parameters.Add(parameterName,sqlDbType, size);
			param.Value = parameterValue;

			return param;
		}

		/// <summary>
		/// Creates a new output parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName) {
			
			SqlParameter param = command.Parameters.Add(new SqlParameter(parameterName, DBNull.Value));
			param.Direction = ParameterDirection.Output;

			return param;
		}


		/// <summary>
		/// Creates a new output parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType) {
			
			SqlParameter param = command.Parameters.Add(parameterName, sqlDbType);
			param.Direction = ParameterDirection.Output;

			return param;
		}

		/// <summary>
		/// Creates a new output parameter for a stored procedure.
		/// </summary>
		/// <param name="parameterName">The name of the output parameter.</param>
		/// <param name="sqlDbType">The sql data type.</param>
		/// <param name="size">The size of the underlying column.</param>
		/// <param name="paramValue">The value of the input paramter.</param>
		/// <returns>The new parameter.</returns>
		public IDbDataParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType, int size) {
			
			SqlParameter param = command.Parameters.Add(parameterName, sqlDbType, size);
			param.Direction = ParameterDirection.Output;

			return param;
		}


		/// <summary>
		/// Executes a SqlCommand that returns no records.
		/// </summary>
		public void ExecuteNonQuery() {
			if(dataStore.InTransaction){
				ExecuteNonQuery(CommandBehavior.Default);
			} else {
				ExecuteNonQuery(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Executes a SqlCommand that returns no records.
		/// </summary>
		public void ExecuteNonQuery(CommandBehavior cmdBehavior) {
			
			if(command.Connection == null){
				if(dataStore.InTransaction){
					command.Connection = (SqlConnection)dataStore.CurrentTransaction.Connection;
					command.Transaction = (SqlTransaction)dataStore.CurrentTransaction;
				} else {
					command.Connection = (SqlConnection)dataStore.Connection.CreateConnection();
				}
			}

			if(command.Connection.State != ConnectionState.Open){
				command.Connection.Open();
			}

			command.ExecuteNonQuery();
			if(cmdBehavior == CommandBehavior.CloseConnection){
				command.Connection.Close();
				command.Connection = null;
			}
		}

		/// <summary>
		/// Executes a SqlCommand and returns the first columen of the first row.
		/// </summary>
		public object ExecuteScalar() {
			if(dataStore.InTransaction){
				return ExecuteScalar(CommandBehavior.Default);
			} else {
				return ExecuteScalar(CommandBehavior.CloseConnection);
			}
		}

		/// <summary>
		/// Executes a SqlCommand and returns the first columen of the first row.
		/// </summary>
		public object ExecuteScalar(CommandBehavior cmdBehavior) {
			
			if(command.Connection == null){
				if(dataStore.InTransaction){
					command.Connection = (SqlConnection)dataStore.CurrentTransaction.Connection;
					command.Transaction = (SqlTransaction)dataStore.CurrentTransaction;
				} else {
					command.Connection = (SqlConnection)dataStore.Connection.CreateConnection();
				}
			}
			
			if(command.Connection.State != ConnectionState.Open){
				command.Connection.Open();
			}

			object obj = command.ExecuteScalar();
			if(cmdBehavior == CommandBehavior.CloseConnection){
				command.Connection.Close();
				command.Connection = null;
			}
			return obj;
		}


		/// <summary>
		/// Gets the SqlDataReader that is the result of excuting a SqlCommand. The associated SqlConnection is closed
		/// when the SqlDataReader is close.
		/// </summary>
		public IDataReader ExecuteReader() {
			return ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Gets the SqlDataReader that is the result of excuting a SqlCommand.
		/// </summary>
		public IDataReader ExecuteReader(CommandBehavior commandBehavior) {
			command.Connection = (SqlConnection)dataStore.Connection.CreateConnection();
			
			if(command.Connection.State != ConnectionState.Open){
				command.Connection.Open();
			}

			return command.ExecuteReader(commandBehavior);
		}

		/// <summary>
		/// Get a DataView as the result of executing a SqlCommand.
		/// </summary>
		/// <returns></returns>
		public DataView ExecuteDataView() {

			command.Connection =  (SqlConnection)dataStore.Connection.CreateConnection();

			//Get data into DataTable
			SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
			
			//Create DataSet
			DataSet dataSet = new DataSet();

			//Fill DataSet
			dataAdapter.Fill(dataSet);

			//Create View from first table
			DataTable table = dataSet.Tables[0];
			table.Locale = CultureInfo.InvariantCulture;
			DataView view = table.DefaultView;

			return view;
		}

		#region Implementation of ICloneable
		public object Clone() {
			SqlDataAccessCommand cmd = (SqlDataAccessCommand)this.MemberwiseClone();
			cmd.command = new SqlCommand(this.command.CommandText);
			cmd.CommandTimeout = this.CommandTimeout;
			return cmd;
		}
		#endregion
	}
}
