using System;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Comment.
	/// </summary>
	public class Comment {
		
		[MemberMapping(PrimaryKey=true,Identity=true,ColumnName="CommentID")]
		private int commentID;

		[MemberMapping(ColumnName="Text", DbType="TEXT")]
		private string text;

		[MemberMapping(ColumnName="Date")]
		private DateTime date;


		private Comment() {
		}

		public Comment(string text){
			this.text = text;
			date = DateTime.Now;
		}

		public int CommentID {
			get { return commentID; }
		}

		public string Text {
			get { return text; }
		}

		public DateTime Date {
			get { return date; }
		}


	}
}
