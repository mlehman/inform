/* 
 * Copyright 2004 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * 
 * $Revision: 1.1.2.1 $
 */

using System;
using System.Text;

namespace Fluent.Text {
	
	/// <exclude />
	public class StringConcatenator {

		private StringBuilder _stringBuilder;
		private string _delimiter;
		private int _count;

		public string Delimiter {
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public int Count {
			get { return _count; }
		}

		public StringConcatenator() {
			_stringBuilder = new StringBuilder();
		}

		public StringConcatenator(string delimiter){
			_stringBuilder = new StringBuilder();
			_delimiter = delimiter;
		}

		public StringConcatenator(StringBuilder stringBuilder) {
			_stringBuilder = stringBuilder;
		}

		public StringConcatenator(StringBuilder stringBuilder, string delimiter) {
			_stringBuilder = stringBuilder;
			_delimiter = delimiter;
		}

		public void Append(string value){
			AppendDelimiter();
			_stringBuilder.Append(value);
		}

		public void AppendFormat(string format, params object[] args){
			AppendDelimiter();
			_stringBuilder.AppendFormat(format, args);
		}

		private void AppendDelimiter(){
			if(_count++ > 0){
				_stringBuilder.Append(_delimiter);
			}
		}

		public void Reset(){
			_count = 0;
		}

		public override string ToString(){
			return _stringBuilder.ToString();
		}
	}
}
