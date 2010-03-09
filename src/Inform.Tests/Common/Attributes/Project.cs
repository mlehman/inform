using System;
using System.Collections;
using Inform;

namespace Inform.Tests {

	public class Project {

		private string projectID;
		private string name;
		private string managerID;

		[RelationshipMapping(Name="Project_Manager", ChildMember="ManagerID", 
			 ParentType=typeof(Manager), ParentMember="EmployeeID", Type=Relationship.OneToOne)]
		private ObjectCache managerCache = new ObjectCache();

		[RelationshipMapping(Name="Project_Tasks", ParentMember="ProjectID", 
			 ChildType=typeof(Task), ChildMember="ProjectID", Type=Relationship.OneToMany)]
		private CollectionCache taskCache = new CollectionCache();

		[MemberMapping(PrimaryKey=true)]
		public string ProjectID {
			get { return this.projectID; }
			set { this.projectID = value; }
		}

		[MemberMapping]
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		[MemberMapping]
		public string ManagerID {
			get { return this.managerID; }
			set { this.managerID = value; }
		}

		[MemberMapping(Ignore=true)]
		public Manager Manager {
			get { return (Manager)this.managerCache.CachedObject; }
			set { this.managerCache.CachedObject = value; }
		}

		[MemberMapping(Ignore=true)]
		public IList Tasks {
			get { return this.taskCache.CachedCollection; }
		}
	}
}
