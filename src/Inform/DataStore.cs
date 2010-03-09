/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Xml;

using Inform.Common;

namespace Inform {
	
	/// <summary>
	/// Provides a set of commands to access a relational database as both objects and relational data.
	/// </summary>
	public abstract class DataStore : ICloneable {


		//Properties
		private string name;
		private DataStoreSettings settings;
		private bool inTransaction;
		private IDbTransaction transaction;
		private ConnectionManager connectionManager;
		private DataStorageManager storageManager;
		private Hashtable commands;

		
		#region Properties

		/// <summary>
		/// Gets or sets the <see cref="DataStore"/> name.
		/// </summary>
		public string Name {
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// The <see cref="DataStoreSettings"/> for the <see cref="DataStore"/>.
		/// </summary>
		public DataStoreSettings Settings {
			get { return settings; }
		}

		/// <summary>
		/// Whether a transaction has been started;
		/// </summary>
		public bool InTransaction {
			get { return inTransaction; }
			set { inTransaction = value; }
		}

		/// <summary>
		/// Gets the current IDbTransaction if the <see cref="DataStore"/> is currently in a transaction, otherwise returns <see langword="null"/>.
		/// </summary>
		protected internal IDbTransaction CurrentTransaction {
			get { return transaction; }
		}

		/// <exclude />
		/// <summary>
		/// Gets the <see cref="ConnectionManager"/> for database connections.
		/// </summary>
		public ConnectionManager Connection {
			get {
				if (connectionManager == null){
					connectionManager = CreateConnectionManager();
				}
				return connectionManager; 
			}
		}

		/// <summary>
		/// The DataStorageManager.
		/// </summary>
		internal DataStorageManager DataStorageManager {
			get {
				if (storageManager == null){
					storageManager = CreateStorageManager();
				}
				return storageManager;
			}
		}

		/// <summary>
		/// The Predefined Commands.
		/// </summary>
		//TODO: Need typed collection?
		internal IDictionary Commands {
			get { return commands; }
		}

		#endregion

		#region Abstract Methods

		protected internal abstract ConnectionManager CreateConnectionManager();
		protected internal abstract DataStorageManager CreateStorageManager();

		/// <summary>
		/// Creates an <see cref="IDataAccessCommand"/> with the text command to execute.
		/// </summary>
		/// <param name="cmdText">The text command to execute.</param>
		/// <returns>An <see cref="IDataAccessCommand"/> object.</returns>
		public abstract IDataAccessCommand CreateDataAccessCommand(string cmdText);

		/// <summary>
		/// Creates an <see cref="IDataAccessCommand"/> with the text command to execute and specifies how the text command is interpreted.
		/// </summary>
		/// <param name="cmdText">The text command to execute.</param>
		/// <param name="commandType">Specifies how a command string is interpreted.</param>
		/// <returns>An <see cref="IDataAccessCommand"/> object.</returns>
		public abstract IDataAccessCommand CreateDataAccessCommand(string cmdText, System.Data.CommandType commandType);
		
		/// <summary>
		/// Creates an <see cref="IObjectAccessCommand"/> with the type to return and filter to apply.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <returns>An <see cref="IObjectAccessCommand"/> object.</returns>
		public abstract IObjectAccessCommand CreateObjectAccessCommand(Type type, string filter);
		
		
		/// <summary>
		/// Creates an <see cref="IObjectAccessCommand"/> with the type to return, a filter to apply, and whether to return subclasses.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <param name="polymorphic">Whether to return subclasses.</param>
		/// <returns>An <see cref="IObjectAccessCommand"/> object.</returns>
		public abstract IObjectAccessCommand CreateObjectAccessCommand(Type type, string filter, bool polymorphic);
		
		
		/// <summary>
		/// Creates an <see cref="IObjectAccessCommand"/> with the type to return, the command text, whether to return subclasses, and specifies how the text command is interpreted.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="cmdText">The text command to execute.</param>
		/// <param name="polymorphic">Whether to return subclasses.</param>
		/// <param name="commandType">Specifies how a command string is interpreted.</param>
		/// <returns>An <see cref="IObjectAccessCommand"/> object.</returns>
		public abstract IObjectAccessCommand CreateObjectAccessCommand(Type type, string cmdText, bool polymorphic, System.Data.CommandType commandType);
		
		/// <summary>
		/// Creates an <see cref="IFindObjectCommand"/> with the type to return and filter to apply.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <returns>An <see cref="IFindObjectCommand"/> object.</returns>
		public abstract IFindObjectCommand CreateFindObjectCommand(Type type, string filter);
		
		/// <summary>
		/// Creates an <see cref="IFindObjectCommand"/> with the type to return, the command text, and specifies how the text command is interpreted.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="cmdText">The text command to execute.</param>
		/// <param name="commandType">Specifies how a command string is interpreted.</param>
		/// <returns>An <see cref="IFindObjectCommand"/> object.</returns>
		public abstract IFindObjectCommand CreateFindObjectCommand(Type type, string cmdText, System.Data.CommandType commandType);
		
		/// <summary>
		/// Creates an <see cref="IFindObjectCommand"/> with the type to return, the filter to apply, and whether to return subclasses.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <param name="polymorphic">Whether to return subclasses.</param>
		/// <returns>An <see cref="IFindObjectCommand"/> object.</returns>
		public abstract IFindObjectCommand CreateFindObjectCommand(Type type, string filter, bool polymorphic);
		
		/// <summary>
		/// Creates an <see cref="IFindCollectionCommand"/> with the type to return and filter to apply.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <returns>An <see cref="IFindCollectionCommand"/> object.</returns>
		public abstract IFindCollectionCommand CreateFindCollectionCommand(Type type, string filter );

		/// <summary>
		/// Creates an <see cref="IFindCollectionCommand"/> with the type to return, the comand text, and specifies how the text command is interpreted.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="cmdText">The text command to execute.</param>
		/// <param name="commandType">Specifies how a command string is interpreted.</param>
		/// <returns>An <see cref="IFindCollectionCommand"/> object.</returns>
		public abstract IFindCollectionCommand CreateFindCollectionCommand(Type type, string cmdText, System.Data.CommandType commandType);
		
		/// <summary>
		/// Creates an <see cref="IFindCollectionCommand"/> with the type to return, the filter to apply, and whether to return subclasses.
		/// </summary>
		/// <param name="type">The type to return.</param>
		/// <param name="filter">The filter to apply.</param>
		/// <param name="polymorphic">Whether to return subclasses.</param>
		/// <returns>An <see cref="IFindCollectionCommand"/> object.</returns>
		public abstract IFindCollectionCommand CreateFindCollectionCommand(Type type, string filter, bool polymorphic);
		
		/// <summary>
		/// Inserts an object into the data source.
		/// </summary>
		/// <param name="o">The object to insert.</param>
		/// <param name="typeMapping">The TypeMapping that defines the mapping to the data source.</param>
		abstract internal void Insert(object o, TypeMapping typeMapping);

		/// <summary>
		/// Updates an object in the data source.
		/// </summary>
		/// <param name="o">The object to update.</param>
		/// <param name="typeMapping">The TypeMapping that defines the mapping to the data source.</param>
		abstract internal void Update(object o, TypeMapping typeMapping);

		/// <summary>
		/// Deletes an object from the data source by primary key..
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the object to be deleted.</param>
		/// <param name="primaryKey">The value of the objects primary key.</param>
		/// <remarks>Using a <see cref="Type"/> that does not have a primary key defined in its <see cref="TypeMapping"/> will cause an exception.</remarks>
		abstract public void Delete(Type type, object primaryKey);
		
		/// <summary>
		/// Finds an object from the data source by primary key.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> of the object to return.</param>
		/// <param name="primaryKey">The value of the objects primary key.</param>
		/// <returns>The object or <see langword="null"/> if not found. </returns>
		/// <remarks> If the DataStore's <see cref="Settings"/> FindObjectReturnsNull is <see langword="true"/> and the object is noe found, an <see cref="ObjectNotFoundException"/> will be thrown.
		/// Using a <see cref="Type"/> that does not have a primary key defined in its <see cref="TypeMapping"/> will cause an exception.
		/// </remarks>
		public object FindByPrimaryKey(Type type, object primaryKey){
			TypeMapping typeMapping = null;
			typeMapping = DataStorageManager.GetTypeMapping(type, true);

			if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + type.FullName);
			}

