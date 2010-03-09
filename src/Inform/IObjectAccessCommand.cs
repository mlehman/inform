using System;
using System.Data;

using Inform.Common;

namespace Inform {

	/// <summary>
	/// An IObjectAccessCommand can be used to execute queries to return a stream of objects from with the data source.
	/// </summary>
	/// <example>
	/// The following example executes a IObjectAccessCommand with the default DataStore.
	/// <code>
	/// IObjectAccessCommand objectAccessCommand = DataStoreServices.Default.CreateObjectAccessCommand(typeof(Employee),"WHERE Title = @Title");
	/// objectAccessCommand.CreateInputParameter("@Title", "Programmer");
	/// IObjectReader reader = objectAccessCommand.ExecuteObjectReader();
	/// </code>
	/// </example>
	public interface IObjectAccessCommand : IDataParameterCommand, ICloneable {


		/// <summary>
		/// Gets the IObjectReader that is the result of excuting a IObjectAccessCommand. The associated IDbConnection is closed
		/// when the IObjectReader is closed.
		/// </summary>
		IObjectReader ExecuteObjectReader();

		/// <summary>
		/// Gets the IObjectReader that is the result of excuting a IObjectAccessCommand.
		/// </summary>
		IObjectReader ExecuteObjectReader(CommandBehavior commandBehavior);

	}
}
