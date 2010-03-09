using System;

namespace Inform.Common {

	/// <summary>
	/// Represents a collection of registered membermapping types for a datastore.
	/// </summary>
	public class RegisteredMemberMappingCollection : Inform.Collections.ListDictionaryBase  {

		public RegisteredMemberMappingCollection(){
		}

		public void Register(Type type, Type memberMappingType) {
			base.Add(type.FullName, memberMappingType);
		}
		
		public Type this[Type type] {
			get { return (Type)base[type.FullName]; }
			set { base[type.FullName] = value; }
		}

		public new Type this[int index] {
			get { return (Type)base[index]; }
		}

	}
}