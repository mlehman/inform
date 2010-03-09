/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text;

using Inform;
using Inform.Common;
using Inform.OleDb.Mappings;

namespace Inform.OleDb {
	/// <summary>
	/// Summary description for OleDbDataStorageManager.
	/// </summary>
	public class OleDbDataStorageManager : DataStorageManager {

		public OleDbDataStorageManager(OleDbDataStore dataStore) : base(dataStore) {
			AddDefaultPropertyMappingsTypes();
		}

		override public bool ExistsStorage(TypeMapping typeMapping){
			return CheckMapping(typeMapping).Condition != TypeMappingState.TypeMappingStateCondition.TableMissing;
		}


		override public string GenerateCreateTableSql(TypeMapping typeMapping){
			
			StringBuilder sqlCreateTable = new StringBuilder();

			//Create statement
			sqlCreateTable.Append("CREATE TABLE [" + typeMapping.TableName + "] (\r\n");
			
			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				//add comas after column definitions
				if(addComa){
					sqlCreateTable.Append(",\r\n");
				}

				sqlCreateTable.Append("\t[" + propertyMapping.ColumnName + "] "+ propertyMapping.MemberDbType.ToSql());
				
				if (propertyMapping.PrimaryKey){
					if(propertyMapping.Identity){ //TODO: Need to support all identity cases
						sqlCreateTable.Append(" IDENTITY (1,1) PRIMARY KEY NOT NULL");
					} else {
						sqlCreateTable.Append(" PRIMARY KEY");
					}
				} else {
					if(propertyMapping.MemberDbType.IsNullable) {
						sqlCreateTable.Append(" NULL");
					} else {
						sqlCreateTable.Append(" NOT NULL"); 
					}
				}
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateTable.Append("\r\n");

			//close create statement
			sqlCreateTable.Append(")\r\n");

			return sqlCreateTable.ToString();
		}

		override public string GenerateDeleteTableSql(TypeMapping typeMapping){
			return "DROP TABLE [" + typeMapping.TableName + "]";
		}

		override public string GenerateCreateRelationshipSql(RelationshipMapping rm){
			StringBuilder sqlCreateRelationship = new StringBuilder();
			TypeMapping childTypeMapping = this.GetTypeMapping(rm.ChildType, true);
			TypeMapping parentTypeMapping = this.GetTypeMapping(rm.ParentType, true);
			sqlCreateRelationship.AppendFormat("ALTER TABLE [{0}] \r\n",childTypeMapping.TableName);
			sqlCreateRelationship.AppendFormat("ADD CONSTRAINT {0} \r\n", rm.Name);
			sqlCreateRelationship.AppendFormat("FOREIGN KEY ([{0}]) REFERENCES [{1}]([{2}])", 
				childTypeMapping.MemberMappings.GetByName(rm.ChildMember).ColumnName, 
				parentTypeMapping.TableName, 
				parentTypeMapping.MemberMappings.GetByName(rm.ParentMember).ColumnName); 
			
			return sqlCreateRelationship.ToString();
		}

		override public string GenerateDropProcedureSql(string procedureName){
			string sql; // = "IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[" + procedureName + "]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)\r\n";
			return sql = "DROP PROCEDURE [" + procedureName + "]";
		}

		override public string GenerateInsertSql(TypeMapping typeMapping){
			StringBuilder sqlCreateInsert = new StringBuilder();

			//Add the columns
			bool addComa = false;

			sqlCreateInsert.AppendFormat("INSERT INTO {0} (\r\n",typeMapping.TableName);

			//Add the column names
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t[{0}]", propertyMapping.ColumnName);
				
				addComa = true;
			}

			sqlCreateInsert.Append("\r\n) VALUES (\r\n");

			//Add the values
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t@{0}", propertyMapping.ColumnName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateInsert.Append("\r\n");
			sqlCreateInsert.Append(")\r\n");


			return sqlCreateInsert.ToString();
		}

