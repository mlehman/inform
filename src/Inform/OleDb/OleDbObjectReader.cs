using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

using Inform.Common;


namespace Inform.OleDb {

	/// <summary>
	/// Summary description for SqlObjectReader.
	/// </summary>
	public class OleDbObjectReader : IObjectReader {

		private Type mappedType;
		private DataStore dataStore;
		private bool polymorphic;
		private OleDbDataReader reader;


		#region Properties

		/// <summary>
		/// Type of object to populate is using mapped population.
		/// </summary>
		public Type MappedType {
			get { return this.mappedType; }
			set { this.mappedType = value; }
		}

		#endregion


		#region Implementation of IObjectReader
		public void Close() {
			this.reader.Close();
		}

		public object GetObject() {
			return PopulateObject(reader);
		}

		public bool Read(){
			Console.WriteLine("READ");
			return this.reader.Read();
			
		}

		public bool IsClosed { 
			get { return this.reader.IsClosed; }
		}
		#endregion


		#region Methods


		internal OleDbObjectReader(OleDbDataStore dataStore, OleDbDataReader reader, Type mappedType, bool polymorphic) {
			this.dataStore = dataStore;
			this.reader = reader;
			this.polymorphic = polymorphic;
			this.MappedType = mappedType;
		}

		/// <summary>
		/// Populates the object from a SqlDataReader.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected object PopulateObject(IDataReader reader) {
			
			if(mappedType == null) throw new InvalidOperationException("MappedType Property is null.");

			TypeMapping typeMapping;
			object dynamicObject = null;

			typeMapping = dataStore.DataStorageManager.GetTypeMapping(mappedType);
			dynamicObject = MappedPopulation(reader, null, typeMapping);
			
			return dynamicObject;

		}


		protected object MappedPopulation(IDataReader reader, object dynamicObject, TypeMapping typeMapping) {

			if(dynamicObject == null){
				if(!polymorphic){
					dynamicObject = mappedType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[]{}, null).Invoke(null);
				} else {
					dynamicObject = GetType(reader, typeMapping).GetConstructor(new Type[]{}).Invoke(null);
					return MappedPopulation(reader, dynamicObject, this.dataStore.DataStorageManager.GetTypeMapping(dynamicObject.GetType()));
				}
			}

			if(typeMapping.BaseType != null){
				TypeMapping baseTypeMapping = null;
				if((baseTypeMapping = dataStore.DataStorageManager.GetTypeMapping(typeMapping.BaseType)) == null){
					throw new ApplicationException("DataStore does not contain storage for " + typeMapping.BaseType.FullName);
				}
				MappedPopulation(reader, dynamicObject, baseTypeMapping);
			} 

			//follow type mapping
			foreach( IMemberMapping mapping in typeMapping.MemberMappings) {
				try {
					if(polymorphic){
						mapping.SetValue(dynamicObject,reader[typeMapping.TableName + "_" + mapping.ColumnName]);
					} else {
						mapping.SetValue(dynamicObject,reader[mapping.ColumnName]);
					}
				} catch (IndexOutOfRangeException iore) {
					throw new DataStoreException("Error populating field: " + mapping.ColumnName, iore);
				} catch (ArgumentException e) {
					throw new DataStoreException("Error populating field: " + mapping.ColumnName ,e);
				}
			}

			foreach( CacheMapping mapping in typeMapping.CacheMappings){
				mapping.SetContext(dynamicObject);
			}
			
			return dynamicObject;
		}

		protected Type GetType(IDataReader reader, TypeMapping typeMapping){
			
			foreach(Type subClass in typeMapping.SubClasses){
				TypeMapping subClassTypeMapping = this.dataStore.DataStorageManager.GetTypeMapping(subClass);
				if(!DBNull.Value.Equals( reader[this.dataStore.DataStorageManager.PolymorphicPrimaryKey(subClassTypeMapping)])){
					return GetType(reader, subClassTypeMapping);
				}
			}

			return typeMapping.MappedType;
		}

		#endregion

		#region Implementation of IEnumerable
		IEnumerator IEnumerable.GetEnumerator () {
			return new ObjectEnumerator(this);
		}
		#endregion
	}
}
