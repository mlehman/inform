/*
 * SqlDataStorageManager.cs	12/26/2002
 *
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Data.SqlTypes;

using Inform.Common;
using Inform.Sql.Mappings;

using Fluent.Text;

namespace Inform.Sql {

	/// <summary>
	/// Summary description for SqlDataStorageManager.
	/// </summary>
	public class SqlDataStorageManager : DataStorageManager {


		public SqlDataStorageManager(SqlDataStore dataStore) : base(dataStore) {
			AddDefaultPropertyMappingsTypes();
		}

		override public bool ExistsStorage(TypeMapping typeMapping){
			return CheckMapping(typeMapping).Condition != TypeMappingState.TypeMappingStateCondition.TableMissing;
		}

		

		override public string GenerateCreateTableSql(TypeMapping typeMapping){
			
			StringBuilder sqlCreateTable = new StringBuilder();

			//Create statement
			sqlCreateTable.Append("CREATE TABLE [dbo].[" + typeMapping.TableName + "] (\r\n");
			
			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				//add comas after column definitions
				if(addComa){
					sqlCreateTable.Append(",\r\n");
				}

				sqlCreateTable.Append("\t[" + propertyMapping.ColumnName + "] "+ propertyMapping.MemberDbType.ToSql());
				
				if (propertyMapping.PrimaryKey){
					if(propertyMapping.Identity){
						sqlCreateTable.Append(" IDENTITY (1, 1) PRIMARY KEY NOT NULL");
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
			return "DROP TABLE [dbo].[" + typeMapping.TableName + "]";
		}

		override public string GenerateCreateRelationshipSql(RelationshipMapping rm){
			//TODO: check rm?
			StringBuilder sqlCreateRelationship = new StringBuilder();
			TypeMapping childTypeMapping = this.GetTypeMapping(rm.ChildType, true);
			TypeMapping parentTypeMapping = this.GetTypeMapping(rm.ParentType, true);
			sqlCreateRelationship.AppendFormat("ALTER TABLE [{0}] \r\n",childTypeMapping.TableName);
			sqlCreateRelationship.AppendFormat("ADD CONSTRAINT {0} \r\n", rm.Name);
			sqlCreateRelationship.AppendFormat("FOREIGN KEY ([{0}]) REFERENCES [{1}]([{2}]) ON DELETE NO ACTION", 
				childTypeMapping.MemberMappings.GetByName(rm.ChildMember).ColumnName, 
				parentTypeMapping.TableName, 
				parentTypeMapping.MemberMappings.GetByName(rm.ParentMember).ColumnName); 
			
			return sqlCreateRelationship.ToString();
		}

		override public string GenerateDropProcedureSql(string procedureName){
			string sql = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[" + procedureName + "]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)\r\n";
			return sql += "DROP PROCEDURE [dbo].[" + procedureName + "]";
		}

		override public string GenerateInsertSql(TypeMapping typeMapping){
			StringBuilder sqlCreateInsert = new StringBuilder();
			sqlCreateInsert.AppendFormat("INSERT INTO {0} (\r\n",typeMapping.TableName);

			//Add the column names
			StringConcatenator concatenator = new StringConcatenator(sqlCreateInsert,",\r\n");
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}
				concatenator.AppendFormat("\t[{0}]", propertyMapping.ColumnName);
			}

			sqlCreateInsert.Append("\r\n) VALUES (\r\n");

			//Add the values
			concatenator.Reset();
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					continue;
				}
				concatenator.AppendFormat("\t@{0}", propertyMapping.ColumnName);
			}

			sqlCreateInsert.Append("\r\n");
			sqlCreateInsert.Append(")\r\n");

			if(typeMapping.PrimaryKey != null && typeMapping.PrimaryKey.Identity){
				sqlCreateInsert.AppendFormat("SELECT @{0} = @@IDENTITY\r\n",typeMapping.PrimaryKey.ColumnName);
			}

			return sqlCreateInsert.ToString();
		}

		override public string GenerateUpdateSql(TypeMapping typeMapping){
			StringBuilder sqlCreateSQL = new StringBuilder();
			sqlCreateSQL.AppendFormat("UPDATE {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.Append("SET \r\n");
			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				if(propertyMapping.PrimaryKey){
					continue;
				}

				//add comas after column definitions
				if(addComa){
					sqlCreateSQL.Append(",\r\n");
				}

				sqlCreateSQL.AppendFormat("\t[{0}] = @{0}", propertyMapping.ColumnName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateSQL.Append("\r\n");
			sqlCreateSQL.AppendFormat("WHERE {0} = @{0}\r\n", typeMapping.PrimaryKey.ColumnName);

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

			sqlCreateInsert.AppendFormat("CREATE PROCEDURE dbo.{0}\r\n", GetInsertProcedureName(typeMapping));
			sqlCreateInsert.Append("(\r\n");

			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				//add comas after column definitions
				if(addComa){
					sqlCreateInsert.Append(",\r\n");
				}

				sqlCreateInsert.AppendFormat("\t@{0} {1}", propertyMapping.ColumnName, propertyMapping.MemberDbType.ToSql());
				
				if(propertyMapping.PrimaryKey && propertyMapping.Identity){
					sqlCreateInsert.Append(" OUTPUT");
				}

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


			if(typeMapping.PrimaryKey.Identity){
				sqlCreateInsert.AppendFormat("SELECT @{0} = @@IDENTITY\r\n",typeMapping.PrimaryKey.ColumnName);
			}

			return sqlCreateInsert.ToString();
		}

		override public string GenerateCreateUpdateProcedureSql(TypeMapping typeMapping){

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("CREATE PROCEDURE dbo.{0}\r\n", GetUpdateProcedureName(typeMapping));
			sqlCreateSQL.Append("(\r\n");

			//Add the columns
			bool addComa = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				
				//add comas after column definitions
				if(addComa){
					sqlCreateSQL.Append(",\r\n");
				}

				sqlCreateSQL.AppendFormat("\t@{0} {1}", propertyMapping.ColumnName, propertyMapping.MemberDbType.ToSql());
				
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

				sqlCreateSQL.AppendFormat("\t[{0}] = @{0}", propertyMapping.ColumnName);
				
				addComa = true;
			}
			//return from last column defintion
			sqlCreateSQL.Append("\r\n");
			sqlCreateSQL.AppendFormat("WHERE {0} = @{0}\r\n", typeMapping.PrimaryKey.ColumnName);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateCreateDeleteProcedureSql(TypeMapping typeMapping){

			if(typeMapping.PrimaryKey == null){
				throw new DataStoreException("Can not create delete procedure without defined primary key");
			}

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("CREATE PROCEDURE dbo.{0}\r\n", GetDeleteProcedureName(typeMapping));
			sqlCreateSQL.Append("(\r\n");

			IMemberMapping primarykeyMapping = typeMapping.PrimaryKey;
			sqlCreateSQL.AppendFormat("\t@{0} {1}\r\n", primarykeyMapping.ColumnName, primarykeyMapping.MemberDbType.ToSql());

			sqlCreateSQL.Append(")\r\n");
			sqlCreateSQL.Append("AS\r\n");

			sqlCreateSQL.AppendFormat("DELETE FROM {0}\r\n",typeMapping.TableName);
			sqlCreateSQL.AppendFormat("WHERE {0} = @{0}\r\n", typeMapping.PrimaryKey.ColumnName);

			return sqlCreateSQL.ToString();
		}

		override public string GenerateFindByPrimaryKeySql(TypeMapping typeMapping){

			if(typeMapping.PrimaryKey == null) {										  
				throw new DataStoreException("TypeMapping does not have defined PrimaryKey for " + typeMapping.MappedType.FullName);
			}

			StringBuilder sqlCreateSQL = new StringBuilder();

			sqlCreateSQL.AppendFormat("WHERE [{0}].[{1}] = @{1}", typeMapping.TableName, typeMapping.PrimaryKey.ColumnName);

			return sqlCreateSQL.ToString();
		}

	

		private void AddDefaultPropertyMappingsTypes(){
			
			AddPropertyMappingType(typeof(string),typeof(SqlStringMapping));
			AddPropertyMappingType(typeof(Byte),typeof(SqlByteMapping));
			AddPropertyMappingType(typeof(Char),typeof(SqlCharMapping));
			AddPropertyMappingType(typeof(Int16),typeof(SqlInt16Mapping));
			AddPropertyMappingType(typeof(int),typeof(SqlInt32Mapping));
			AddPropertyMappingType(typeof(Int64),typeof(SqlInt64Mapping));
			AddPropertyMappingType(typeof(bool),typeof(SqlBoolMapping));
			AddPropertyMappingType(typeof(double),typeof(SqlDoubleMapping));
			AddPropertyMappingType(typeof(float),typeof(SqlSingleMapping));
			AddPropertyMappingType(typeof(Byte[]),typeof(SqlByteArrayMapping));
			AddPropertyMappingType(typeof(Enum),typeof(SqlEnumMapping));
			AddPropertyMappingType(typeof(Decimal),typeof(SqlDecimalMapping));
			AddPropertyMappingType(typeof(DateTime),typeof(SqlDateTimeMapping));
			AddPropertyMappingType(typeof(Guid),typeof(SqlGuidMapping));
			AddPropertyMappingType(typeof(SqlByte),typeof(SqlSqlByteMapping));
			AddPropertyMappingType(typeof(SqlInt16),typeof(SqlSqlInt16Mapping));
			AddPropertyMappingType(typeof(SqlInt32),typeof(SqlSqlInt32Mapping));
			AddPropertyMappingType(typeof(SqlInt64),typeof(SqlSqlInt64Mapping));
			AddPropertyMappingType(typeof(SqlGuid),typeof(SqlSqlGuidMapping));
			AddPropertyMappingType(typeof(SqlSingle),typeof(SqlSqlSingleMapping));
			AddPropertyMappingType(typeof(SqlDouble),typeof(SqlSqlDoubleMapping));
			AddPropertyMappingType(typeof(SqlBoolean),typeof(SqlSqlBooleanMapping));
			AddPropertyMappingType(typeof(SqlDateTime),typeof(SqlSqlDateTimeMapping));
			AddPropertyMappingType(typeof(SqlDecimal),typeof(SqlSqlDecimalMapping));
			AddPropertyMappingType(typeof(SqlMoney),typeof(SqlSqlMoneyMapping));
			AddPropertyMappingType(typeof(SqlString),typeof(SqlSqlStringMapping));
		}
	
		override public TypeMappingState CheckMapping(TypeMapping typeMapping){

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
			SqlDataAccessCommand command = (SqlDataAccessCommand)ManagedDataStore.CreateDataAccessCommand("SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName", CommandType.Text);
			command.CreateInputParameter("@tableName", SqlDbType.VarChar, tableName);
			return ((int)command.ExecuteScalar()) > 0;
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
				sqlQuerySQL.AppendFormat("SELECT {0}.*{1} FROM {0}\r\n", typeMapping.TableName, BuildBaseClassColumnList(typeMapping));
			
//				if(typeMapping.BaseType != null){
//					TypeMapping baseTypeMapping = GetTypeMapping(typeMapping.BaseType);
//					sqlQuerySQL.AppendFormat("INNER JOIN {0}\r\n ON {0}.{1} = {2}.{1}\r\n", baseTypeMapping.TableName, typeMapping.PrimaryKey.ColumnName, typeMapping.TableName);
//				}
			
			} else {
				
				//TODO: extend recursively
				
				sqlQuerySQL.AppendFormat("SELECT {1}{2} FROM {0}\r\n", typeMapping.TableName, BuildSubClassColumnList(typeMapping), BuildBaseClassColumnList(typeMapping));
                AppendSubClassJoins(typeMapping, sqlQuerySQL);

				//TODO: add to oledb

                TypeMapping baseTypeMapping = typeMapping;

                while (baseTypeMapping.BaseType != null) {
                    baseTypeMapping = GetTypeMapping(baseTypeMapping.BaseType);
					sqlQuerySQL.AppendFormat("INNER JOIN {0}\r\n ON {0}.{1} = {2}.{1}\r\n", baseTypeMapping.TableName, typeMapping.PrimaryKey.ColumnName, typeMapping.TableName);
                }

				//sqlQuerySQL.AppendFormat("INNER JOIN {0}\r\n ON {0}.{1} = {2}.{1}\r\n",GetSubclassTableName(typeMapping),typeMapping.PrimaryKey,typeMapping.TableName);
				//sqlQuerySQL.AppendFormat("INNER JOIN {0}\r\n ON {0}.{1} = {2}.{1}\r\n",GetTypeTableName(),"TypeID",GetSubclassTableName(typeMapping));
			}
			return sqlQuerySQL.ToString() + " " + filter;
		}

        private void AppendSubClassJoins(TypeMapping typeMapping, StringBuilder sqlQuerySQL) {
            foreach (Type subClass in typeMapping.SubClasses) {
                TypeMapping subClassMapping = GetTypeMapping(subClass);
                sqlQuerySQL.AppendFormat("LEFT OUTER JOIN {0}\r\n ON {0}.{1} = {2}.{1}\r\n", subClassMapping.TableName, typeMapping.PrimaryKey.ColumnName, typeMapping.TableName);
                AppendSubClassJoins(subClassMapping, sqlQuerySQL);
            }
        }

	}

}
