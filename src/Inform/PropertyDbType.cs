/*
 * PropertyDbType.cs	12/26/2002
 *
 * Copyright 2002 Screen Show, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */


using System;

namespace DataCommand {

	/// <summary>
	/// Summary description for DbType.
	/// </summary>
	abstract public class PropertyDbType {

		abstract public int Size {
			get;
			set;
		}

		abstract public int Precision {
			get;
			set;
		}

		abstract public int Scale {
			get;
			set;
		}
		
		abstract public bool Nullable {
			get;
			set;
		}

		abstract public string ToSql();
		
	}
}
