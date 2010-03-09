/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

namespace Inform.Common {

	/// <summary>
	/// Represents a collection of DataStore objects.
	/// </summary>
	public class DataStoreCollection  : Inform.Collections.ListDictionaryBase  {

		public DataStoreCollection(){
		}

		public void Add(DataStore dataStore) {
			base.Add(dataStore.Name, dataStore);
		}
		
		public DataStore this[string name] {
			get { return (DataStore)base[name]; }
			set { base[name] = value; }
		}

		public new DataStore this[int index] {
			get { return (DataStore)base[index]; }
		}

	}
}
