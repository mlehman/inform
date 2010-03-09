using System;

namespace Inform.Common {

	/// <summary>
	/// Represents a collection of RelationShipMappings.
	/// </summary>
	public class RelationshipMappingCollection  : Inform.Collections.ListDictionaryBase  {

		public RelationshipMappingCollection(){
		}

		public void Add(RelationshipMapping value) {
			base.Add(value.Name,value);
		}
		
		public RelationshipMapping this[string name] {
			get { return (RelationshipMapping)base[name]; }
			set { base[name] = value; }
		}

		public new RelationshipMapping this[int index] {
			get { return (RelationshipMapping)base[index]; }
		}

		public RelationshipMapping Find(Type childType, string childMember){
			foreach(RelationshipMapping rm in this){
				if(rm.ChildType == childType && rm.ChildMember == childMember){
					return rm;
				}
			}
			return null;
		}
	}
}