		override public string GenerateUpdateSql(TypeMapping typeMapping){
			
			StringBuilder sqlCreateSQL = new StringBuilder();

			//Add the columns
			bool addComa = false;
			sqlCreateSQL.AppendFormat("UPDATE {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.Append("SET \r\n");
			//Add the columns
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateSQL.Append(",\r\n");
				}

				sqlCreateSQL.AppendFormat("\t{1}.{0} = @{0}", propertyMapping.ColumnName,typeMapping.TableName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateSQL.Append("\r\n");
			sqlCreateSQL.AppendFormat("WHERE {1}.{0} = @{0}\r\n", typeMapping.PrimaryKey.ColumnName,typeMapping.TableName);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateDeleteSql(TypeMapping typeMapping){
			if(typeMapping.PrimaryKey == null){
				throw new DataStoreException("Can not create delete procedure without defined primary key");
			}

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("DELETE FROM {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.AppendFormat("WHERE {0} = @{0}\r\n", typeMapping.PrimaryKey.ColumnName);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateCreateInsertProcedureSql(TypeMapping typeMapping){

			StringBuilder sqlCreateInsert = new StringBuilder();

			sqlCreateInsert.AppendFormat("CREATE PROCEDURE {0}\r\n", GetInsertProcedureName(typeMapping));
			sqlCreateInsert.Append("(\r\n");

			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){

				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}
				
				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t@{0} {1}", propertyMapping.ColumnName, propertyMapping.MemberDbType.ToSql());

				addComa = true;
			}
			//return from last column defintion
			sqlCreateInsert.Append("\r\n");

			sqlCreateInsert.Append(")\r\n");
			sqlCreateInsert.Append("AS\r\n");

			sqlCreateInsert.AppendFormat("INSERT INTO {0} (\r\n",typeMapping.TableName);

			//Add the column names
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t[{0}]", propertyMapping.ColumnName);
				
				addComa = true;
			}

			sqlCreateInsert.Append("\r\n) VALUES (\r\n");

			//Add the values
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t@{0}", propertyMapping.ColumnName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateInsert.Append("\r\n");
			sqlCreateInsert.Append(")\r\n");


			return sqlCreateInsert.ToString();
		}

		override public string GenerateCreateUpdateProcedureSql(TypeMapping typeMapping){

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("CREATE PROCEDURE {0}\r\n", GetUpdateProcedureName(typeMapping));
			sqlCreateSQL.Append("(\r\n");

			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				//add comas after column definitions
				if(addComa){
					sqlCreateSQL.Append(",\r\n");
				}

				sqlCreateSQL.AppendFormat("\t{0} {1}", propertyMapping.ColumnName, propertyMapping.MemberDbType.ToSql());
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateSQL.Append("\r\n");

			sqlCreateSQL.Append(")\r\n");
			sqlCreateSQL.Append("AS\r\n");

			sqlCreateSQL.AppendFormat("UPDATE {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.Append("SET \r\n");
			//Add the columns
			addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateSQL.Append(",\r\n");
				}

				sqlCreateSQL.AppendFormat("\t{1}.{0} = {0}", propertyMapping.ColumnName,typeMapping.TableName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateSQL.Append("\r\n");
			sqlCreateSQL.AppendFormat("WHERE {1}.{0} = {0}\r\n", typeMapping.PrimaryKey.ColumnName,typeMapping.TableName);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateCreateDeleteProcedureSql(TypeMapping typeMapping){

			if(typeMapping.PrimaryKey == null){
				throw new DataStoreException("Can not create delete procedure without defined primary key");
			}

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("CREATE PROCEDURE {0}\r\n", GetDeleteProcedureName(typeMapping));
			sqlCreateSQL.Append("(\r\n");

			IMemberMapping primarykeyMapping = typeMapping.PrimaryKey;
			sqlCreateSQL.AppendFormat("\t@{0} {1}\r\n", primarykeyMapping.ColumnName, primarykeyMapping.MemberDbType.ToSql());

			sqlCreateSQL.Append(")\r\n");
			sqlCreateSQL.Append("AS\r\n");

			sqlCreateSQL.AppendFormat("DELETE FROM {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.AppendFormat("WHERE {0} = @{0}\r\n", typeMapping.PrimaryKey);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateFindByPrimaryKeySql(TypeMapping typeMapping){

			if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + typeMapping.MappedType.FullName);
			}

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("WHERE {1} = @{1}", typeMapping.TableName, typeMapping.PrimaryKey.ColumnName);

			return sqlCreateSQL.ToString();
		}

	

		private void AddDefaultPropertyMappingsTypes(){
			
			AddPropertyMappingType(typeof(Boolean),typeof(OleDbBooleanMapping));
//			AddPropertyMappingType(typeof(Byte[]),typeof(OleDbByteArrayMapping));
			AddPropertyMappingType(typeof(Byte),typeof(OleDbByteMapping));
			AddPropertyMappingType(typeof(Char),typeof(OleDbCharMapping));
			AddPropertyMappingType(typeof(DateTime),typeof(OleDbDateTimeMapping));
			AddPropertyMappingType(typeof(Decimal),typeof(OleDbDecimalMapping));
			AddPropertyMappingType(typeof(Double),typeof(OleDbDoubleMapping));
//			AddPropertyMappingType(typeof(Enum),typeof(OleDbEnumMapping));
			AddPropertyMappingType(typeof(Guid),typeof(OleDbGuidMapping));
			AddPropertyMappingType(typeof(Int16),typeof(OleDbInt16Mapping));
			AddPropertyMappingType(typeof(Int32),typeof(OleDbInt32Mapping));
			AddPropertyMappingType(typeof(Int64),typeof(OleDbInt64Mapping));
			AddPropertyMappingType(typeof(Single),typeof(OleDbSingleMapping));
			AddPropertyMappingType(typeof(String),typeof(OleDbStringMapping));
			
			AddPropertyMappingType(typeof(SqlBoolean),typeof(OleDbSqlBooleanMapping));
			AddPropertyMappingType(typeof(SqlByte),typeof(OleDbSqlByteMapping));
			AddPropertyMappingType(typeof(SqlDateTime),typeof(OleDbSqlDateTimeMapping));
			AddPropertyMappingType(typeof(SqlDecimal),typeof(OleDbSqlDecimalMapping));
			AddPropertyMappingType(typeof(SqlDouble),typeof(OleDbSqlDoubleMapping));
			AddPropertyMappingType(typeof(SqlGuid),typeof(OleDbSqlGuidMapping));
			AddPropertyMappingType(typeof(SqlInt16),typeof(OleDbSqlInt16Mapping));
			AddPropertyMappingType(typeof(SqlInt32),typeof(OleDbSqlInt32Mapping));
			AddPropertyMappingType(typeof(SqlInt64),typeof(OleDbSqlInt64Mapping));
			AddPropertyMappingType(typeof(SqlMoney),typeof(OleDbSqlMoneyMapping));
			AddPropertyMappingType(typeof(SqlSingle),typeof(OleDbSqlSingleMapping));
			AddPropertyMappingType(typeof(SqlString),typeof(OleDbSqlStringMapping));

		}
	
		override public TypeMappingState CheckMapping(TypeMapping typeMapping){

			if(typeMapping == null){
				throw new ArgumentNullException("typeMapping");
			}

			if(!TableExists(typeMapping.TableName)){
				return new TypeMappingState(TypeMappingState.TypeMappingStateCondition.TableMissing);
			}
			
			IDataAccessCommand command = ManagedDataStore.CreateDataAccessCommand("SELECT * FROM " + typeMapping.TableName, CommandType.Text);
			
			//Get Schema
			IDataReader r = null;
			DataTable schema;
			try {
				r = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.CloseConnection);
				schema = r.GetSchemaTable();
			} finally {
				if(r != null){
					r.Close();
				}
			}
			

			//Check Schema
			ArrayList errors = new ArrayList();
			foreach(IMemberMapping mapping in typeMapping.MemberMappings){
				DataRow[] rows = schema.Select("ColumnName = '" + mapping.ColumnName + "'");
				if(rows.Length != 1){
					errors.Add(new MemberMappingError(MemberMappingError.MemberMappingErrorCode.ColumnMissing,mapping));
				}
			}

			if(errors.Count > 0){
				return new TypeMappingState(errors);
			} else {
				return new TypeMappingState(TypeMappingState.TypeMappingStateCondition.None);
			}
		}

