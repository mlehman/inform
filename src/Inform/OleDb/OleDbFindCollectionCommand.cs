/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

using Inform;

namespace Inform.OleDb {
	
	/// <summary>
	/// Summary description for SqlFindCollectionCommand.
	/// </summary>
	public class OleDbFindCollectionCommand : OleDbFindObjectCommand, IFindCollectionCommand {

		/// <summary>
		/// Creates a new SqlFindCollectionCommand Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public OleDbFindCollectionCommand(OleDbDataStore dataStore, Type dynamicType, String filter ) : base(dataStore, dynamicType, filter){
		}
			
		/// <summary>
		/// Creates a new SqlFindCollectionCommand Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		/// <param name="dynamicObjectType">The type of object to populate dynamically.</param>
		public OleDbFindCollectionCommand(OleDbDataStore dataStore,  Type dynamicType, String cmdText, CommandType commandType) : base(dataStore, dynamicType, cmdText, false, commandType){
		}

		/// <summary>
		/// Creates a new SqlFindObject Command.
		/// </summary>
		/// <param name="storeProcedureName">The name of the stored procedure to call.</param>
		public OleDbFindCollectionCommand(OleDbDataStore dataStore, Type dynamicType, string filter, bool polymorphic) : base(dataStore, dynamicType, filter, polymorphic) {
		}

		/// <summary>
		/// Executes the Data Access Command.
		/// </summary>
		/// <returns></returns>
		public new IList Execute(){

			IObjectReader reader = Command.ExecuteObjectReader();
			ArrayList collection = new ArrayList();

			try {
				while (reader.Read()){
					collection.Add(reader.GetObject());
				}  
			} finally {
				if (reader != null){
					reader.Close();
				}
			}

			return ArrayList.ReadOnly(collection);
		}


        /// <summary>
        /// Executes the Data Access Command.
        /// </summary>
        /// <returns></returns>
        public IList Execute(int startPage, int pageSize, out int totalCount) {
            totalCount = 0;
            return Execute();
        }

		#region Implementation of ICloneable
		public new object Clone() {
			OleDbFindCollectionCommand cmd = (OleDbFindCollectionCommand)this.MemberwiseClone();
			this.m_command = (OleDbObjectAccessCommand)m_command.Clone();
			return cmd;
		}
		#endregion
	}
}
