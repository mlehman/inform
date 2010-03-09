/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

using Inform.Common;

namespace Inform.OleDb {

	/// <summary>
	/// Summary description for SqlPropertyDbType.
	/// </summary>
	public class OleDbMemberDbType : MemberDbType {
	
		private OleDbType oleDbType;
		private int length;
		private int precision;
		private int scale;
		private bool isNullable;

		public static Hashtable sqlToDbType;
		public static Hashtable dbTypeToSql;

		static OleDbMemberDbType(){
			sqlToDbType = new Hashtable();
			dbTypeToSql = new Hashtable();

			AddSql(OleDbType.BigInt,"BIGINT", false);
			AddSql(OleDbType.Boolean, "BIT", false);
			AddSql(OleDbType.Char, "CHAR", false);
			AddSql(OleDbType.Currency, "MONEY", false);
			AddSql(OleDbType.Date, "DATETIME", false);
			AddSql(OleDbType.DBDate, "DATETIME", false);
			AddSql( OleDbType.Decimal, "DECIMAL", false);
			AddSql( OleDbType.Double, "DOUBLE", false);
			AddSql( OleDbType.Guid, "UNIQUEIDENTIFIER", false);
			AddSql( OleDbType.Integer , "INTEGER", false);
			AddSql( OleDbType.Numeric, "NUMERIC", false);
			AddSql( OleDbType.Single, "SINGLE", false);
			AddSql( OleDbType.SmallInt, "SMALLINT", false);
			AddSql( OleDbType.TinyInt, "TINYINT", false);
			AddSql( OleDbType.UnsignedTinyInt, "TINYINT", false);
			AddSql( OleDbType.VarChar, "VARCHAR", false);
			AddSql( OleDbType.VarWChar, "NVARCHAR", false);
			AddSql( OleDbType.WChar, "NCHAR", false);
			
		}

		public static void AddSql(OleDbType dbType, string sql, bool alias){
			sqlToDbType.Add(sql, dbType);
			if(!alias){
				dbTypeToSql.Add(dbType,sql);
			}
		}

		public OleDbMemberDbType(OleDbType oleDbType) {
			this.oleDbType = oleDbType;
		}

		public OleDbMemberDbType(OleDbType oleDbType, int length) {
			this.oleDbType = oleDbType;
			this.length = length;
		}

		public OleDbMemberDbType(OleDbType oleDbType, bool nullable) {
			this.oleDbType = oleDbType;
			this.isNullable = nullable;
		}

		public OleDbMemberDbType(OleDbType oleDbType, int length, bool nullable) {
			this.oleDbType = oleDbType;
			this.length = length;
			this.isNullable = nullable;
		}

		override public string DbType {
			get { return OleDbTypeToSql(oleDbType); }
			set { oleDbType = SqlToOleDbType(value); }
		}

		public OleDbType OleDbType {
			get { return oleDbType; }
			set { oleDbType = value; }
		}

		override public int Length {
			get { 
				if(length == 0){
					switch(oleDbType){
						case OleDbType.VarChar:
							return 50;
						case OleDbType.VarWChar:
							return 50;
					}
				}
				return length; 
			}
			set { length = value; }
		}

		override public int Precision {
			get { return precision; }
			set { precision = value; }
		}

		override public int Scale {
			get { return scale; }
			set { scale = value; }
		}

		override public bool IsNullable {
			get { return isNullable; }
			set { isNullable = value; }
		}


		override internal string ToSql(){
			string sql = OleDbTypeToSql(this.OleDbType);

			//if size intialized
			if (Length != 0){
				sql += "(" + Length + ")";
			} else if (Precision != 0){
				sql += "(" + Precision;
				if (Scale != 0){
					sql += "," + Scale; 
				}
				sql	+= ")";
			}

			return sql;
		}

		private OleDbType SqlToOleDbType(string sql){
			return (OleDbType)sqlToDbType[sql];
		}


		private string OleDbTypeToSql(OleDbType oleDbType) {
			return (string)dbTypeToSql[oleDbType];
		}

			//			switch (oleDbType) {
			//				case OleDbType.BigInt :
			//					return "BIGINT";
			//				case OleDbType.Binary :
			//					throw new NotImplementedException("OleDbType.Binary");
			//				case OleDbType.Boolean :
			//					return "BIT";
			//				case OleDbType.BSTR :
			//					throw new NotImplementedException ("BSTR");
			//				case OleDbType.Char :
			//					return "CHAR";
			//				case OleDbType.Currency :
			//					return "MONEY";
			//				case OleDbType.Date :
			//					return "DATETIME";
			//				case OleDbType.DBDate :
			//					return "DATETIME";
			//				case OleDbType.DBTime :
			//					throw new NotImplementedException ("DBTime");
			//				case OleDbType.DBTimeStamp :
			//					throw new NotImplementedException ("DBTimeStamp");
			//				case OleDbType.Decimal :
			//					return "DECIMAL";
			//				case OleDbType.Double :
			//					return "DOUBLE";
			//				case OleDbType.Empty :
			//					throw new NotImplementedException ("Empty");
			//				case OleDbType.Error :
			//					throw new NotImplementedException ("Error");
			//				case OleDbType.Filetime :
			//					throw new NotImplementedException ("Filetime");
			//				case OleDbType.Guid :
			//					return "UNIQUEIDENTIFIER";
			//				case OleDbType.IDispatch :
			//					throw new NotImplementedException ("IDispatch");
			//				case OleDbType.Integer :
			//					return "INTEGER";
			//				case OleDbType.IUnknown :
			//					throw new NotImplementedException ("IUnknown");
			//				case OleDbType.LongVarBinary :
			//					throw new NotImplementedException ("LongVarBinary");
			//				case OleDbType.LongVarChar :
			//					throw new NotImplementedException ("LongVarChar");
			//				case OleDbType.LongVarWChar :
			//					throw new NotImplementedException ("LongVarWChar");
			//				case OleDbType.Numeric :
			//					return "NUMERIC";
			//				case OleDbType.PropVariant :
			//					throw new NotImplementedException ("PropVariant");
			//				case OleDbType.Single :
			//					return "SINGLE";
			//				case OleDbType.SmallInt :
			//					return "SMALLINT";
			//				case OleDbType.TinyInt :
			//					return "TINYINT";
			//				case OleDbType.UnsignedBigInt :
			//					throw new NotImplementedException ("UnsignedBigInt");
			//				case OleDbType.UnsignedInt :
			//					throw new NotImplementedException ("UnsignedInt");
			//				case OleDbType.UnsignedSmallInt :
			//					throw new NotImplementedException ("UnsignedSmallInt");
			//				case OleDbType.UnsignedTinyInt :
			//					return "TINYINT";
			//				case OleDbType.VarBinary :
			//					throw new NotImplementedException ("VarBinary");
			//				case OleDbType.VarChar :
			//					return "VARCHAR";
			//				case OleDbType.Variant :
			//					throw new NotImplementedException ("Variant");
			//				case OleDbType.VarNumeric :
			//					throw new NotImplementedException ("VarNumeric");
			//				case OleDbType.VarWChar :
			//					return "NVARCHAR";
			//				case OleDbType.WChar :
			//					return "NCHAR";
			//			}
			//			throw new NotImplementedException ("Default");
		//}

	}
}
