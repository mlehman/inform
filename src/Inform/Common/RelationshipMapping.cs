using System;

namespace Inform.Common {

	/// <summary>
	/// Represents a parent/child relationship between two Types.
	/// </summary>
	public class RelationshipMapping {

		private string name;
		private Type parentType;
		private Type childType;
		private string parentMember;
		private string childMember;
		private Relationship relationshipType;

		public RelationshipMapping() {
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public Type ParentType {
			get { return parentType; }
			set { parentType = value; }
		}

		public string ParentMember {
			get { return parentMember; }
			set { parentMember = value; }
		}

		public Type ChildType {
			get { return childType; }
			set { childType = value; }
		}

		public string ChildMember {
			get { return childMember; }
			set { childMember = value; }
		}

		public Relationship Type{
			get { return relationshipType; }
			set { relationshipType = value; }
		}


	}
}
