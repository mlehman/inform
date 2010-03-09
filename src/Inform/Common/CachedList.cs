using System;
using System.Collections;


namespace Inform.Common {
	/// <summary>
	/// Summary description for CachedList.
	/// </summary>
	public class CachedList : ArrayList {

		private object key;
		private IMemberMapping childPropertyMapping;
		private DataStore dataStore;




		internal CachedList(IList list, DataStore dataStore, IMemberMapping childPropertyMapping, object key) : base(list) {
			this.key = key;
			this.dataStore = dataStore;
			this.childPropertyMapping = childPropertyMapping;
		}

		public override void RemoveAt(int index) {
			throw new NotSupportedException();
		}

		public override void Insert(int index, object value) {
			throw new NotSupportedException();
		}

		public override void Remove(object value) {
			throw new NotSupportedException();
		}

		public override void Clear() {
			throw new NotSupportedException();
		}

		public override int Add(object value) {
			return base.Add(value);
		}

		public override void AddRange(ICollection collection) {
			throw new NotSupportedException();
		}

		public override object this[int index] {
			get {
				return base[index];
			}
			set {
				throw new NotSupportedException();
			}
		}

	}
}