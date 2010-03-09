using System;

namespace Inform.Common {
	/// <summary>
	/// Represents a collection of CacheMapping objects.
	/// </summary>
	public class CacheMappingCollection : Inform.Collections.ListDictionaryBase  {

		protected internal CacheMappingCollection(){
		}

		public void Add(CacheMapping value) {
			base.Add(value.MemberInfo.Name, value);
		}
		
		public CacheMapping this[string name] {
			get { return (CacheMapping)base[name]; }
			set { base[name] = value; }
		}

		public new CacheMapping this[int index] {
			get { return (CacheMapping)base[index]; }
		}

	}
}