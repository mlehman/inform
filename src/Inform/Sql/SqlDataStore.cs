/*
 * SqlDataStore.cs	12/26/2002
 *
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;

using Inform;
using Inform.Common;

namespace Inform.Sql {

	/// <exclude/>
	/// <summary>
	/// Summary description for SqlDataStore.
	/// </summary>
	public class SqlDataStore : DataStore {

		protected internal override ConnectionManager CreateConnectionManager(){
			return new SqlConnectionManager();
		}

		protected internal override DataStorageManager CreateStorageManager(){
			return new SqlDataStorageManager(this);
		}

		public new SqlConnectionManager Connection {
			get { return (SqlConnectionManager)base.Connection; }
		}

		public override IDataAccessCommand CreateDataAccessCommand(string cmdText){
			return new SqlDataAccessCommand(this, cmdText);
		}

		public override IDataAccessCommand CreateDataAccessCommand(string cmdText, System.Data.CommandType commandType){
			return new SqlDataAccessCommand(this, cmdText, commandType);
		}

		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string filter){
			return new SqlObjectAccessCommand(this, dynamicType, filter);
		}


		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string filter, bool polymorphic){
			return new SqlObjectAccessCommand(this, dynamicType, filter, polymorphic);
		}

		public override IObjectAccessCommand CreateObjectAccessCommand(Type dynamicType, string cmdText, bool polymorphic, System.Data.CommandType commandType){
			return new SqlObjectAccessCommand(this, dynamicType, cmdText, polymorphic, commandType);
		}
		
		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string filter){
			return new SqlFindObjectCommand(this, dynamicObjectType, filter );
		}

		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string cmdText, System.Data.CommandType commandType ){
			return new SqlFindObjectCommand(this, dynamicObjectType, cmdText, false, commandType );
		}

		public override IFindObjectCommand CreateFindObjectCommand(Type dynamicObjectType, string filter, bool polymorphic){
			return new SqlFindObjectCommand(this, dynamicObjectType, filter, polymorphic);
		}

		public override IFindCollectionCommand CreateFindCollectionCommand(Type dynamicObjectType, string filter, bool polymorphic){
			return new SqlFindCollectionCommand(this, dynamicObjectType, filter, polymorphic);
		}

		public override IFindCollectionCommand CreateFindCollectionCommand( Type dynamicObjectType, string filter){
			return new SqlFindCollectionCommand(this, dynamicObjectType, filter);
		}


		public override IFindCollectionCommand CreateFindCollectionCommand(  Type dynamicObjectType, string filter, System.Data.CommandType commandType){
			return new SqlFindCollectionCommand(this, dynamicObjectType, filter, commandType );	
		}

		internal override void Insert(object o, TypeMapping typeMapping){

			if(typeMapping.BaseType != null){
				TypeMapping baseTypeMapping = null;
				if((baseTypeMapping = DataStorageManager.GetTypeMapping(typeMapping.BaseType)) == null){
					throw new DataStoreException("DataStore does not contain storage for " + typeMapping.BaseType.FullName);
				}
				Insert(o,baseTypeMapping);
			}

			SqlDataAccessCommand insertCommand;
			if(typeMapping.UseStoredProcedures){
				insertCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GetInsertProcedureName(typeMapping), CommandType.StoredProcedure);
			} else {
				insertCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GenerateInsertSql(typeMapping), CommandType.Text);
			}

			IDbDataParameter output = null;
			IMemberMapping outputmapping = null;

			foreach(IMemberMapping mapping in typeMapping.MemberMappings){

				SqlMemberDbType dbType = (SqlMemberDbType)mapping.MemberDbType;

				if(mapping.PrimaryKey && mapping.Identity){
					output = insertCommand.CreateOutputParameter("@" + mapping.ColumnName, dbType.SqlDbType);
					outputmapping = mapping;
				} else if(dbType.Length == 0){ 
					insertCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, mapping.GetValue(o));
				} else {
					insertCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, dbType.Length, mapping.GetValue(o));
				}
			}
		
			insertCommand.ExecuteNonQuery();

			if(output!=null){
				outputmapping.SetValue(o,output.Value);
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

            if ((typeMapping.PrimaryKey != null && typeMapping.MemberMappings.Count > 1) || (typeMapping.PrimaryKey == null && typeMapping.MemberMappings.Count > 0)) {

                SqlDataAccessCommand updateCommand;
                if (typeMapping.UseStoredProcedures) {
                    updateCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GetUpdateProcedureName(typeMapping), CommandType.StoredProcedure);
                } else {
                    updateCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GenerateUpdateSql(typeMapping), CommandType.Text);
                }

                foreach (IMemberMapping mapping in typeMapping.MemberMappings) {

                    SqlMemberDbType dbType = (SqlMemberDbType)mapping.MemberDbType;

                    if (dbType.Length == 0) {
                        updateCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, mapping.GetValue(o));
                    } else {
                        updateCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, dbType.Length, mapping.GetValue(o));
                    }
                }

                updateCommand.ExecuteNonQuery();
            }
		}

		//TODO: Push down
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
			
			SqlDataAccessCommand deleteCommand; 
			if(typeMapping.UseStoredProcedures){
				deleteCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GetDeleteProcedureName(typeMapping), CommandType.StoredProcedure);
			} else {
				deleteCommand = new SqlDataAccessCommand(this, ((SqlDataStorageManager)this.DataStorageManager).GenerateDeleteSql(typeMapping), CommandType.Text);
			}

			IMemberMapping mapping = typeMapping.PrimaryKey;
			SqlMemberDbType dbType = (SqlMemberDbType)mapping.MemberDbType;

			if(dbType.Length == 0){
				deleteCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, primaryKey);
			} else {
				deleteCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, dbType.Length, primaryKey);
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
			}
		}


//		override public object FindByPrimaryKey(Type type, object primaryKey){
//			TypeMapping typeMapping = null;
//			if((typeMapping = DataStorageManager.GetTypeMapping(type)) == null){
//				throw new DataStoreException("DataStore does not contain storage for " + type.FullName);
//			} else if(typeMapping.PrimaryKey == null) {										  
//				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + type.FullName);
//			}
//
//			SqlFindObjectCommand findCommand = new SqlFindObjectCommand(this, type, this.DataStorageManager.GenerateFindByPrimaryKeySql(typeMapping), false, CommandType.Text);
//			IMemberMapping mapping = typeMapping.MemberMappings[typeMapping.PrimaryKey];
//			SqlMemberDbType dbType = (SqlMemberDbType)mapping.MemberDbType;
//		
//			if(dbType.Length == 0){
//				findCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, primaryKey);
//			} else {
//				findCommand.CreateInputParameter("@" + mapping.ColumnName, dbType.SqlDbType, dbType.Length, primaryKey);
//			}
//		
//			return findCommand.Execute();
//		}
	}
}
