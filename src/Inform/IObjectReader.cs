using System;
using System.Collections;
using System.Data;

namespace Inform {

	/// <summary>
	/// Creates a stream of objects from an IDataReader.
	/// </summary>
	public interface IObjectReader : IEnumerable {

		/// <summary>
		/// Gets a value indicating whether the object reader is closed.
		/// </summary>
		bool IsClosed { get; }

		/// <summary>
		/// Closes the IObjectReader 0bject.
		/// </summary>
		void Close();

		/// <summary>
		/// Retrieves the current object the IObjectReader is reading.
		/// </summary>
		object GetObject();

		/// <summary>
		/// Advances the IObjectReader to the next object.
		/// </summary>
		/// <returns>
		/// true if there are more objects; otherwise, false.
		/// </returns>
		bool Read();

	}
}
