/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Reflection;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.XPath;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace Inform.Common {

	/// <summary>
	/// This type supports the Inform infrastructure and is not intended to be used directly from your code.
	/// </summary>
	public class DataStoreConfiguration : IConfigurationSectionHandler {

		private static string SECTION_NAME = "inform";
		public event ResolveAppSettingHandler ResolveAppSetting;

		public DataStoreConfiguration() {
		}

		/// <summary>
		/// Initializes the DataStore(s) with setting in the DataStore .Net Configuration Section.
		/// </summary>
		public void LoadFromConfigSection(bool throwerror){
			XmlNode section = ((XmlNode)ConfigurationSettings.GetConfig(SECTION_NAME));
			
			if(section == null){
				if(throwerror){
					throw new ConfigurationException("Config Section not found: '" + SECTION_NAME + "'"); 
				} else {
					return;
				}
			} else {
				section = section.CloneNode(true);
			}

			string file = GetAttribute(section, "file", false);
			if(file != null){
				LoadXml(ResolveFilePath(section, file));	
			} else {
				Load(section);
			}
		}

		private string ResolveFilePath(XmlNode sectionNode, string file) {
			if(Path.IsPathRooted(file)) {
				return file;
			}
			string configPath=ConfigurationException.GetXmlNodeFilename(sectionNode);
			string path = Path.GetDirectoryName(configPath);
			return Path.Combine(path, file);
		}

		/// <summary>
		/// Initialize the DataStore(s) with an external configuration file.
		/// </summary>
		/// <param name="filename">The xml configuration file.</param>
		public void LoadXml(string filename){
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			Load(doc.SelectSingleNode("//inform"));
		}

		/// <summary>
		/// Initialize the DataStore(s) with an external configuration file.
		/// </summary>
		/// <param name="filename">The xml configuration file.</param>
		public void LoadXml(XmlNode node){
			if(node == null){
				throw new ArgumentNullException("node");
			}
			Load(node.Clone());
		}

		private void Load(XmlNode section) {

			//for each DataStore
			foreach(XmlNode dataStoreNode in section.SelectNodes("dataStore")) {
				LoadDataStore(dataStoreNode);
			}
		}

		private void LoadDataStore(XmlNode dataStoreNode){

			DataStore dataStore;

			string provider = GetAttribute(dataStoreNode, "provider", false);
			if(provider == null){
				//Default provider
				dataStore = new Sql.SqlDataStore();
			} else {
				Type type =	Type.GetType(provider);
				dataStore = (DataStore)type.GetConstructor(new Type[]{}).Invoke(null);
			}
			
			string name = GetAttribute(dataStoreNode, "name", true);
			dataStore.Name = name;
			DataStoreServices.RegisterDataStore(dataStore);
			dataStore.Connection.ConnectionString = GetAttribute(dataStoreNode,"connectionString", true);

			try {
				string returnNull = GetAttribute(dataStoreNode,"findObjectReturnsNull", false);
				if(returnNull != null){
					dataStore.Settings.FindObjectReturnsNull = bool.Parse(returnNull);
				}
			} catch(FormatException fe){
				throw new ConfigurationException("Attribute 'findObjectReturnsNull' is not a Boolean Type for DataStore: " + name);
			}
		
			try {
				string autoGenerate = GetAttribute(dataStoreNode,"autoGenerate", false);
				if(autoGenerate != null){
					dataStore.Settings.AutoGenerate = bool.Parse(autoGenerate);
				}
			} catch(FormatException fe){
				throw new ConfigurationException("Attribute 'autoGenerate' is not a Boolean Type for DataStore: " + name);
			}

			try {
				string createOnInitialize = GetAttribute(dataStoreNode,"createOnInitialize", false);
				if(createOnInitialize != null){
					dataStore.Settings.CreateOnInitialize = bool.Parse(createOnInitialize);
				}
			} catch(FormatException fe){
				throw new ConfigurationException("Attribute 'createOnInitialize' is not a Boolean Type for DataStore: " + name);
			}

			CheckUnknownAttributes(dataStoreNode);

			LoadTypeMappings(dataStore, dataStoreNode);
			LoadCommands(dataStore, dataStoreNode);
			
		}

		private void CheckUnknownAttributes(XmlNode node){
			if(node.Attributes.Count != 0){
				throw new ConfigurationException ("Unknown attribute '" + node.Attributes[0].Name + "'", node);
			}
		}


		private void LoadTypeMappings(DataStore dataStore, XmlNode dataStoreNode){
			//TODO: Clean up method

			DataStorageManager mngr = dataStore.DataStorageManager;
			foreach(XmlNode typeMappingNode in dataStoreNode.SelectNodes("typeMapping")) {	
		
				string typeName = GetAttribute(typeMappingNode, "type", true);

				Type type =	Type.GetType(typeName);

				if(type == null) {
					throw new ConfigurationException(string.Format("Could not get Type '{0}' for DataStore '{1}'",typeName, dataStore.Name), typeMappingNode);
				}

				XmlNodeList memberMappingNodes = typeMappingNode.SelectNodes("memberMapping");


				if(memberMappingNodes.Count == 0){
					CheckUnknownAttributes(typeMappingNode);
					mngr.AddTypeMapping(mngr.CreateTypeMappingFromAttributes(type));

				} else {

					TypeMapping typeMapping = new TypeMapping();
					typeMapping.UseStoredProcedures = dataStore.Settings.UseStoredProcedures;

					//Set the type
					typeMapping.MappedType = type;

					string baseType = GetAttribute(typeMappingNode, "baseType", false);
					if(baseType != null){
						typeMapping.BaseType = Type.GetType(baseType);
					}

					string tablename = GetAttribute(typeMappingNode, "table", false);
					if(tablename != null){
						typeMapping.TableName = tablename;
					} else {
						typeMapping.TableName = dataStore.DataStorageManager.CreateDefaultTableName(type);
					}

					CheckUnknownAttributes(typeMappingNode);

					foreach(XmlNode memberMappingNode in memberMappingNodes){

						string member = GetAttribute(memberMappingNode, "member", true);

						MemberInfo[] memberInfos = type.GetMember(member, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
						if(memberInfos.Length != 1){
							throw new ConfigurationException("Could not get member information for " + member);
						} 

						MemberInfo memberInfo = memberInfos[0];

						Type mappedType;

						if(memberInfo is FieldInfo){
							mappedType = ((FieldInfo)memberInfo).FieldType;
						} else if(memberInfo is PropertyInfo) {
							mappedType = ((PropertyInfo)memberInfo).PropertyType;
						} else {
							throw new DataStoreException("Unsupported MemberType: " + memberInfo.MemberType );
						}

						IMemberMapping mapping = dataStore.DataStorageManager.GetPropertyMapping(mappedType);

						if (mapping == null){
							throw new ArgumentException("Could not create MemberMapping for " + mappedType.FullName );
						}

						
						string primaryKey = GetAttribute(memberMappingNode, "primaryKey", false);
						if(primaryKey != null){ 
							mapping.PrimaryKey = bool.Parse(primaryKey);

							string identity = GetAttribute(memberMappingNode, "identity", false);
							mapping.Identity = bool.Parse(identity);
						} 

						string length = GetAttribute(memberMappingNode, "length", false);
						if(length != null){
							mapping.MemberDbType.Length = int.Parse(length);
						}

						string precision = GetAttribute(memberMappingNode, "precision", false);
						if(precision != null){
							mapping.MemberDbType.Precision = int.Parse(precision);
						}

						string scale = GetAttribute(memberMappingNode, "scale", false);
						if(scale != null){
							mapping.MemberDbType.Scale = int.Parse(scale);
						}

						string column = GetAttribute(memberMappingNode, "column", false);
						if(column!= null){
							mapping.ColumnName = column;
						} else {
							mapping.ColumnName = memberInfo.Name;
						}

						string allowNulls = GetAttribute(memberMappingNode, "allowNulls", false);
						if(allowNulls != null){
							mapping.MemberDbType.IsNullable = bool.Parse(allowNulls);
						}

						mapping.MemberInfo = memberInfo;

						CheckUnknownAttributes(memberMappingNode);

						typeMapping.MemberMappings.Add(mapping);
					}

					if(typeMapping.MemberMappings.Count == 0 && typeMapping.BaseType == null){
						throw new TypeMappingException("TypeMapping has no MemberMappings defined.");
					}

					mngr.AddTypeMapping(typeMapping);
					
				}
			}
		}

		private void LoadCommands(DataStore dataStore, XmlNode dataStoreNode){
			DataStorageManager mngr = dataStore.DataStorageManager;
			//TODO: Other Command Types
			foreach(XmlNode commandNode in dataStoreNode.SelectNodes("commands/dataAccessCommand")) {	
		
				try{
					string name = GetAttribute(commandNode, "name", true);
					//TODO: commandtype
					string cmdText = commandNode.InnerText.Trim();

					IDataAccessCommand cmd =  dataStore.CreateDataAccessCommand(cmdText);
					string timeout = GetAttribute(commandNode, "commandTimeout", false);
					if(timeout != null){
						cmd.CommandTimeout = int.Parse(timeout);
						}

					dataStore.Commands.Add(name, cmd);
				} catch (Exception e){
					throw new ConfigurationException(e.Message, e, commandNode);
				}
				
			}
		}

		private string GetAttribute(XmlNode node, string name, bool required){
			if (node.Attributes[name] == null) {
				if(required){
					throw new ConfigurationException("Required attribute '" + name + "' missing for " + node.Name + ".", node);
				}
				return null;
			} else {
				XmlAttribute attr = node.Attributes[name];
				node.Attributes.Remove(attr);
				return ResolveAppSettings(attr.Value);
			}
		}

		private string ResolveAppSettings(string attribute){
			Regex r = new Regex(
				@"\$\{(.*?)\}",
				RegexOptions.IgnoreCase);

			Match m = r.Match(attribute);		
			while (m.Success) {
				Group g = m.Groups[1];
				string key = g.Value;
				string keyValue;
				
				if(ResolveAppSetting != null){
					ResolveAppSettingArgs arg = new ResolveAppSettingArgs(key);
					ResolveAppSetting(this,arg);
					keyValue = arg.Value;
				} else {
					keyValue = ConfigurationSettings.AppSettings[key];
				}
				attribute = attribute.Replace(m.Value, keyValue);
				m = m.NextMatch();			
			}
			return attribute;
		}

		#region Implementation of IConfigurationSectionHandler
		public object Create(object parent, object configContext, System.Xml.XmlNode section) {
			return section;
		}
		#endregion
	}

	public delegate void ResolveAppSettingHandler(object sender, ResolveAppSettingArgs e);

	public class ResolveAppSettingArgs : EventArgs {
		private string key;
		private string keyValue;

		public string Key {
			get { return key;}
		}

		public string Value {
			get { return keyValue; }
			set { keyValue = value; }
		}

		public ResolveAppSettingArgs(string key){
			this.key = key;
		}
	
	}
}
