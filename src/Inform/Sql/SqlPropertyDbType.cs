/*
 * SqlPropertyDbType.cs	12/26/2002
 *
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;

namespace DataCommand.Sql {

	/// <summary>
	/// Summary description for SqlPropertyDbType.
	/// </summary>
	public class SqlPropertyDbType : PropertyDbType {

		public const string VARCHAR = "varchar";
		public const string	INT	= "int";
		public const string BIT = "bit";
		public const string DATETIME = "datetime";
		public const string FLOAT = "float";
		public const string REAL = "real";
		public const string DECIMAL = "decimal";


		private string dbType;
		private int size;
		private int precision;
		private int scale;
		private bool nullable;

		public SqlPropertyDbType(string dbType) {
			this.dbType = dbType;
		}

		public SqlPropertyDbType(string dbType, int size) {
			this.dbType = dbType;
			this.size = size;
		}
		public SqlPropertyDbType(string dbType, bool nullable) {
			this.dbType = dbType;
			this.nullable = nullable;
		}

		public SqlPropertyDbType(string dbType, int size, bool nullable) {
			this.dbType = dbType;
			this.size = size;
			this.nullable = nullable;
		}

		override public int Size {
			get { return size; }
			set { size = value; }
		}

		override public int Precision {
			get { return precision; }
			set { precision = value; }
		}

		override public int Scale {
			get { return scale; }
			set { scale = value; }
		}

		override public bool Nullable {
			get { return nullable; }
			set { nullable = value; }
		}


		override public string ToSql(){
			string sql = dbType;

			//if size intialized
			if (size != 0){
				sql += "(" + size + ")";
			} else if (precision != 0){
				sql += "(" + precision;
				if (scale != 0){
					sql += "," + scale; 
				}
				sql	+= ")";
			}

			return sql;
		}

		public System.Data.SqlDbType GetSqlDbType(){
			switch(this.dbType){
				case BIT:
					return System.Data.SqlDbType.Bit;
				case INT:
					return System.Data.SqlDbType.Int;
				case VARCHAR:
					return System.Data.SqlDbType.VarChar;
				case DATETIME:
					return System.Data.SqlDbType.DateTime;
				case FLOAT:
					return System.Data.SqlDbType.Float;
				case REAL:
					return System.Data.SqlDbType.Real;
				case DECIMAL:
					return System.Data.SqlDbType.Decimal;
				default:
					throw new ApplicationException("Unknown dbType");
			}
		}
	}
}
