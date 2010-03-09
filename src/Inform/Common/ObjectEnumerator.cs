using System;
using System.Collections;

namespace Inform.Common {
	/// <summary>
	/// This type supports the Inform infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class ObjectEnumerator : IEnumerator {

		IObjectReader reader;

		public ObjectEnumerator(IObjectReader reader) {
			this.reader = reader; 
		}

		#region Implementation of IEnumerator
		public void Reset() {
			throw new NotSupportedException ();
		}

		public bool MoveNext() {
			if (reader.Read()){ 
				return true;
			} else {
				reader.Close ();
				return false;
			}
		}

		public object Current {
			get {
				return reader.GetObject();
			}
		}
		#endregion
	}
}
