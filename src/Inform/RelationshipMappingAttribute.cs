/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace Inform {
	/// <summary>
	/// Specifies a parent/child relationship between two Types.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple=true, Inherited=false)] 
	public class RelationshipMappingAttribute : Attribute {

		private string name;
		private Type childType;
		private Type parentType;
		private string parentMember;
		private string childMember;
		private Relationship relationshipType;

		public RelationshipMappingAttribute() {
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
