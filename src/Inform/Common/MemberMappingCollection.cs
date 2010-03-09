/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

namespace Inform.Common {

	/// <summary>
	/// Represents a collection of IMemberMappings.
	/// </summary>
	public class MemberMappingCollection  : System.Collections.CollectionBase  {

		private Hashtable hashByColumn;
		private Hashtable hashByProperty; 

		public MemberMappingCollection() {
			hashByColumn = new Hashtable();
			hashByProperty = new Hashtable();
		}

		// returns -1 if parameter is null
		public int Add(IMemberMapping value) {
			if (value != null && !hashByColumn.Contains(value.ColumnName)) {
				// throws NotSupportedException
				int i = List.Add(value);
				hashByColumn.Add(value.ColumnName, value);
				hashByProperty.Add(value.Name, value);
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
		public IMemberMapping this[int index] {
			get {
				// ArgumentOutOfRangeException
				return (IMemberMapping)List[index];  
			}
		}
		
		// provide an indexer
		public IMemberMapping this[string columnName] {
			get {
				// ArgumentOutOfRangeException
				return (IMemberMapping)hashByColumn[columnName]; 
			}
		}

		// provide an indexer
		public IMemberMapping GetByName(string name) {
			return (IMemberMapping)hashByProperty[name]; 
		}

		public void Insert(int index, IMemberMapping value) {
			if (value != null && !hashByColumn.Contains(value.ColumnName)) {
				// throws NotSupportedException
				List.Insert(index,value);
				hashByColumn.Add(value.ColumnName, value);
				hashByProperty.Add(value.Name, value);
			}
		}


	}
}