			bool polymorphic = type.BaseType != null;

			IFindObjectCommand findCommand =  this.CreateFindObjectCommand(type, this.DataStorageManager.GenerateFindByPrimaryKeySql(typeMapping), polymorphic);
			IMemberMapping mapping = typeMapping.PrimaryKey;
			findCommand.CreateInputParameter("@" + mapping.ColumnName, primaryKey);
		
			return findCommand.Execute();
		}


		#endregion

		#region Methods

		/// <summary>
		/// A DataStore normally is created through factory methods provided by <see cref="DataStoreSevices"/>.
		/// </summary>
		protected DataStore() {
			settings = new DataStoreSettings();
			commands = new Hashtable();
		}


		/// <summary>
		/// Creates storage in the datasource for the registered typemappings if it does not currently exist.
		/// </summary>
		protected internal void EnsureStorage(){
			foreach(TypeMapping typeMapping in this.DataStorageManager.TypeMappings){
				if(!this.ExistsStorage(typeMapping.MappedType)){
					this.CreateStorage(typeMapping);
				}
			}
		}


		/// <summary>
		/// Creates the necessary tables and, if defined, stored procedures in the datasource for the type.
		/// </summary>
		public void CreateStorage(Type type){
			CreateStorage(type, Settings.AutoGenerate);
		}

