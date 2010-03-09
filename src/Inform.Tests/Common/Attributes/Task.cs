using System;
using Inform;

namespace Inform.Tests {

	public class Task {
		
		private int taskID;
		private string projectID;
		private string description;

		[CacheMapping(Relationship="Project_Tasks")]
		private ObjectCache projectCache = new ObjectCache();

		[MemberMapping(PrimaryKey=true,Identity=true)]
		public int TaskID {
			get { return this.taskID; }
			set { this.taskID = value; }
		}

		[MemberMapping]
		public string ProjectID {
			get { return this.projectID; }
			set { this.projectID = value; }
		}

		[MemberMapping(100,false)]
		public string Description {
			get { return this.description; }
			set { this.description = value; }
		}

		[MemberMapping(Ignore=true)]
		public Project Project {
			get { return (Project)this.projectCache.CachedObject; }
		}

	}

	
}
