/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Data;

namespace Inform.Common {

	/// <summary>
	/// This type supports the Inform infrastructure and is not intended to be used directly from your code.
	/// </summary>
	abstract public class MemberDbType {

		abstract public string DbType {
			get;
			set;
		}

		/// <summary>
		/// Length for a numeric data type is the number of bytes used to store the number. 
		/// Length for a character string or Unicode data type is the number of characters. 
		/// The length for binary data types is the number of bytes. 
		/// </summary>
		abstract public int Length {
			get;
			set;
		}

		/// <summary>
		/// Precision is the number of digits in a number.
		/// </summary>
		abstract public int Precision {
			get;
			set;
		}

		/// <summary>
		/// Scale is the number of digits to the right of the decimal point in a number.
		/// </summary>
		abstract public int Scale {
			get;
			set;
		}
		
		abstract public bool IsNullable {
			get;
			set;
		}

		/// <exclude />
		abstract internal string ToSql();
		
	}
}
