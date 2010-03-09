using System;

namespace Inform.Common {

	/// <summary>
	/// Summary description for TypeCollection.
	/// </summary>
	public class TypeCollection : Inform.Collections.ListDictionaryBase  {

		public TypeCollection(){
		}

		public void Add(Type value) {
			base.Add(value.FullName, value);
		}
		
		public Type this[string fullName] {
			get { return (Type)base[fullName]; }
			set { base[fullName] = value; }
		}

		public new Type this[int index] {
			get { return (Type)base[index]; }
		}

	}
}