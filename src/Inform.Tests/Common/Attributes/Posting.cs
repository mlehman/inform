using System;
using Inform;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for SimpleClass.
	/// </summary>
	public class Posting {
		[MemberMapping(PrimaryKey=true,Identity=true)] public int ID;
		[MemberMapping] public string Topic;
		[MemberMapping] public string Details;
	}
}