		/// <summary>
		/// Creates the necessary tables and,  if defined in the TypeMapping, stored procedures in the datasource for the type.
		/// </summary>
		/// <param name="autoGenerate">
		/// Whether to auto generate type mappings by using attributes.
		/// </param>
		public void CreateStorage(Type type, bool autoGenerate){
			TypeMapping typeMapping = DataStorageManager.GetTypeMapping(type, true);
			this.DataStorageManager.CreateStorage(typeMapping);
		}

		/// <summary>
		/// Creates the necessary tables and,  if defined in the TypeMapping, stored procedures in the datasource for the type using a specific TypeMapping.
		/// </summary>
		public void CreateStorage(TypeMapping typeMapping){
			//TODO: Review - add if not already added
			if(DataStorageManager.GetTypeMapping(typeMapping.MappedType) == null){
				DataStorageManager.AddTypeMapping(typeMapping);
			}

			this.DataStorageManager.CreateStorage(typeMapping);
		}

		/// <summary>
		/// Checks if the necessary tables and, if defined in the TypeMapping, stored procedures exists in the datasource for a type.
		/// </summary>
		public bool ExistsStorage(Type type){
			if(type == null){
				throw new ArgumentNullException("type");
			}

			TypeMapping typeMapping = DataStorageManager.GetTypeMapping(type, true);
			
			return this.DataStorageManager.ExistsStorage(typeMapping);
		}
	
		/// <summary>
		/// Deletes the tables and, if defined in the TypeMapping, stored procedures from the data source for this type.
		/// </summary>
		public void DeleteStorage(Type type){
			if(type == null){
				throw new ArgumentNullException("type");
			}

			TypeMapping typeMapping = DataStorageManager.GetTypeMapping(type, true);
			this.DataStorageManager.DeleteStorage(typeMapping);
		}


		/// <summary>
		/// Inserts an object into the data source.
		/// </summary>
		/// <param name="o">The object to insert.</param>
		public void Insert(object o){

			TypeMapping typeMapping = null;
			if((typeMapping = DataStorageManager.GetTypeMapping(o.GetType())) == null){
				throw new DataStoreException("DataStore does not contain storage for " + o.GetType().FullName);
			}

			Insert(o,typeMapping);

			SetContext(o);

		}

