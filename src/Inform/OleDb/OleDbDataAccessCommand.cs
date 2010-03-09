/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Inform.OleDb {

	/// <summary>
	/// This is the base class for Data Access Commands.
	/// 
	/// A OleDbDataAccessCommand can be used for queries that do not return result sets.  They may however still 
	/// return data by registering an output parameter with CreateOutputParameter.  The result can be read 
	/// after calling ExecuteNonQuery.
	/// 
	/// Example usage:
	/// <code>
	/// IDataAccessCommand updateCommand = dataStore.createDataAccessCommand("Employees_UpdateEmployeeSalary");
	/// updateCommand.CreateInputParameter("@EmployeeID",SqlDbType.Int, 2);
	/// updateCommand.CreateInputParameter("@Salary",SqlDbType.Int, salary);
	/// updateCommand.ExecuteNonQuery();
	/// </code>
	/// </summary>
	public class OleDbDataAccessCommand : IDataAccessCommand {

		private OleDbCommand command;
		private OleDbDataStore dataStore;
		private Hashtable parameterPositions = new Hashtable();


		/// <summary>
		/// Creates the command.
		/// </summary>
		public OleDbDataAccessCommand(OleDbDataStore dataStore, string cmdText){

			Console.WriteLine(cmdText);Console.WriteLine();

			this.dataStore = dataStore;
			this.command = new OleDbCommand(cmdText);
		}

		/// <summary>
		/// Creates the command as a specific command type.
		/// </summary>
		public OleDbDataAccessCommand(OleDbDataStore dataStore, string cmdText, System.Data.CommandType commandType){
			
			Console.WriteLine(cmdText);Console.WriteLine();


			this.dataStore = dataStore;
			this.command = new OleDbCommand(cmdText);
			this.command.CommandType = commandType;
			
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
		/// Gets the CommandText.
		/// </summary>
		public string CommandText {
			get { return this.command.CommandText; }
			set { this.command.CommandText = value; }
		}

		/// <summary>
		/// Gets the CommandType.
		/// </summary>
		public CommandType CommandType {
			get { return this.command.CommandType; }
			set { this.command.CommandType = value; }
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

			OleDbParameter param = command.Parameters.Add(new OleDbParameter(parameterName, parameterValue));
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
		public IDbDataParameter CreateInputParameter(string parameterName, OleDbType oleDbType, object parameterValue) {

			OleDbParameter param = command.Parameters.Add(parameterName, oleDbType);
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
		public IDbDataParameter CreateInputParameter(string parameterName, OleDbType oleDbType, int size, object parameterValue) {

			OleDbParameter param = command.Parameters.Add(parameterName, oleDbType, size);
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
			
			OleDbParameter param = command.Parameters.Add(new OleDbParameter(parameterName, DBNull.Value));
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
		public IDbDataParameter CreateOutputParameter(string parameterName, OleDbType oleDbType) {
			
			OleDbParameter param = command.Parameters.Add(parameterName, oleDbType);
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
		public IDbDataParameter CreateOutputParameter(string parameterName, OleDbType oleDbType, int size) {
			
			OleDbParameter param = command.Parameters.Add(parameterName, oleDbType, size);
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
					command.Connection = (OleDbConnection)dataStore.CurrentTransaction.Connection;
					command.Transaction = (OleDbTransaction)dataStore.CurrentTransaction;
				} else {
					command.Connection = (OleDbConnection)dataStore.Connection.CreateConnection();
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
					command.Connection = (OleDbConnection)dataStore.CurrentTransaction.Connection;
					command.Transaction = (OleDbTransaction)dataStore.CurrentTransaction;
				} else {
					command.Connection = (OleDbConnection)dataStore.Connection.CreateConnection();
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
			command.Connection = (OleDbConnection)dataStore.Connection.CreateConnection();
			command.Connection.Open();
			return command.ExecuteReader(commandBehavior);
		}

		/// <summary>
		/// Get a DataView as the result of executing a SqlCommand.
		/// </summary>
		/// <returns></returns>
		public DataView ExecuteDataView() {

			command.Connection = (OleDbConnection)dataStore.Connection.CreateConnection();

			//Get data into DataTable
			OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
			
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
			OleDbDataAccessCommand cmd = (OleDbDataAccessCommand)this.MemberwiseClone();
			cmd.command = new OleDbCommand(this.command.CommandText);
			cmd.CommandTimeout = this.CommandTimeout;
			return cmd;
		}
		#endregion

//TODO: Remove?
//		public string ParseParameters(string cmdText){
//
//			Regex r = new Regex("@\\w+");
//			Match m = r.Match(cmdText);
//			int matchCount = 0;
//			while(m.Success){
//				parameterPositions.Add(m.Captures[0].Value, matchCount++); 
//				m = m.NextMatch();
//			}
//
//			foreach(string param in parameterPositions.Keys){
//				cmdText = cmdText.Replace(param, "?");
//			}
//
//			return cmdText;
//		}

	}
}
