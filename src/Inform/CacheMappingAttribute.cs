/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace Inform {

	/// <summary>
	/// Specifies the configuration of a cache for the delayed loading of a related object or objects.
	/// The CacheAttribute can be applied to a field or read/write property that returns a <see cref="ObjectCache"/> or a <see cref="CollectionCache"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple=true, Inherited=false)] 
	public class CacheMappingAttribute : Attribute {

		private string relationship;
		private string orderBy;

		/// <summary>
		/// The name of a relationship mapping that defines the relationship used in the cache.
		/// <seealso cref="RelationshipMappingAttribute"/>
		/// </summary>
		public string Relationship {
			get { return relationship; }
			set { relationship = value; }
		}

		/// <summary>
		/// </summary>
		public string OrderBy {
			get { return orderBy; }
			set { orderBy = value; }
		}
	}
}
