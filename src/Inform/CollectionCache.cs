/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using Inform.Common;
using Inform.ProviderBase;

namespace Inform {

	/// <summary>
	/// Enables the delayed loading of related objects.
	/// </summary>
	/// <remarks>
	/// You can cast the delay loaded objects as your specific object type.
	/// </remarks>
	/// <example>
	/// The following example uses a class that has delay loaded objects.
	/// <code>
	///
	///	Employee e =  DataStoreServices.Default.FindByPrimaryKey(typeof(Employee), 1);
	/// 
	///	//The first access of a CachedCollection will load the related tasks.
	///	foreach(Task t in e.Tasks){
	///		Console.WriteLine("Task: {0}", t.Description);
	///	}
	///
	/// </code>
	/// 
	/// The following example is a class with a mapping specified by attributes. 
	/// The <see cref="RelationshipMappingAttribute"/> applied to the <see cref="CollectionCache"/> is 
	/// a shortcut that implies a <see cref="CacheMappingAttribute"/>.
	/// <code>
	/// public class Employee {
	///
	///		[MemberMapping(PrimaryKey=true,Identity=True)] public string EmployeeID;	
	///		[MemberMapping(length=10, AllowNulls=false)] public string FirstName;
	///		[MemberMapping(length=20, AllowNulls=false)] public string LastName;
	///		[MemberMapping(length=30)] public string Title;
	///
	///		[RelationshipMapping(Name="Employee_Tasks", ParentMember="EmployeeID", 
	///			ChildType=typeof(Task), ChildMember="AssignedEmployeeID"]
	///		private CollectionCache taskCache = new CollectionCache();
	///
	///		public IList Tasks {
	///			get { return this.taskCache.CachedCollection; }
	///		}
	///
	///	}
	///	</code>
	/// </example>
	public class CollectionCache : CacheBase {
		
		private IList cachedCollection;
		private string orderBy;


		/// <summary>
		/// </summary>
		internal string OrderBy {
			get { return orderBy; }
			set { orderBy = value; }
		}

		public override void Clear(){
			cachedCollection = null;
		}

		/// <summary>
		/// Returns the delay loaded objects.
		/// </summary>
		public IList CachedCollection {
			get {
				if(cachedCollection == null){
					cachedCollection = GetCachedCollection();
				}
				return cachedCollection;
			}
		}

		internal IList GetCachedCollection(){

			if(this.DataStore == null){
				throw new DataStoreException("Context not set.");
			}

			RelationshipMapping rm = this.DataStore.DataStorageManager.GetRelationshipMapping(this.RelationshipName);

			IMemberMapping parentPropertyMapping = FindMemberMapping(rm.ParentType, rm.ParentMember);
			if(parentPropertyMapping == null) {
				throw new DataStoreException("PropertyMapping not found for: " + rm.ParentMember );
			}

			IMemberMapping childPropertyMapping = FindMemberMapping(rm.ChildType, rm.ChildMember);
			if(childPropertyMapping == null) {
				throw new DataStoreException("PropertyMapping not found for: " + rm.ChildMember );
			}

			TypeMapping childTypeMapping = FindMemberTypeMapping(rm.ChildType, rm.ChildMember);

			string filter = string.Format("WHERE [{0}].[{1}] = @KeyValue", childTypeMapping.TableName, childPropertyMapping.ColumnName);
			if(OrderBy != null){
				string[] args = OrderBy.Trim().Split(' ');
				string sortedMember = args[0];

				IMemberMapping sortedPropertyMapping = FindMemberMapping(rm.ChildType, sortedMember);
				args[0] = sortedPropertyMapping.ColumnName;
				
				filter += " ORDER BY " + string.Join(" ", args);
			}

			IFindCollectionCommand cmd = this.DataStore.CreateFindCollectionCommand(rm.ChildType, filter, IsPolymorphic(rm.ChildType) );
			cmd.CreateInputParameter("@KeyValue", parentPropertyMapping.GetValue(this.CacheSource));
			return new ArrayList(cmd.Execute());
		}

	}
}
