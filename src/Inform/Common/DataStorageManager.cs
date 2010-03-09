/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Inform.Common {

	/// <summary>
	/// This type supports the Inform infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public abstract class DataStorageManager {

		private RegisteredMemberMappingCollection registeredMemberTypeMappings;
		private RelationshipMappingCollection relationshipMappings;
		private TypeMappingCollection typeMappings;
		private DataStore dataStore;

		/// <summary>
		/// Provides access to a DataStore's DataStorageManager.
		/// </summary>
		/// <param name="dataStore"></param>
		/// <returns></returns>
		public static DataStorageManager GetDataStorageManager(DataStore dataStore) {
			return dataStore.DataStorageManager;
		}

		/// <summary>
		/// Gets the TypeMappingCollection.
		/// </summary>
		public TypeMappingCollection TypeMappings {
			get { return typeMappings; }
		}

		/// <summary>
		/// Gets the RelationshipMappingCollection.
		/// </summary>
		public RelationshipMappingCollection RelationshipMappings {
			get { return relationshipMappings; }
		}

		/// <summary>
		/// Gets the DataStore that created this DataStorageManager.
		/// </summary>
		protected DataStore ManagedDataStore {
			get { return dataStore; }
		}

		protected DataStorageManager(DataStore dataStore) {
			this.dataStore = dataStore;
			this.typeMappings = new TypeMappingCollection();
			this.registeredMemberTypeMappings = new RegisteredMemberMappingCollection();
			this.relationshipMappings = new RelationshipMappingCollection();
		}

		#region Abstract Methods

		abstract public bool TableExists(string tableName);
		abstract public bool ExistsStorage(TypeMapping typeMapping);

		abstract public string GenerateCreateTableSql(TypeMapping typeMapping);
		abstract public string GenerateDeleteTableSql(TypeMapping typeMapping);

		abstract public string GenerateCreateRelationshipSql(RelationshipMapping rm);

		abstract public string GenerateCreateInsertProcedureSql(TypeMapping typeMapping);
		abstract public string GenerateCreateUpdateProcedureSql(TypeMapping typeMapping);
		abstract public string GenerateCreateDeleteProcedureSql(TypeMapping typeMapping);

		abstract public string GenerateInsertSql(TypeMapping typeMapping);
		abstract public string GenerateUpdateSql(TypeMapping typeMapping);
		abstract public string GenerateDeleteSql(TypeMapping typeMapping);

		abstract public string GenerateDropProcedureSql(string procedureName);
		
		abstract public string GenerateFindByPrimaryKeySql(TypeMapping typeMapping);
		

		#endregion


		public void CreateStorage(TypeMapping typeMapping){
			
			if(typeMapping == null){
				throw new ArgumentNullException("typeMapping");
			}

			IDataAccessCommand createCommand;

			RelationshipMapping baseMapping = null;
			if(typeMapping.BaseType != null){
				
				TypeMapping baseTypeMapping = this.GetTypeMapping(typeMapping.BaseType);
				TypeMappingState state = CheckMapping(baseTypeMapping);
				if (state.Condition == TypeMappingState.TypeMappingStateCondition.TableMissing){
					CreateStorage(baseTypeMapping);
				} else if (state.Condition != TypeMappingState.TypeMappingStateCondition.None) {
					throw new TypeMappingException("TypeMappingState: " + state.Condition.ToString());
				}

				baseMapping = new RelationshipMapping();
				baseMapping.Name = string.Format("{0}_{1}",typeMapping.TableName,baseTypeMapping.TableName);
				baseMapping.ChildType = typeMapping.MappedType;
				baseMapping.ParentType = baseTypeMapping.MappedType;
				baseMapping.ChildMember = typeMapping.PrimaryKey.MemberInfo.Name;
				baseMapping.ParentMember = baseTypeMapping.PrimaryKey.MemberInfo.Name;
				baseMapping.Type = Relationship.OneToOne;
			}
			
			createCommand = ManagedDataStore.CreateDataAccessCommand(
				GenerateCreateTableSql(typeMapping), CommandType.Text);
			createCommand.ExecuteNonQuery();

			if(baseMapping != null){
				createCommand = ManagedDataStore.CreateDataAccessCommand(
					GenerateCreateRelationshipSql(baseMapping), CommandType.Text);
				createCommand.ExecuteNonQuery();
			}

			if(typeMapping.UseStoredProcedures){
				CreateInsertProcedure(typeMapping);
				CreateUpdateProcedure(typeMapping);
				CreateDeleteProcedure(typeMapping);
			}

			foreach(RelationshipMapping rm in RelationshipMappings){
				if(rm.ChildType == typeMapping.MappedType){
					createCommand = ManagedDataStore.CreateDataAccessCommand(
						GenerateCreateRelationshipSql(rm), CommandType.Text);
					createCommand.ExecuteNonQuery();
				}
			}

		}

		public void DeleteStorage(TypeMapping typeMapping){

			IDataAccessCommand deleteCommand; 

			//delete table
			deleteCommand = ManagedDataStore.CreateDataAccessCommand(GenerateDeleteTableSql(typeMapping), CommandType.Text);
			deleteCommand.ExecuteNonQuery();

			//delete insert
			if(typeMapping.UseStoredProcedures){
				deleteCommand = ManagedDataStore.CreateDataAccessCommand(GenerateDropProcedureSql(GetInsertProcedureName(typeMapping)), CommandType.Text);
				deleteCommand.ExecuteNonQuery();

				//delete update
				deleteCommand = ManagedDataStore.CreateDataAccessCommand(GenerateDropProcedureSql(GetUpdateProcedureName(typeMapping)), CommandType.Text);
				deleteCommand.ExecuteNonQuery();

				//delete delete
				deleteCommand = ManagedDataStore.CreateDataAccessCommand(GenerateDropProcedureSql(GetDeleteProcedureName(typeMapping)), CommandType.Text);
				deleteCommand.ExecuteNonQuery();
			}

		}

		public string GetInsertProcedureName(TypeMapping typeMapping){
			return typeMapping.TableName + "_Insert";
		}

		public string GetUpdateProcedureName(TypeMapping typeMapping){
			return typeMapping.TableName + "_Update";
		}

		public string GetDeleteProcedureName(TypeMapping typeMapping){
			return typeMapping.TableName + "_Delete";
		}

		public void CreateInsertProcedure(TypeMapping typeMapping){
			IDataAccessCommand createCommand = ManagedDataStore.CreateDataAccessCommand(GenerateCreateInsertProcedureSql(typeMapping), CommandType.Text);
			createCommand.ExecuteNonQuery();
		}

		public void CreateUpdateProcedure(TypeMapping typeMapping){
			if(typeMapping.PrimaryKey != null){
				IDataAccessCommand createCommand = ManagedDataStore.CreateDataAccessCommand(GenerateCreateUpdateProcedureSql(typeMapping), CommandType.Text);
				createCommand.ExecuteNonQuery();
			}
		}		

		public void CreateDeleteProcedure(TypeMapping typeMapping){
			if(typeMapping.PrimaryKey != null){
				IDataAccessCommand createCommand = ManagedDataStore.CreateDataAccessCommand(GenerateCreateDeleteProcedureSql(typeMapping), CommandType.Text);
				createCommand.ExecuteNonQuery();
			}
		}

		public TypeMapping GetTypeMapping(Type type){
			TypeMapping typeMapping = (TypeMapping)this.typeMappings[type.FullName];

			if(typeMapping == null && this.dataStore.Settings.AutoGenerate){
				typeMapping = CreateTypeMappingFromAttributes(type);
				AddTypeMapping(typeMapping);
				AddDefaultRelationshipMappings(type);
			}

			return typeMapping;
		}

		public TypeMapping GetTypeMapping(Type type, bool throwError){
			TypeMapping typeMapping = GetTypeMapping(type);
			if(typeMapping == null && throwError){
				throw new ApplicationException("There is no TypeMapping defined for " + type.FullName);
			}
			return typeMapping;
		}
		
		public void AddTypeMapping(TypeMapping typeMapping){
			this.typeMappings.Add(typeMapping);
			if(typeMapping.BaseType != null){
				this.GetTypeMapping(typeMapping.BaseType).SubClasses.Add(typeMapping.MappedType);
			}
		}
		
		public IMemberMapping CreateDefaultMemberMapping(MemberInfo memberInfo, MemberMappingAttribute mma){
			
			Type mappedType;

			if(memberInfo is FieldInfo){
				mappedType = ((FieldInfo)memberInfo).FieldType;
			} else if(memberInfo is PropertyInfo) {
				mappedType = ((PropertyInfo)memberInfo).PropertyType;
			} else {
				throw new DataStoreException("Unsupported MemberType: " + memberInfo.MemberType );
			}
			
			IMemberMapping mapping = GetPropertyMapping(mappedType);

			if (mapping == null){
				throw new ArgumentException("Could not create MemberMapping for " + mappedType.FullName );
			}

			if(mma.ColumnName != null){
				mapping.ColumnName = mma.ColumnName;
			} else {
				mapping.ColumnName = memberInfo.Name;
			}

			mapping.PrimaryKey = mma.PrimaryKey;
			mapping.Identity = mma.Identity;
			mapping.MemberDbType.IsNullable = mma.AllowNulls;
			mapping.MemberInfo = memberInfo;

			if(mma.UseDbType){ mapping.MemberDbType.DbType = mma.DbType; }
			//TODO: Only apply to Decimal?
			if(mma.UseLength){ mapping.MemberDbType.Length = mma.Length; }
			if(mma.UsePrecision){ mapping.MemberDbType.Precision = mma.Precision;}
			if(mma.UseScale){ mapping.MemberDbType.Scale = mma.Scale; }

			return mapping;
		}


		/// <summary>
		/// Creates a TypeMapping using special attributes to classes and members.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public TypeMapping CreateTypeMappingFromAttributes(Type type){

			TypeMapping typeMapping = new TypeMapping();
			typeMapping.UseStoredProcedures = dataStore.Settings.UseStoredProcedures;

			//Set the type
			typeMapping.MappedType = type;
			
			TypeMappingAttribute[] typeMappingAttributes = (TypeMappingAttribute[])type.GetCustomAttributes(typeof(TypeMappingAttribute),false);
			TypeMappingAttribute typeMappingAttibute = null;
			if(typeMappingAttributes.Length > 0){
				typeMappingAttibute =  typeMappingAttributes[0];
			}

			if(typeMappingAttibute != null && typeMappingAttibute.BaseType != null){
				typeMapping.BaseType = typeMappingAttibute.BaseType;
			}

			if(typeMappingAttibute != null && typeMappingAttibute.TableName != null){
				typeMapping.TableName = typeMappingAttibute.TableName;
			} else {
				typeMapping.TableName = CreateDefaultTableName(type);
			}

			foreach(MemberInfo memberInfo in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)){
				
				if(!((memberInfo is PropertyInfo) || (memberInfo is FieldInfo))){
					continue;
				}

				MemberMappingAttribute mma = (MemberMappingAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(MemberMappingAttribute), true);

				if(mma != null && !mma.Ignore){

					if(typeMapping.BaseType != null && !mma.PrimaryKey && !MemberIsDeclaredAbove(memberInfo,typeMapping.BaseType)){
						continue;	
					}

					
					if(mma.PrimaryKey){
						if(typeMapping.BaseType != null){ // && MemberIsDeclaredAbove(memberInfo,typeMapping.BaseType) 
							mma.Identity = false;
						} 
					}

					IMemberMapping mapping = CreateDefaultMemberMapping(memberInfo, mma);
					typeMapping.MemberMappings.Add(mapping);

				}

				AddDefaultCacheMappings(memberInfo, typeMapping);
			}

			AddDefaultRelationshipMappings(type);

			if(typeMapping.MemberMappings.Count == 0 && typeMapping.BaseType == null){
				throw new TypeMappingException(string.Format("TypeMapping has no MemberMappings defined for type '{0}'.", type.FullName));
			}
			return typeMapping;
		}

		private void AddDefaultRelationshipMappings(Type type){
			RelationshipMappingAttribute[] relationshipMappingAttributes = (RelationshipMappingAttribute[])type.GetCustomAttributes(typeof(RelationshipMappingAttribute),false);
			foreach(RelationshipMappingAttribute rma in relationshipMappingAttributes){
				RelationshipMapping rm = new RelationshipMapping();
				rm.Name = rma.Name;
				if(rma.ParentType != null){
					rm.ParentType = rma.ParentType;
					rm.ChildType = type;
				} else {
					rm.ParentType = type;
					rm.ChildType = rma.ChildType;
				}
				
				rm.ParentMember = rma.ParentMember;
				rm.ChildMember = rma.ChildMember;

				rm.Type = rma.Type; //TODO: rename RelationshipType

				relationshipMappings.Add(rm);
			}
		}

		private void AddDefaultCacheMappings(MemberInfo memberInfo, TypeMapping typeMapping){

			RelationshipMappingAttribute rma = (RelationshipMappingAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(RelationshipMappingAttribute),false);
			if(rma != null){

				RelationshipMapping rm = new RelationshipMapping();
				rm.Name = rma.Name;
				if(rma.ParentType != null){
					rm.ParentType = rma.ParentType;
					rm.ChildType = typeMapping.MappedType;
				} else {
					rm.ParentType = typeMapping.MappedType;
					rm.ChildType = rma.ChildType;
				}
				rm.ParentMember = rma.ParentMember;
				rm.ChildMember = rma.ChildMember;
				rm.Type = rma.Type;
				relationshipMappings.Add(rm);
			}

			CacheMappingAttribute ca = (CacheMappingAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(CacheMappingAttribute), false);
			if(ca != null){
				CacheMapping cm = new CacheMapping();
				cm.Relationship = ca.Relationship == null ? rma.Name : ca.Relationship;
				cm.MemberInfo = memberInfo;
				cm.DataStore = this.dataStore;
				cm.OrderBy = ca.OrderBy;
				typeMapping.CacheMappings.Add(cm);
			} else if(rma != null){
				CacheMapping cm = new CacheMapping();
				cm.Relationship = rma.Name;
				cm.MemberInfo = memberInfo;
				cm.DataStore = this.dataStore;
				typeMapping.CacheMappings.Add(cm);
			}
		}


		public RelationshipMapping GetRelationshipMapping(string name){
			return (RelationshipMapping)relationshipMappings[name];
		}


		public bool MemberIsDeclaredAbove(MemberInfo memberInfo, Type type){
			Type reflectedType = memberInfo.ReflectedType;
			Type declaringType = memberInfo.DeclaringType;

			while(reflectedType != null){
				if(reflectedType.FullName == type.FullName){
					return false;
				}
				if(reflectedType.FullName == declaringType.FullName){
					return true;
				}
				reflectedType = reflectedType.BaseType;
			}

			return false;
		}
		
		public IMemberMapping GetPropertyMapping(Type type){
			Type memberMappingType = (Type)registeredMemberTypeMappings[type];

			//TODO: Review if this is the best method for capturing enums
			if(type.IsEnum){
				memberMappingType = (Type)registeredMemberTypeMappings[typeof(Enum)];
			}

			if(memberMappingType == null){
				throw new ApplicationException("DataStore does not contain IMemberMapping for " + type.FullName);
			}

			object memberMapping = memberMappingType.GetConstructor(new Type[]{}).Invoke(null);
			
			if(memberMapping == null){
				throw new ApplicationException("DataStore could not create IMemberMapping for " + type.FullName);
			}

			return (IMemberMapping)memberMapping;
		}

		
		public void AddPropertyMappingType(Type type, Type memberMappingType){
			registeredMemberTypeMappings.Register(type, memberMappingType);
		}
		
		internal string CreateDefaultTableName(Type type){

			if(type.Name.EndsWith("Y")){
				return type.Name.Substring(0,type.Name.Length-1) + "IES";
			} else if(type.Name.EndsWith("y")) {
				return type.Name.Substring(0,type.Name.Length-1) + "ies";
			} else if(type.Name.EndsWith("S")) {
				return type.Name + "ES";
			} else if(type.Name.EndsWith("s")) {
				return type.Name + "es";
			} else if(char.IsUpper(type.Name,type.Name.Length-1)) {
				return type.Name + "S";
			} else {
				return type.Name + "s";
			}
			
		}
	
	
		abstract public TypeMappingState CheckMapping(TypeMapping typeMapping);

		abstract public string CreateQuery(Type type, string filter, bool polymorphic);

		public string BuildSubClassColumnList(TypeMapping typeMapping){
			StringBuilder columns = new StringBuilder();
			bool notFirst = false;
			foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
				if(notFirst){
					columns.Append(",");
				} else {
					notFirst = true;
				}
				columns.AppendFormat(" [{0}].[{1}] AS [{0}_{1}]", typeMapping.TableName, propertyMapping.ColumnName);
			}
			foreach(Type subClass in typeMapping.SubClasses){
				string subClassColumns = BuildSubClassColumnList(GetTypeMapping(subClass));
				if(subClassColumns.Length > 0){
					columns.Append("," + subClassColumns);
				}
			}
			return columns.ToString();
		}

		public string PolymorphicPrimaryKey(TypeMapping typeMapping){
			return string.Format("{0}_{1}", typeMapping.TableName, typeMapping.PrimaryKey.ColumnName);
		}

		public string BuildBaseClassColumnList(TypeMapping typeMapping){
			
			if(typeMapping.BaseType != null){
				typeMapping = GetTypeMapping(typeMapping.BaseType);

				StringBuilder columns = new StringBuilder();
				bool addComma = true;
				foreach(IMemberMapping propertyMapping in typeMapping.MemberMappings){
					if(addComma){
						columns.Append(",");
					} else {
						addComma = true;
					}
					columns.AppendFormat(" {0}.{1} AS [{0}_{1}]", typeMapping.TableName, propertyMapping.ColumnName);
				}

				columns.Append(BuildBaseClassColumnList(typeMapping));
				return columns.ToString();
			} else {
				return string.Empty;
			}
		}
	
	}

}

