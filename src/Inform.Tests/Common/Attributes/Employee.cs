using System;
using System.Data.SqlTypes;
using Inform;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Employee.
	/// </summary>
	public class Employee {
		
		private string employeeID;
		private string firstName;
		private string lastName;
		private string title;
		private int salary;
		private bool contract;
		private SqlDateTime dateOfBirth;

		public Employee() {
		}

		[MemberMapping(PrimaryKey=true)]
		public string EmployeeID {
			get { return this.employeeID; }
			set { this.employeeID = value; }
		}

		[MemberMapping(Length=50,AllowNulls=false)]
		public string FirstName {
			get { return this.firstName; }
			set { this.firstName = value; }
		}

		[MemberMapping(Length=50,AllowNulls=false)]
		public string LastName {
			get { return this.lastName; }
			set { this.lastName = value; }
		}

		[MemberMapping(Length=100)]
		public string Title {
			get { return this.title; }
			set { this.title = value; }
		}

		[MemberMapping]
		public int Salary {
			get { return this.salary; }
			set { this.salary = value; }
		}

		[MemberMapping]
		public bool Contract {
			get { return this.contract; }
			set { this.contract = value; }
		}

		[MemberMapping(AllowNulls=true)]
		public SqlDateTime DateOfBirth {
			get { return this.dateOfBirth; }
			set { this.dateOfBirth = value; }
		}
	}
}
