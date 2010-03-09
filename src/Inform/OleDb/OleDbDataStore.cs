/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

using Inform;
using Inform.Common;

namespace Inform.OleDb {

	/// <exclude/>
	/// <summary>
	/// Summary description for OleDbDataStore.
	/// </summary>
	public class OleDbDataStore : DataStore {

		protected internal override ConnectionManager CreateConnectionManager(){
			return new OleDbConnectionManager();
		}

		protected internal override DataStorageManager CreateStorageManager(){
			return new OleDbDataStorageManager(this);
		}

		public new OleDbConnectionManager Connection {
			get { return (OleDbConnectionManager)base.Connection; } 
		}

		public override IDataAccessCommand CreateDataAccessCommand(string cmdText){
			return new OleDbDataAccessCommand(this, cmdText);
		}

		public override IDataAccessCommand CreateDataAccessCommand(string cmdText, System.Data.CommandType commandType){
			return new OleDbDataAccessCommand(this, cmdText, commandType);
		}

		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string filter){
			return new OleDbObjectAccessCommand(this, dynamicType, filter);
		}


		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string filter, bool polymorphic){
			return new OleDbObjectAccessCommand(this, dynamicType, filter, polymorphic);
		}

		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string cmdText, bool polymorphic, System.Data.CommandType commandType){
			return new OleDbObjectAccessCommand(this, dynamicType, cmdText, polymorphic, commandType);
		}
		
		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string filter){
			return new OleDbFindObjectCommand(this, dynamicObjectType, filter );
		}

		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string cmdText, System.Data.CommandType commandType ){
			return new OleDbFindObjectCommand(this, dynamicObjectType, cmdText, false, commandType );
		}

		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string filter, bool polymorphic){
			return new OleDbFindObjectCommand(this, dynamicObjectType, filter, polymorphic);
		}

		public override IFindCollectionCommand CreateFindCollectionCommand(Type dynamicObjectType, string filter, bool polymorphic){
			return new OleDbFindCollectionCommand(this, dynamicObjectType, filter, polymorphic);
		}

		public override IFindCollectionCommand CreateFindCollectionCommand( Type dynamicObjectType, string filter){
			return new OleDbFindCollectionCommand(this, dynamicObjectType, filter);
		}


		public override IFindCollectionCommand CreateFindCollectionCommand(  Type dynamicObjectType, string filter, System.Data.CommandType commandType){
			return new OleDbFindCollectionCommand(this, dynamicObjectType, filter, commandType );	
		}

		internal override void Insert(object o, TypeMapping typeMapping){

			if(typeMapping.BaseType != null){
				TypeMapping baseTypeMapping = null;
				if((baseTypeMapping = DataStorageManager.GetTypeMapping(typeMapping.BaseType)) == null){
					throw new DataStoreException("DataStore does not contain storage for " + typeMapping.BaseType.FullName);
				}
				Insert(o, baseTypeMapping);
			}

			OleDbDataAccessCommand insertCommand;
			if(typeMapping.UseStoredProcedures){
				insertCommand = new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GetInsertProcedureName(typeMapping), CommandType.StoredProcedure);
			} else {
				insertCommand =  new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GenerateInsertSql(typeMapping), CommandType.Text);
			}

			IMemberMapping outputmapping = null;
			foreach(IMemberMapping mapping in typeMapping.MemberMappings){

				OleDbMemberDbType dbType = (OleDbMemberDbType)mapping.MemberDbType;

				if(mapping.PrimaryKey && mapping.Identity){
					//No batch statemenst allowed in access
					//output = insertCommand.CreateOutputParameter("@" + mapping.ColumnName, dbType.DbType);
					outputmapping = mapping;
				} else if(dbType.Length == 0){
					insertCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, mapping.GetValue(o));
				} else {
					insertCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, dbType.Length, mapping.GetValue(o));
				}
			}
		
			if(outputmapping != null){
				insertCommand.ExecuteNonQuery(CommandBehavior.Default);
				insertCommand.CommandType = CommandType.Text;
				insertCommand.CommandText = "SELECT @@IDENTITY;";
				Object output = insertCommand.ExecuteScalar();
				if(output != null){
					outputmapping.SetValue(o,output);
				}
			} else {
				insertCommand.ExecuteNonQuery();
			}
		}

		internal override void Update(object o, TypeMapping typeMapping){
			
			if(typeMapping.BaseType != null){
				TypeMapping baseTypeMapping = null;
				if((baseTypeMapping = DataStorageManager.GetTypeMapping(typeMapping.BaseType)) == null){
					throw new DataStoreException("DataStore does not contain storage for " + typeMapping.BaseType.FullName);
				}
				Update(o,baseTypeMapping);
			}

			OleDbDataAccessCommand updateCommand;
			if(typeMapping.UseStoredProcedures){
				updateCommand = new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GetUpdateProcedureName(typeMapping), CommandType.StoredProcedure);
			} else {
				updateCommand =  new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GenerateUpdateSql(typeMapping), CommandType.Text);
			}
			
			IMemberMapping primaryKeyMapping = null;
			foreach(IMemberMapping mapping in typeMapping.MemberMappings){

				OleDbMemberDbType dbType = (OleDbMemberDbType)mapping.MemberDbType;

				if(!typeMapping.UseStoredProcedures && mapping.PrimaryKey){
					primaryKeyMapping = mapping;
					continue;
				}

				if(dbType.Length == 0){
					updateCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, mapping.GetValue(o));
				} else {
					updateCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, dbType.Length, mapping.GetValue(o));
				}
			}

			if(!typeMapping.UseStoredProcedures && primaryKeyMapping != null){
				OleDbMemberDbType dbType = (OleDbMemberDbType)primaryKeyMapping.MemberDbType;
				if(dbType.Length == 0){
					updateCommand.CreateInputParameter("@" + primaryKeyMapping.ColumnName, dbType.OleDbType, primaryKeyMapping.GetValue(o));
				} else {
					updateCommand.CreateInputParameter("@" + primaryKeyMapping.ColumnName, dbType.OleDbType, dbType.Length, primaryKeyMapping.GetValue(o));
				}
			}
		
			updateCommand.ExecuteNonQuery();
		}

		override public void Delete(Type type, object primaryKey){
			TypeMapping typeMapping = null;
			if((typeMapping = DataStorageManager.GetTypeMapping(type)) == null){
				throw new DataStoreException("DataStore does not contain storage for " + type.FullName);
			} else if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + type.FullName);
			}

			Delete(typeMapping, primaryKey, true);
		}

		public void Delete(TypeMapping typeMapping, object primaryKey, bool cascade){

			foreach(Type subClass in typeMapping.SubClasses){
				Delete(this.DataStorageManager.GetTypeMapping(subClass), primaryKey, false);
			}
			
			OleDbDataAccessCommand deleteCommand; 
			if(typeMapping.UseStoredProcedures){
				deleteCommand = new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GetDeleteProcedureName(typeMapping), CommandType.StoredProcedure);
			} else {
				deleteCommand =  new OleDbDataAccessCommand(this, ((OleDbDataStorageManager)this.DataStorageManager).GenerateDeleteSql(typeMapping), CommandType.Text);
			}
			
			IMemberMapping mapping = typeMapping.PrimaryKey;
			OleDbMemberDbType dbType = (OleDbMemberDbType)mapping.MemberDbType;

			if(dbType.Length == 0){
				deleteCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, primaryKey);
			} else {
				deleteCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.OleDbType, dbType.Length, primaryKey);
			}
		
			deleteCommand.ExecuteNonQuery();

			if(cascade){
				if(typeMapping.BaseType != null){
					TypeMapping baseTypeMapping = null;
					if((baseTypeMapping = DataStorageManager.GetTypeMapping(typeMapping.BaseType)) == null){
						throw new DataStoreException("DataStore does not contain storage for " + typeMapping.BaseType.FullName);
					}
					Delete(baseTypeMapping, primaryKey, true);
				}

				//TODO: delete subclass table entry
			}
		}

	}
}
