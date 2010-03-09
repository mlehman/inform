/*
 * SqlDataAccessCommand.cs	12/26/2002
 *
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

namespace DataCommand {

	/// <summary>
	/// Summary description for PropertyMappingCollection.
	/// </summary>
	public class PropertyMappingCollection  : System.Collections.CollectionBase  {

		private Hashtable hash; 

		public PropertyMappingCollection() {
			hash = new Hashtable();
		}

		// returns -1 if parameter is null
		public int Add(IPropertyMapping value) {
			if (value != null) {
				// throws NotSupportedException
				int i = List.Add(value);
				hash.Add(value.ColumnName, value);
				return i;
			}
			else {
				return -1;
			}
		}

		public void CopyTo(Array array, int start) {
			//throws ArgumentOutOfRangeException
			List.CopyTo(array, start); 
		}

		// provide an indexer
		public IPropertyMapping this[int index] {
			get {
				// ArgumentOutOfRangeException
				return (IPropertyMapping)List[index];  
			}
		}
		
		// provide an indexer
		public IPropertyMapping this[string columnName] {
			get {
				// ArgumentOutOfRangeException
				return (IPropertyMapping)hash[columnName]; 
			}
		}

	}
}
