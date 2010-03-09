/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.Common;

namespace Inform {

	/// <summary>
	/// An IDataAccessCommand can be used to execute queries directly with the data source.
	/// </summary>
	/// <example>
	/// The following example executes a query with the default DataStore which does not return a result.
	/// <code>
	/// IDataAccessCommand updateCommand = DataStoreServices.Default.CreateDataAccessCommand("UPDATE Employee SET Title = @Title WHERE EmployeeID = @EmployeeID");
	/// updateCommand.CreateInputParameter("@EmployeeID", 2);
	/// updateCommand.CreateInputParameter("@Title", "Programmer");
	/// updateCommand.ExecuteNonQuery();
	/// </code>
	/// </example>
	public interface IDataAccessCommand : IDataParameterCommand, ICloneable {

		/// <summary>
		/// Executes a IDbCommand that returns no records.
		/// </summary>
		void ExecuteNonQuery();

		/// <summary>
		/// Executes a IDbCommand and returns the first column of the first row.
		/// </summary>
		object ExecuteScalar();

		/// <summary>
		/// Gets the IDataReader that is the result of excuting a the IDataAccessCommand. The associated IDataReader is closed
		/// when the IDataReader is closed.
		/// </summary>
		IDataReader ExecuteReader();

		/// <summary>
		/// Gets the IDataReader that is the result of excuting a the IDataAccessCommand.
		/// </summary>
		IDataReader ExecuteReader(CommandBehavior commandBehavior);

		/// <summary>
		/// Get a DataView as the result of excuting a the IDataAccessCommand.
		/// </summary>
		/// <returns></returns>
		DataView ExecuteDataView();
	}
}
