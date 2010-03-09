/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

using Inform.Common;

namespace Inform {

	/// <summary>
	/// An IFindCollectionCommand can be used to execute queries to return a collection of objects from with the data source.
	/// </summary>
	/// <example>
	/// The following example executes a IFindCollectionCommand with the default DataStore.
	/// <code>
	/// IFindCollectionCommand findCommand = DataStoreServices.Default.CreateFindCollectionCommand(typeof(Employee),"WHERE Title = @Title");
	/// findCommand.CreateInputParameter("@Title", "Programmer");
	/// IList employees = findCommand.Execute();
	/// </code>
	/// </example>
	public interface IFindCollectionCommand : IDataParameterCommand, ICloneable  {

		/// <summary>
		/// Executes the IFindCollectionCommand.
		/// </summary>
		IList Execute();

        IList Execute(int pageIndex, int pageSize, out int totalCount);
    }
}