using System;
using Inform;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Manager.
	/// </summary>
	public class Manager : Employee {

		private bool certified;

		[MemberMapping]
		public bool Certified {
			get { return this.certified; }
			set { this.certified = value; }
		}
	}
}
