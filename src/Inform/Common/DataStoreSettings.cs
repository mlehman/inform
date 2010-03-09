using System;

namespace Inform.Common {

	/// <summary>
	/// Represents the settings for a DataStore.
	/// </summary>
	public class DataStoreSettings {

		private bool autoGenerate;
		private bool useStoredProcedures;
		private bool createOnInitialize;
		private bool findObjectReturnsNull = true;

		public DataStoreSettings() {
		}

		/// <summary>
		/// Whether to auto generate type mappings by using attributes.
		/// </summary>
		public bool AutoGenerate {
			get { return autoGenerate; }
			set { autoGenerate = value; }
		}

		/// <summary>
		/// Whether to create storage for type mappings on inialization when storage does not exist.
		/// </summary>
		public bool CreateOnInitialize {
			get { return createOnInitialize; }
			set { createOnInitialize = value; }
		}

		/// <summary>
		/// Whether to use stored procedures as a default on type mappings.
		/// </summary>
		public bool UseStoredProcedures {
			get { return useStoredProcedures; }
			set { useStoredProcedures = value; }
		}

		/// <summary>
		/// Whether FindObject Commands return null or throw an exception.
		/// </summary>
		public bool FindObjectReturnsNull {
			get { return findObjectReturnsNull; }
			set { findObjectReturnsNull = value; }
		}
	}
}
