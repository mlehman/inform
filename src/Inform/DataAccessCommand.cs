/*
 * DataAccessCommand.cs	12/26/2002
 *
 * Copyright 2002 Screen Show, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace DataCommand
{
	/// <summary>
	/// Interface for DataAccessCommand.
	/// </summary>
	public interface DataAccessCommand
	{
		object Execute();
	}
}