		/// <summary>
		/// Updates an object int the data source.
		/// </summary>
		/// <param name="o">The object to update.</param>
		/// <remarks>Using a <see cref="Type"/> that does not have a primary key defined in its <see cref="TypeMapping"/> will cause an exception.</remarks>
		public void Update(object o){

			TypeMapping typeMapping = null;
			if((typeMapping = DataStorageManager.GetTypeMapping(o.GetType())) == null){
				throw new DataStoreException("DataStore does not contain storage for " + o.GetType().FullName);
			} else if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + o.GetType().FullName);
			}

			Update(o,typeMapping);
		}

		/// <summary>
		/// Inserts an object into the data source if the primary key is not set, otherwise updates the object.
		/// </summary>
		/// <param name="o">The object to insert or update.</param>
		/// <remarks>Using a <see cref="Type"/> that does not have a primary key defined in its <see cref="TypeMapping"/> will cause an exception.</remarks>
		public void InsertOrUpdate(object o){

			TypeMapping typeMapping = null;
			if((typeMapping = DataStorageManager.GetTypeMapping(o.GetType())) == null){
				throw new DataStoreException("DataStore does not contain storage for " + o.GetType().FullName);
			} else if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + o.GetType().FullName);
			}
			
			IMemberMapping memberMapping = typeMapping.PrimaryKey;
			if(!memberMapping.HasIdentity(o)){
				Insert(o, typeMapping);
			} else {
				Update(o, typeMapping);
			}
		}


		/// <summary>
		/// Deletes an object from the data source.
		/// </summary>
		/// <param name="o">The object to insert or update.</param>
		/// <remarks>Using a <see cref="Type"/> that does not have a primary key defined in its <see cref="TypeMapping"/> will cause an exception.</remarks>
		public void Delete(object o) {

			TypeMapping typeMapping = DataStorageManager.GetTypeMapping(o.GetType(), true);
			if(typeMapping.PrimaryKey == null) {										  
				throw new ApplicationException("TypeMapping does not have defined PrimaryKey for " + o.GetType().FullName);
			}

			IMemberMapping memberMapping = typeMapping.PrimaryKey;
			Delete(o.GetType(), memberMapping.GetValue(o));

		}


		internal string GetFullName(Type type){
			return type.FullName + "," + type.Assembly.GetName().Name;
		}


		/// <summary>
		/// Sets the context on each <see cref="CacheMapping"/> defined for an object.
		/// </summary>
		/// <param name="o">The object to set the context.</param>
		/// <remarks>
		/// Setting the context is only required when using cach mappings and the object was not inserted or retrieved from a DataStore command.
		/// </remarks>
		public void SetContext(object o){

			TypeMapping typeMapping = DataStorageManager.GetTypeMapping(o.GetType(), true);
			SetContext(o,typeMapping);
			
		}

		private void SetContext(object o, TypeMapping typeMapping){

			foreach( CacheMapping mapping in typeMapping.CacheMappings){
				mapping.SetContext(o);
			}

			if(typeMapping.BaseType != null){
				TypeMapping baseTypeMapping = DataStorageManager.GetTypeMapping(typeMapping.BaseType, true);
				SetContext(o,baseTypeMapping);
			}
		}
		

		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		public void BeginTransaction(){
			if(inTransaction){
				throw new DataStoreException("Transaction has already been started");
			}
			IDbConnection connection = this.Connection.CreateConnection();
			connection.Open();
			transaction = connection.BeginTransaction();
			inTransaction = true;
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		public void CommitTransaction(){
			if(!inTransaction){
				throw new DataStoreException("No transaction has been started");
			}
			transaction.Commit();
			inTransaction = false;
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		public void RollbackTransaction(){
			if(!inTransaction){
				throw new DataStoreException("No transaction has been started");
			}
			transaction.Rollback();
			inTransaction = false;
		}


		/// <summary>
		/// Returns a pre-defined IDataAccessCommand.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>An <see cref="IDataAccessCommand"/> object.</returns>
		public IDataAccessCommand GetDataAccessCommand(string name){
			object command = Commands[name];
			if(command != null){
				return (IDataAccessCommand)((IDataAccessCommand)command).Clone();
			} else {
				return null;
			}
		}

		/// <summary>
		/// Returns a pre-defined IObjectAccessCommand.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>An <see cref="IObjectAccessCommand"/> object.</returns>
		public IObjectAccessCommand GetObjectAccessCommand(string name){
			object command = Commands[name];
			if(command != null){
				return (IObjectAccessCommand)((IObjectAccessCommand)command).Clone();
			} else {
				return null;
			}
		}


		/// <summary>
		/// Returns a pre-defined IFindObjectCommand.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>An <see cref="IFindObjectCommand"/> object.</returns>
		public IFindObjectCommand GetFindObjectCommand(string name){
			object command = Commands[name];
			if(command != null){
				return (IFindObjectCommand)((IFindObjectCommand)command).Clone();
			} else {
				return null;
			}
		}

		/// <summary>
		/// Returns a pre-defined IFindCollectionCommand.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>An <see cref="IFindCollectionCommand"/> object.</returns>
		public IFindCollectionCommand GetFindCollectionCommand(string name){
			object command = Commands[name];
			if(command != null){
				return (IFindCollectionCommand)((IFindCollectionCommand)command).Clone();
			} else {
				return null;
			}
		}

		#endregion

		#region Implementation of ICloneable
		/// <summary>
		/// As each instance of DataStore is not thread, this creates a copy of this DataStore object that can be safely used.
		/// </summary>
		/// <returns></returns>
		public object Clone() {
			return this.MemberwiseClone();
		}
		#endregion


       
    }
}