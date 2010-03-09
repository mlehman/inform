/* $Id: Circle.cs,v 1.1.2.1 2004/10/24 17:08:49 mlehman Exp $
 * $Date: 2004/10/24 17:08:49 $
 * $Author: mlehman $
 * $Revision: 1.1.2.1 $
 * $Log: Circle.cs,v $
 * Revision 1.1.2.1  2004/10/24 17:08:49  mlehman
 * no message
 *
 * Revision 1.1.2.4  2004/05/18 11:49:31  mlehman
 * Removed constructor
 *
 * Revision 1.1.2.3  2004/05/18 11:48:18  mlehman
 * no message
 *
 */
using System;

namespace Inform.Tests {
	/// <summary>
	/// Summary description for Circle.
	/// </summary>
	[TypeMapping(BaseType=typeof(Shape))]
	public class Circle : Shape {

		public int radius;

		[MemberMapping]
		public int Radius {
			get{ return radius; }
			set{ radius = value; }
		}

		override public double GetArea(){
			return 2 * Math.PI * Math.Pow(radius,2);
		}

	}
}
