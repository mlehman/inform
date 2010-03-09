/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace Inform {
	/// <summary>
	/// Represents the exception that is optionally thrown when a <see cref="IFindObjectCommand"/> does not find an object.
	/// </summary>
	public class ObjectNotFoundException : DataStoreException {
		public ObjectNotFoundException() : base("Object was not found.") {
		}

		public ObjectNotFoundException(string message) : base(message) {
		}
	}
}
