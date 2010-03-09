/*
 * Copyright 2002 Fluent Consulting, Inc. All rights reserved.
 * PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

using System;
using System.Collections;
using System.Data;

using Inform.Common;

namespace Inform.Sql {

	/// <summary>
	/// Summary description for SqlPropertyDbType.
	/// </summary>
	public class SqlMemberDbType : MemberDbType {

		private SqlDbType sqlDbType;
		private int length;
		private int precision;
		private int scale;
		private bool isNullable;

		public static Hashtable sqlToDbType;
		public static Hashtable dbTypeToSql;

		static SqlMemberDbType(){
			sqlToDbType = new Hashtable();
			dbTypeToSql = new Hashtable();

			AddSql(SqlDbType.Bit,"BIT", false);
			AddSql(SqlDbType.Char, "CHAR", false);
			AddSql(SqlDbType.NChar, "NCHAR", false);
			AddSql(SqlDbType.Int, "INT", false);
			AddSql(SqlDbType.VarChar, "VARCHAR", false);
			AddSql(SqlDbType.NVarChar, "NVARCHAR", false);
			AddSql(SqlDbType.DateTime, "DATETIME", false);
			AddSql(SqlDbType.Float, "FLOAT", false);
			AddSql(SqlDbType.Real , "REAL", false);
			AddSql(SqlDbType.Decimal, "DECIMAL", false);
			AddSql(SqlDbType.BigInt, "BIGINT", false);
			AddSql(SqlDbType.VarBinary, "VARBINARY", false);
			AddSql(SqlDbType.UniqueIdentifier, "UNIQUEIDENTIFIER", false);
			AddSql(SqlDbType.TinyInt, "TINYINT", false);
			AddSql(SqlDbType.SmallInt, "SMALLINT", false);
			AddSql(SqlDbType.Money, "MONEY", false);
			AddSql(SqlDbType.NText, "NTEXT", false);
			AddSql(SqlDbType.Text, "TEXT", false);			
		}

		public static void AddSql(SqlDbType dbType, string sql, bool alias){
			sqlToDbType.Add(sql, dbType);
			if(!alias){
				dbTypeToSql.Add(dbType,sql);
			}
		}

		public SqlMemberDbType(SqlDbType sqlDbType) {
			this.sqlDbType = sqlDbType;
		}

		public SqlMemberDbType(SqlDbType sqlDbType, int length) {
			this.sqlDbType = sqlDbType;
			this.length = length;
		}

		public SqlMemberDbType(SqlDbType sqlDbType, bool isNullable) {
			this.sqlDbType = sqlDbType;
			this.isNullable = isNullable;
		}

		public SqlMemberDbType(SqlDbType sqlDbType, int length, bool isNullable) {
			this.sqlDbType = sqlDbType;
			this.length = length;
			this.isNullable = isNullable;
		}

		public SqlMemberDbType(SqlDbType sqlDbType, int precision, int scale) {
			this.sqlDbType = sqlDbType;
			this.precision = precision;
			this.scale = scale;
		}

		override public string DbType {
			get { return SqlDbTypeToSql(sqlDbType); }
			set { sqlDbType = SqlToSqlDbType(value); }
		}

		public SqlDbType SqlDbType {
			get { return sqlDbType; }
			set { sqlDbType = value; }
		}

		override public int Length {
			get { 
				if(length == 0){
					switch(sqlDbType){
						case SqlDbType.VarChar:
							return 50;
						case SqlDbType.NVarChar:
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
			string sql = SqlDbTypeToSql(this.SqlDbType);

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

		private SqlDbType SqlToSqlDbType(string sql){
			return (SqlDbType)sqlToDbType[sql];
		}


		private string SqlDbTypeToSql(SqlDbType sqlDbType) {
			return (string)dbTypeToSql[sqlDbType];
		}

		

//		private string SqlDbTypeToSql(SqlDbType sqlDbType) {
//			//TODO: better to cast enum as string?
//			switch (sqlDbType) {
//				case SqlDbType.Bit :
//					return "BIT";
//				case SqlDbType.Char :
//					return "CHAR";
//				case SqlDbType.NChar :
//					return "NCHAR";
//				case SqlDbType.Int :
//					return "INT";
//				case SqlDbType.VarChar :
//					return "VARCHAR";
//				case SqlDbType.NVarChar :
//					return "NVARCHAR";
//				case SqlDbType.DateTime :
//					return "DATETIME";
//				case SqlDbType.Float :
//					return "FLOAT";
//				case SqlDbType.Real :
//					return "REAL";
//				case SqlDbType.Decimal :
//					return "DECIMAL";
//				case SqlDbType.BigInt :
//					return "BIGINT";
//				case SqlDbType.VarBinary :
//					return "VARBINARY";
//				case SqlDbType.UniqueIdentifier :
//					return "UNIQUEIDENTIFIER";
//				case SqlDbType.TinyInt :
//					return "TINYINT";
//				case SqlDbType.SmallInt :
//					return "SMALLINT";
//				case SqlDbType.Money:
//					return "MONEY";
//				case SqlDbType.NText:
//					return "NTEXT";
//				case SqlDbType.Text:
//					return "TEXT";
//			}
//			throw new NotImplementedException (Enum.GetName(typeof(SqlDbType),sqlDbType));
//		}
	}
}
