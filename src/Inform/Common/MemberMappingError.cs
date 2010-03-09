/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace Inform.Common {

	/// <summary>
	/// Represents the error with a member mapping to a data source.
	/// </summary>
	public class MemberMappingError {

		private IMemberMapping memberMapping;
		private MemberMappingErrorCode errorCode;


		/// <summary>
		/// The error code.
		/// </summary>
		public enum MemberMappingErrorCode {
			ColumnMissing
		}

		public MemberMappingError(MemberMappingErrorCode errorCode, IMemberMapping memberMapping) {
			this.memberMapping = memberMapping;
			this.errorCode = errorCode;
		}

		/// <summary>
		/// Gets the IMemberMapping for this error.
		/// </summary>
		public IMemberMapping MemberMapping{
			get{ return memberMapping; }
		}

		/// <summary>
		/// Gets the error code from this error.
		/// </summary>
		public MemberMappingErrorCode ErrorCode {
			get{ return errorCode; }
		}

		/// <summary>
		/// Gets a friendly description of this error.
		/// </summary>
		public string Description {
			get {
				switch(ErrorCode){
					case MemberMappingErrorCode.ColumnMissing:
						return "The column '" + memberMapping.ColumnName + "' is missing.";
					default:
						return "Unknown error.";
				}
			}
		}



	}
}
