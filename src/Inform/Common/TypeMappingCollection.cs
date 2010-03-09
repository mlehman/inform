using System;

namespace Inform.Common {

	/// <summary>
	/// Represents a collection of TypeMappings.
	/// </summary>
	public class TypeMappingCollection : Inform.Collections.ListDictionaryBase  {

		public TypeMappingCollection(){
		}

		public void Add(TypeMapping value) {
			base.Add(value.MappedType.FullName,value);
		}

		public TypeMapping this[Type type] {
			get { return (TypeMapping)base[type.FullName]; }
			set { base[type.FullName] = value; }
		}

		public new TypeMapping this[int index] {
			get { return (TypeMapping)base[index]; }
		}

	}
}