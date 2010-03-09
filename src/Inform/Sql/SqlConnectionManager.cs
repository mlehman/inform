using System;
using System.Data;
using System.Data.SqlClient;

using Inform;
using Inform.Common;

namespace Inform.Sql {
	/// <summary>
	/// Summary description for SqlConnectionManager.
	/// </summary>
	public class SqlConnectionManager : ConnectionManager {
		
		/// <summary>
		/// Creates a new SqlConnection.
		/// </summary>
		override public IDbConnection CreateConnection(){
			return new SqlConnection(ConnectionString);
		}
	}
}
