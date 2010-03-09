/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using Inform.Common;
using Inform.ProviderBase;

namespace Inform {
	/// <summary>
	/// Enables the delayed loading of a related object.
	/// </summary>
	public class ObjectCache : CacheBase {
		
		private object cachedObject;

		public Object CachedObject {
			get {
				if(cachedObject == null){
					cachedObject = GetCachedObject();
				}
				return cachedObject;
			}
			set {
				SetCachedObject(value);
			}
		}

		public ObjectCache() {
		}

		public override void Clear(){
			cachedObject = null;
		}


		protected object GetCachedObject(){

			if(this.DataStore == null){
				throw new DataStoreException("Context not set.");
			}

			RelationshipMapping rm = this.DataStore.DataStorageManager.GetRelationshipMapping(RelationshipName);
			if(rm == null) {
				throw new DataStoreException("RelationshipMapping not found: " + RelationshipName );
			}

			IMemberMapping parentMemberMapping = FindMemberMapping(rm.ParentType, rm.ParentMember); 
			if(parentMemberMapping == null) {
				throw new DataStoreException("MemberMapping not found for: " + rm.ParentMember );
			}

			IMemberMapping childMemberMapping = FindMemberMapping(rm.ChildType, rm.ChildMember);
			if(childMemberMapping == null) {
				throw new DataStoreException("MemberMapping not found for: " + rm.ChildMember );
			}

			string relatedColumName;
			string relatedTableName;
			object keyValue;
			Type relatedType;

			if(IsParent(rm)){
				relatedTableName = FindMemberTypeMapping(rm.ChildType, rm.ChildMember).TableName;
				relatedColumName = childMemberMapping.ColumnName;
				keyValue = parentMemberMapping.GetValue(CacheSource);
				relatedType = rm.ChildType;
			} else {
				relatedTableName = FindMemberTypeMapping(rm.ParentType, rm.ParentMember).TableName;
				relatedColumName = parentMemberMapping.ColumnName;
				keyValue = childMemberMapping.GetValue(CacheSource);
				relatedType = rm.ParentType;
			}


			string filter = string.Format("WHERE [{0}].[{1}] = @KeyValue", relatedTableName, relatedColumName);
			IFindObjectCommand cmd = this.DataStore.CreateFindObjectCommand(relatedType, filter, IsPolymorphic(relatedType));
			cmd.CreateInputParameter("@KeyValue", keyValue);
			return cmd.Execute();
		}

		public void SetCachedObject(object cachedObject){

			if(this.DataStore == null){
				throw new DataStoreException("Context not set.");
			}

			RelationshipMapping rm = this.DataStore.DataStorageManager.GetRelationshipMapping(RelationshipName);
			if(rm == null) {
				throw new DataStoreException("RelationshipMapping not found: " + RelationshipName );
			}

			IMemberMapping parentMemberMapping = FindMemberMapping(rm.ParentType, rm.ParentMember); 
			if(parentMemberMapping == null) {
				throw new DataStoreException("MemberMapping not found for: " + rm.ParentMember );
			}

			IMemberMapping childMemberMapping = FindMemberMapping(rm.ChildType, rm.ChildMember);
			if(childMemberMapping == null) {
				throw new DataStoreException("MemberMapping not found for: " + rm.ChildMember );
			}

			object keyValue;


			if(IsParent(rm)){
				keyValue = childMemberMapping.GetValue(cachedObject);
				parentMemberMapping.SetValue(CacheSource, keyValue);

			} else {
				keyValue = parentMemberMapping.GetValue(cachedObject);
				childMemberMapping.SetValue(CacheSource, keyValue);
			}

			this.cachedObject = cachedObject;
		}

		private bool IsParent(RelationshipMapping rm) {
				return rm.ParentType.IsInstanceOfType(CacheSource) && !rm.ChildType.IsInstanceOfType(CacheSource);
		}


	}
}
