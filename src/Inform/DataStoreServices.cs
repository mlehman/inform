using System;
using System.Xml;

using Inform.Common;

namespace Inform {
	/// <summary>
	/// Provides static methods to aid with DataStore registration and retrieval.
	/// </summary>
	public class DataStoreServices {

		private static object lockObject = new object();
		private static bool initialized;
		private static bool initializing;
		private static DataStoreCollection datastores = new DataStoreCollection();

		
		/// <summary>
		/// Whether a Initialized
		/// </summary>
		public static bool Initialized {
			get { return initialized; }
		}

		/// <summary>
		/// Gets a list of currently registered datastores.
		/// </summary>
		static public DataStoreCollection RegisteredDataStores {
			get {
				EnsureInitialized();
				return datastores; 
			}
		}
		
		/// <summary>
		/// Gets the datastore with the first index in the registered datastores.
		/// </summary>
		static public DataStore Default {
			//TODO: Handle no default, move clone to collection
			get {
				return (DataStore)RegisteredDataStores[0].Clone(); 
			}
		}

		/// <summary>
		/// Returns a registered datastore with the specified name.
		/// </summary>
		static public DataStore GetDataStore(string name) {
			//TODO: move clone to collection
			DataStore ds = RegisteredDataStores[name];
			if(ds != null){
				return (DataStore)ds.Clone();
			} else {
				return null;
			}
		}

		/// <summary>
		/// Registers a datastore with the datastore services.
		/// </summary>
		static public void RegisterDataStore(DataStore dataStore) {
			RegisteredDataStores.Add(dataStore);
		}

		/// <summary>
		/// Unregisters a particular datastore from the registered datastores list.
		/// </summary>
		static public void UnregisterDataStore(string name) {
			RegisteredDataStores.Remove(name);
		}


		/// <summary>
		/// Registers the datastores from the .Net Configuration files.
		/// </summary>
		static public void Initialize(){
			lock(lockObject){
				if(!Initialized){
					try{
						initializing = true;
						RegisteredDataStores.Clear();

						DataStoreConfiguration config = new DataStoreConfiguration();
						config.LoadFromConfigSection(true);
						InitializeStorage();
						initialized = true;
					} finally {
						initializing = false;
					}
				}
			}
		}

		/// <summary>
		/// Registers the datastores from the .Net Configuration files.
		/// </summary>
		static public void Initialize(bool throwerror){
			lock(lockObject){
				if(!Initialized){
					try{
						initializing = true;
						RegisteredDataStores.Clear();

						DataStoreConfiguration config = new DataStoreConfiguration();
						
						config.LoadFromConfigSection(throwerror);
						InitializeStorage();
						initialized = true;
					} finally {
						initializing = false;
					}
				}
			}
		}

		/// <summary>
		/// Initialize the datastores from an external configuration file.
		/// </summary>
		/// <param name="filename">The xml configuration file.</param>
		static public void Initialize(string filename){
			lock(lockObject){
				if(!Initialized){
					try{
						initializing = true;
						RegisteredDataStores.Clear();

						DataStoreConfiguration config = new DataStoreConfiguration();
						config.LoadXml(filename);
						InitializeStorage();
						initialized = true;
					} finally {
						initializing = false;
					}
				}
			}
		}

		/// <summary>
		/// Initialize the datastores from an XmlNode.
		/// </summary>
		/// <param name="filename">The XmlNode.</param>
		static public void Initialize(XmlNode node){
			lock(lockObject){
				if(!Initialized){
					try{
						initializing = true;
						RegisteredDataStores.Clear();

						DataStoreConfiguration config = new DataStoreConfiguration();
						config.LoadXml(node);
						InitializeStorage();
						initialized = true;
					} finally {
						initializing = false;
					}
				}
			}
		}

		//TODO: Rename CreateStorage?
		static internal void InitializeStorage(){
			foreach(DataStore dataStore in RegisteredDataStores){
				if(dataStore.Settings.CreateOnInitialize){
					dataStore.EnsureStorage();
				}
			}
		}

		static internal void EnsureInitialized(){
			if(!Initialized && !initializing){
				Initialize();
			}
		}

	}
}
