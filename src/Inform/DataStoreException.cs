/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Runtime.Serialization;


namespace Inform {
	/// <summary>
	/// Represents the exception that is thrown when errors are generated using Inform components.
	/// </summary>
	[Serializable]
	public class DataStoreException : ApplicationException {

		protected internal DataStoreException() {
		}

		protected internal DataStoreException(string message) : base(message) {
		}

		protected internal DataStoreException(string message, Exception innerException) : base(message, innerException) {
		}

		protected internal DataStoreException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}
