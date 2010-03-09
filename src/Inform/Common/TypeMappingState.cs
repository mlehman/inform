/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;

namespace Inform.Common {

	/// <summary>
	/// Represents the current state of a <see cref="TypeMapping"/> with the data source. 
	/// </summary>
	public class TypeMappingState {

		private ArrayList errors;
		private TypeMappingStateCondition condition;

		public enum TypeMappingStateCondition {
			None,
			TableMissing,
			MemberErrors
		}

		public TypeMappingState(ArrayList errors) {
			this.condition = TypeMappingStateCondition.MemberErrors;
			this.errors = errors;
		}

		public TypeMappingState(TypeMappingStateCondition condition) {
			this.condition = condition;
			this.errors  = new ArrayList();
		}

		public TypeMappingStateCondition Condition {
			get { return condition; }
		}

		public ArrayList Errors {
			get { return errors; }
		}
		
	}
}
