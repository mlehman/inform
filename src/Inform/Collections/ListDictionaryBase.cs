using System;
using System.Collections;

namespace Inform.Collections {
	/// <exclude />
	public class ListDictionaryBase : ICollection, IEnumerable {

		private ArrayList ArrayListBase;
		private Hashtable HashtableBase;

		public ListDictionaryBase() {
			ArrayListBase = new ArrayList();
			HashtableBase = new Hashtable();
		}

		protected void Add(object key, object value) {
			HashtableBase.Add(key,value);
			ArrayListBase.Add(value);
		}

		public void Remove(string name) {
			object removed = HashtableBase[name];
			HashtableBase.Remove(name);
			ArrayListBase.Remove(removed);
		}

		public void Clear() {
			HashtableBase.Clear();
			ArrayListBase.Clear();
		}

		public object this[object key]{
			get { return HashtableBase[key]; }
			set {
				object oldValue = HashtableBase[key];
				HashtableBase[key] = value;
				int index = ArrayListBase.IndexOf(oldValue);
				ArrayListBase[index] = value;
			}
		}

		public object this[int index]{
			get { return ArrayListBase[index]; }
		}

		#region Implementation of ICollection
		public void CopyTo(System.Array array, int index) {
			ArrayListBase.CopyTo(array, index);
		}

		public bool IsSynchronized {
			get { return ArrayListBase.IsSynchronized && HashtableBase.IsSynchronized; }
		}

		public int Count {
			get { return ArrayListBase.Count; }
		}

		public object SyncRoot {
			get { return ArrayListBase.SyncRoot; }
		}
		#endregion

		#region Implementation of IEnumerable
		public System.Collections.IEnumerator GetEnumerator() {
			return ArrayListBase.GetEnumerator();
		}
		#endregion
	}
}
