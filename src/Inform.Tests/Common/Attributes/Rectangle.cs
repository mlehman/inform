using System;
using Inform;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Rectangle.
	/// </summary>
	[TypeMapping(BaseType=typeof(Shape))]
	public class Rectangle : Shape {

		private int width;
		private int length;

		public Rectangle() {
		}

		[MemberMapping]
		public int Width {
			get{ return width; }
			set{ width = value; }
		}

		[MemberMapping]
		public int Length {
			get{ return length; }
			set{ length = value; }
		}

		override public double GetArea(){
			return Width * Length;
		}
	}
}
