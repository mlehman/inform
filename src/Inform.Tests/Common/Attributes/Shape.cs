using System;
using Inform;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Shape.
	/// </summary>
	public abstract class Shape {

		private int shapeID;
		private string color;

		public Shape() {
		}

		[MemberMapping(PrimaryKey=true,Identity=true)]
		public int ShapeID {
			get{ return shapeID; }
			set{ shapeID = value; }
		}

		[MemberMapping]
		public string Color {
			get{ return color; }
			set{ color = value; }
		}

		public abstract double GetArea();

	}
}
