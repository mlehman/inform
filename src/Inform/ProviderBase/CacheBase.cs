using System;
using Inform.Common;

namespace Inform.ProviderBase {
	/// <exclude />
	/// <summary>
	/// Aids implementation of a cache.
	/// </summary>
	public abstract class CacheBase {

		private object source;
		private string relationshipName;
		private DataStore dataStore;

		protected internal CacheBase(){}

		/// <exclude />
		/// <summary>
		/// The DataStore used in the relationship.
		/// </summary>
		internal DataStore DataStore {
			get { return dataStore; }
			set { dataStore = value; }
		}

		/// <exclude />
		/// <summary>
		/// The source object in the relationship.
		/// </summary>
		internal object CacheSource {
			get { return source; }
			set { source = value; }
		}

		/// <exclude />
		/// <summary>
		/// The name of the relationship.
		/// </summary>
		internal string RelationshipName {
			get { return relationshipName; }
			set { relationshipName = value; }
		}

		public abstract void Clear();

		//TODO: Consolidate

		internal IMemberMapping FindMemberMapping(Type type, string name){
			TypeMapping typeMapping = this.DataStore.DataStorageManager.GetTypeMapping(type, true);
			IMemberMapping mapping = typeMapping.MemberMappings.GetByName(name);
			if(mapping == null && typeMapping.BaseType != null){
				mapping = FindMemberMapping(typeMapping.BaseType, name);
			}
			return mapping;
		}

		internal TypeMapping FindMemberTypeMapping(Type type, string name){
			TypeMapping typeMapping = this.DataStore.DataStorageManager.GetTypeMapping(type, true);
			IMemberMapping mapping = typeMapping.MemberMappings.GetByName(name);
			if(mapping == null && typeMapping.BaseType != null){
				typeMapping = FindMemberTypeMapping(typeMapping.BaseType, name);
			}
			return typeMapping;
		}

		//TODO: Move centralized?
		internal bool IsPolymorphic(Type type){
			TypeMapping typeMapping = this.DataStore.DataStorageManager.GetTypeMapping(type, true);
			return typeMapping.BaseType != null;
		}
	}
}
