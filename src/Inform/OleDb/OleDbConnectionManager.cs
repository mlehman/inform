/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.OleDb;

using Inform;
using Inform.Common;

namespace Inform.OleDb {

	/// <summary>
	/// OleDbConnectionManager.
	/// </summary>
	public class OleDbConnectionManager : ConnectionManager {
		
		/// <summary>
		/// Creates a new OleDbConnection.
		/// </summary>
		override public IDbConnection CreateConnection(){
			return new OleDbConnection(ConnectionString);
		}
	}
}