		override public bool TableExists(string tableName){
			OleDbConnection conn = (OleDbConnection)ManagedDataStore.Connection.CreateConnection();
			conn.Open();
			DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
				new object[] {null, null, null, "TABLE"});
			conn.Close();

			foreach (DataRow row in schemaTable.Rows) {
				if (((string)(row["TABLE_NAME"])).Equals(tableName))
					return true;
			}
			return false;
		}

		override public string CreateQuery(Type type, string filter, bool polymorphic){

			if(filter != null && filter.TrimStart().ToUpper().StartsWith("SELECT")){
				return filter;
			}

			TypeMapping typeMapping = null;
			if((typeMapping = this.GetTypeMapping(type)) == null){
				throw new DataStoreException("DataStore does not contain storage for " + type.FullName);
			}
			
			StringBuilder sqlQuerySQL = new StringBuilder();
			if(!polymorphic){
				sqlQuerySQL.AppendFormat("SELECT {0}.*{1} FROM {0}", typeMapping.TableName, BuildBaseClassColumnList(typeMapping));
			} else {
				sqlQuerySQL.AppendFormat("SELECT {1}\r\nFROM ", typeMapping.TableName, BuildSubClassColumnList(typeMapping));
				
				string from = JoinSubClasses(typeMapping.TableName, typeMapping);
				sqlQuerySQL.Append(from);
			
			}
			return sqlQuerySQL.ToString() + " " + filter;
		}

		private string JoinSubClasses(string from, TypeMapping typeMapping){
			foreach(Type subClass in typeMapping.SubClasses){
				TypeMapping subClassMapping = GetTypeMapping(subClass);
				from = string.Format("({3} LEFT JOIN {0}\r\n ON {0}.{1} = {2}.{1}) ",subClassMapping.TableName,typeMapping.PrimaryKey.ColumnName,typeMapping.TableName, from);
			}
			return from;
		}
		



	}
}
