using System;
using System.Reflection;
using Inform.Common;
using Inform.ProviderBase;

namespace Inform.Common {
	/// <summary>
	/// The mapping to a data source for a cache's relationship.
	/// </summary>
	public class CacheMapping {

		private MemberInfo memberInfo;
		private string relationship;
		private string orderBy;
		private DataStore dataStore;

		protected internal CacheMapping() {
		}

		internal DataStore DataStore {
			get { return dataStore; }
			set { dataStore = value; }
		}

		internal MemberInfo MemberInfo {
			get { return memberInfo; }
			set { memberInfo = value; }
		}

		internal string OrderBy {
			get { return orderBy; }
			set { orderBy = value; }
		}

		/// <summary>
		/// The name of the <see cref="RelationshipMapping"/> used by this cache mapping.
		/// </summary>
		public string Relationship {
			get { return relationship; }
			set { relationship = value; }
		}

		internal void SetContext(object o){
			CacheBase cacheBase;

			if(memberInfo is FieldInfo){
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				cacheBase = fieldInfo.GetValue(o) as CacheBase;
				if(cacheBase == null){
					throw new DataStoreException(string.Format("Could not set context for field: {0} type:{1} ", fieldInfo.Name, o.GetType().FullName));
				}
			} else if (memberInfo is PropertyInfo) {
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				cacheBase = propertyInfo.GetValue(o, null) as CacheBase;
				if(cacheBase == null){
					throw new DataStoreException(string.Format("Could not set context for property: {0} type: {1}", propertyInfo.Name, o.GetType().FullName));
				}
			} else {
				throw new DataStoreException(string.Format("CacheMapping not configured for member: {0} type: {1}", memberInfo.Name, o.GetType().FullName));
			}
			cacheBase.DataStore = dataStore;
			cacheBase.CacheSource = o;
			cacheBase.RelationshipName = relationship;
			
			if(cacheBase is CollectionCache){
				((CollectionCache)cacheBase).OrderBy = OrderBy;
			}
		}
	}
}
