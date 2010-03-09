/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

namespace Inform.Common {

	/// <summary>
	/// Contains a definition of a mapped relationship between a data source table and a <see cref="Type"/>. 
	/// </summary>
	public class TypeMapping {
	
		private Type mappedType;
		private string tableName;
		private MemberMappingCollection memberMappings;
		private CacheMappingCollection cacheMappings; 
		private Type baseType;
		private TypeCollection subClasses;
		private bool useStoredProcedures;

		public TypeMapping() {
			this.memberMappings = new MemberMappingCollection();
			this.subClasses = new TypeCollection();
			this.cacheMappings = new CacheMappingCollection();
		}

		/// <summary>
		/// The Type for this mapping.
		/// </summary>
		public Type MappedType{
			get { return mappedType; }
			set { mappedType = value; }
		}

		/// <summary>
		/// The Base Type used for inherited mappings. 
		/// </summary>
		public Type BaseType{
			get { return baseType; }
			set { baseType = value; }
		}

		/// <summary>
		/// The PrimaryKey for this mapping.
		/// </summary>
		public IMemberMapping PrimaryKey{
			get {
				foreach(IMemberMapping mapping in MemberMappings){
					if(mapping.PrimaryKey){
						return mapping;
					}
				}
				return null;
			}
		}

//		/// <summary>
//		/// The PrimaryKey is an identity.
//		/// </summary>
//		public bool Identity {
//			get { return identity; }
//			set { identity = value; }
//		}

		/// <summary>
		/// The table for this Type.
		/// </summary>
		public String TableName {
			get { return tableName; }
			set { tableName = value; }
		}

		/// <summary>
		/// Whether to use stored procedures.
		/// </summary>
		public bool UseStoredProcedures {
			get { return useStoredProcedures; }
			set { useStoredProcedures = value; }
		}

		/// <summary>
		/// Returns a list of types that are mapped as subclasses.
		/// </summary>
		public TypeCollection SubClasses {
			get { return subClasses; }
		}

		/// <summary>
		/// Returns a list of types that are mapped as subclasses.
		/// </summary>
		public CacheMappingCollection CacheMappings {
			get { return cacheMappings; }
		}

		/// <summary>
		/// The Collection of PropertyMappings.
		/// </summary>
		public MemberMappingCollection MemberMappings {
			get { return memberMappings; }
		}

	}
}
