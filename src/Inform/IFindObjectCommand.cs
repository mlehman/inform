/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform.Common;

namespace Inform {

	/// <summary>
	/// An IFindObjectCommand can be used to execute queries to return an object from with the data source.
	/// </summary>
	/// <example>
	/// The following example executes a IFindObjectCommand with the default DataStore.
	/// <code>
	/// IFindObjectCommand findCommand = DataStoreServices.Default.CreateFindObjectCommand(typeof(Employee),"WHERE Title = 'CEO'");
	/// Employee ceo = (Employee)findCommand.Execute();
	/// </code>
	/// </example>
	public interface IFindObjectCommand : IDataParameterCommand, ICloneable {
		

		/// <summary>
		/// Executes the IFindObjectCommand.
		/// </summary>
		/// <returns>The object.</returns>
		object Execute();
	}
}
