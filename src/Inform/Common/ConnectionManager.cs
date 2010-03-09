/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;

namespace Inform.Common {

	/// <summary>
	/// This type supports the Inform infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public abstract class ConnectionManager {

		private string connectionString;


		/// <summary>
		/// The connection string for database connections.
		/// </summary>
		public string ConnectionString {
			get { return this.connectionString; }
			set { this.connectionString = value; }
		}

		/// <summary>
		/// Creates a new IDbConnection.
		/// </summary>
		abstract public IDbConnection CreateConnection();


	}
}
