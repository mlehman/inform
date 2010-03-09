using System;

namespace Inform {
	/// <summary>
	/// Represents the exception that is thrown when errors are generated using a TypeMapping.
	/// </summary>
	public class TypeMappingException : Exception {
	
		public TypeMappingException(string msg) : base(msg){}
	}
}
