using System;
using System.Data.SqlTypes;

using Inform.Tests.Common;
using Inform;
using Inform.Sql;
using NUnit.Framework;

namespace Inform.Tests.Common {
	/// <summary>
	/// Summary description for MappingTest.
	/// </summary>
	public abstract class MappingTest {

		protected DataStore dataStore;

		public abstract DataStore CreateDataStore();

		[SetUp]
		public void InitializeConnection() {

			dataStore = CreateDataStore();
			dataStore.Settings.AutoGenerate = true;
		}

		[Test]
		public void TestCommonValueMappings(){
			
			try{
				dataStore.CreateStorage(typeof(CommonValueType));

				CommonValueType cvt = new CommonValueType();

				cvt.BoolValue = true;
				cvt.ByteValue = byte.MaxValue;
				cvt.CharValue = char.MaxValue;
				cvt.Int16Value = short.MaxValue;
				cvt.Int32Value = int.MaxValue;
				cvt.Int64Value = long.MaxValue;
				cvt.SingleValue = float.MaxValue;
				cvt.DoubleValue = long.MaxValue;

				dataStore.Insert(cvt);

				CommonValueType ncvt = (CommonValueType)dataStore.FindByPrimaryKey(typeof(CommonValueType), cvt.ID);

				Assert.AreEqual(cvt.BoolValue, ncvt.BoolValue, "BoolValue");
				Assert.AreEqual(cvt.ByteValue, ncvt.ByteValue, "ByteValue");
				Assert.AreEqual(cvt.CharValue, ncvt.CharValue, "CharValue");
				Assert.AreEqual(cvt.Int16Value, ncvt.Int16Value, "Int16Value");
				Assert.AreEqual(cvt.Int32Value, ncvt.Int32Value, "Int32Value");
				Assert.AreEqual(cvt.Int64Value, ncvt.Int64Value, "Int64Value");
				Assert.AreEqual(cvt.DoubleValue, ncvt.DoubleValue, "DoubleValue");
				Assert.AreEqual(cvt.SingleValue, ncvt.SingleValue, "SingleValue");

				cvt.BoolValue = false;
				cvt.ByteValue = byte.MinValue;
				cvt.CharValue = char.MinValue;
				cvt.Int16Value = short.MinValue;
				cvt.Int32Value = int.MinValue;
				cvt.Int64Value = long.MinValue;
				cvt.SingleValue = float.MinValue;
				cvt.DoubleValue = long.MinValue;

				dataStore.Update(cvt);

				ncvt = (CommonValueType)dataStore.FindByPrimaryKey(typeof(CommonValueType), cvt.ID);

				Assert.AreEqual(cvt.BoolValue, ncvt.BoolValue, "BoolValue");
				Assert.AreEqual(cvt.ByteValue, ncvt.ByteValue, "ByteValue");
				Assert.AreEqual(cvt.CharValue, ncvt.CharValue, "CharValue");
				Assert.AreEqual(cvt.Int16Value, ncvt.Int16Value, "Int16Value");
				Assert.AreEqual(cvt.Int32Value, ncvt.Int32Value, "Int32Value");
				Assert.AreEqual(cvt.Int64Value, ncvt.Int64Value, "Int64Value");
				Assert.AreEqual(cvt.DoubleValue, ncvt.DoubleValue, "DoubleValue");
				Assert.AreEqual(cvt.SingleValue, ncvt.SingleValue, "SingleValue");

			} finally {
				dataStore.DeleteStorage(typeof(CommonValueType));
			}
		}

		[Test]
		public void TestCommonObjectsMappings(){
			
			try{
				dataStore.CreateStorage(typeof(CommonObjectType));

				CommonObjectType cot = new CommonObjectType();

				cot.DateTimeValue = SqlDateTime.MaxValue.Value;;
				cot.DecimalValue = 99999999.9999M;
				cot.GuidValue = Guid.NewGuid();

				dataStore.Insert(cot);

				CommonObjectType ncot = (CommonObjectType)dataStore.FindByPrimaryKey(typeof(CommonObjectType), cot.ID);

				Assert.AreEqual(cot.DateTimeValue, ncot.DateTimeValue, "DateTimeValue");
				Assert.AreEqual(cot.DecimalValue, ncot.DecimalValue, "DecimalValue");
				Assert.AreEqual(cot.GuidValue, ncot.GuidValue, "GuidValue");


				cot.DateTimeValue = SqlDateTime.MinValue.Value;
				cot.DecimalValue = -99999999.9999M;
				cot.GuidValue = Guid.NewGuid();

				dataStore.Update(cot);

				ncot = (CommonObjectType)dataStore.FindByPrimaryKey(typeof(CommonObjectType), cot.ID);

				Assert.AreEqual(cot.DateTimeValue, ncot.DateTimeValue, "DateTimeValue");
				Assert.AreEqual(cot.DecimalValue, ncot.DecimalValue, "DecimalValue");
				Assert.AreEqual(cot.GuidValue, ncot.GuidValue, "GuidValue");

				cot.DateTimeValue = DateTime.Parse("1/1/2004 5:35PM");
				cot.DecimalValue = 12345.2392M;
				cot.GuidValue = Guid.NewGuid();

				dataStore.Update(cot);

				ncot = (CommonObjectType)dataStore.FindByPrimaryKey(typeof(CommonObjectType), cot.ID);

				Assert.AreEqual(cot.DateTimeValue, ncot.DateTimeValue, "DateTimeValue");
				Assert.AreEqual(cot.DecimalValue, ncot.DecimalValue, "DecimalValue");
				Assert.AreEqual(cot.GuidValue, ncot.GuidValue, "GuidValue");

			} finally {
				if(dataStore.ExistsStorage(typeof(CommonObjectType))){
					dataStore.DeleteStorage(typeof(CommonObjectType));
				}
			}
		}

		[Test]
		public void TestCommonSqlTypeMappings(){
			
			try{
				dataStore.CreateStorage(typeof(CommonSqlType));

				CommonSqlType cst = new CommonSqlType();

				cst.SqlBooleanValue = true;
				cst.SqlByteValue = SqlByte.MaxValue;
				cst.SqlDateTimeValue = new SqlDateTime (9999,12,31);
				cst.SqlDecimalValue = 99999999.9999M;
				cst.SqlDoubleValue = SqlDouble.MaxValue;
				cst.SqlGuidValue = new SqlGuid(Guid.NewGuid());
				cst.SqlInt16Value = SqlInt16.MaxValue;
				cst.SqlInt32Value = SqlInt32.MaxValue;
				cst.SqlInt64Value = SqlInt64.MaxValue;
				cst.SqlMoneyValue = SqlMoney.MaxValue;
				cst.SqlSingleValue = SqlSingle.MaxValue;
				cst.SqlStringValue = "SqlStringTest";

				dataStore.Insert(cst);

				CommonSqlType ncst = (CommonSqlType)dataStore.FindByPrimaryKey(typeof(CommonSqlType), cst.ID);

				Assert.AreEqual(cst.SqlBooleanValue, ncst.SqlBooleanValue, "SqlBooleanValue");
				Assert.AreEqual(cst.SqlByteValue, ncst.SqlByteValue, "SqlByteValue");
				Assert.AreEqual(cst.SqlDateTimeValue, ncst.SqlDateTimeValue, "SqlDateTimeValue");
				Assert.AreEqual(cst.SqlDecimalValue, ncst.SqlDecimalValue, "SqlDecimalValue");
				Assert.AreEqual(cst.SqlDoubleValue, ncst.SqlDoubleValue, "SqlDoubleValue");
				Assert.AreEqual(cst.SqlGuidValue, ncst.SqlGuidValue, "SqlGuidValue");
				Assert.AreEqual(cst.SqlInt16Value, ncst.SqlInt16Value, "SqlInt16Value");
				Assert.AreEqual(cst.SqlInt32Value, ncst.SqlInt32Value, "SqlInt32Value");
				Assert.AreEqual(cst.SqlInt64Value, ncst.SqlInt64Value, "SqlInt64Value");
				Assert.AreEqual(cst.SqlMoneyValue, ncst.SqlMoneyValue, "SqlMoneyValue");
				Assert.AreEqual(cst.SqlSingleValue, ncst.SqlSingleValue, "SqlSingleValue");
				Assert.AreEqual(cst.SqlStringValue, ncst.SqlStringValue, "SqlStringValue");

				cst.SqlBooleanValue = false;
				cst.SqlByteValue = SqlByte.MinValue;
				cst.SqlDateTimeValue = SqlDateTime.MinValue;
				cst.SqlDecimalValue = -99999999.9999M;
				cst.SqlDoubleValue = SqlDouble.MinValue;
				cst.SqlGuidValue = new SqlGuid(Guid.NewGuid());
				cst.SqlInt16Value = SqlInt16.MinValue;
				cst.SqlInt32Value = SqlInt32.MinValue;
				cst.SqlInt64Value = SqlInt64.MinValue;
				cst.SqlMoneyValue = SqlMoney.MinValue;
				cst.SqlSingleValue = SqlSingle.MinValue;
				cst.SqlStringValue = "SqlStringTest2";
				
				dataStore.Update(cst);

				ncst = (CommonSqlType)dataStore.FindByPrimaryKey(typeof(CommonSqlType), cst.ID);

				Assert.AreEqual(cst.SqlBooleanValue, ncst.SqlBooleanValue, "SqlBooleanValue");
				Assert.AreEqual(cst.SqlByteValue, ncst.SqlByteValue, "SqlByteValue");
				Assert.AreEqual(cst.SqlDateTimeValue, ncst.SqlDateTimeValue, "SqlDateTimeValue");
				Assert.AreEqual(cst.SqlDecimalValue, ncst.SqlDecimalValue, "SqlDecimalValue");
				Assert.AreEqual(cst.SqlDoubleValue, ncst.SqlDoubleValue, "SqlDoubleValue");
				Assert.AreEqual(cst.SqlGuidValue, ncst.SqlGuidValue, "SqlGuidValue");
				Assert.AreEqual(cst.SqlInt16Value, ncst.SqlInt16Value, "SqlInt16Value");
				Assert.AreEqual(cst.SqlInt32Value, ncst.SqlInt32Value, "SqlInt32Value");
				Assert.AreEqual(cst.SqlInt64Value, ncst.SqlInt64Value, "SqlInt64Value");
				Assert.AreEqual(cst.SqlMoneyValue, ncst.SqlMoneyValue, "SqlMoneyValue");
				Assert.AreEqual(cst.SqlSingleValue, ncst.SqlSingleValue, "SqlSingleValue");
				Assert.AreEqual(cst.SqlStringValue, ncst.SqlStringValue, "SqlStringValue");

				cst.SqlBooleanValue = SqlBoolean.Null;
				cst.SqlByteValue = SqlByte.Null;
				cst.SqlDateTimeValue = SqlDateTime.Null;
				cst.SqlDecimalValue = SqlDecimal.Null;
				cst.SqlDoubleValue = SqlDouble.Null;
				cst.SqlGuidValue = SqlGuid.Null;
				cst.SqlInt16Value = SqlInt16.Null;
				cst.SqlInt32Value = SqlInt32.Null;
				cst.SqlInt64Value = SqlInt64.Null;
				cst.SqlMoneyValue = SqlMoney.Null;
				cst.SqlSingleValue = SqlSingle.Null;
				cst.SqlStringValue = SqlString.Null;
				
				dataStore.Update(cst);

				ncst = (CommonSqlType)dataStore.FindByPrimaryKey(typeof(CommonSqlType), cst.ID);

				if(!(dataStore is Inform.OleDb.OleDbDataStore)){ // not supported in access
					Assert.AreEqual(cst.SqlBooleanValue, ncst.SqlBooleanValue, "SqlBooleanValue");
				}
				Assert.AreEqual(cst.SqlByteValue, ncst.SqlByteValue, "SqlByteValue");
				Assert.AreEqual(cst.SqlDateTimeValue, ncst.SqlDateTimeValue, "SqlDateTimeValue");
				Assert.AreEqual(cst.SqlDecimalValue, ncst.SqlDecimalValue, "SqlDecimalValue");
				Assert.AreEqual(cst.SqlDoubleValue, ncst.SqlDoubleValue, "SqlDoubleValue");
				Assert.AreEqual(cst.SqlGuidValue, ncst.SqlGuidValue, "SqlGuidValue");
				Assert.AreEqual(cst.SqlInt16Value, ncst.SqlInt16Value, "SqlInt16Value");
				Assert.AreEqual(cst.SqlInt32Value, ncst.SqlInt32Value, "SqlInt32Value");
				Assert.AreEqual(cst.SqlInt64Value, ncst.SqlInt64Value, "SqlInt64Value");
				Assert.AreEqual(cst.SqlMoneyValue, ncst.SqlMoneyValue, "SqlMoneyValue");
				Assert.AreEqual(cst.SqlSingleValue, ncst.SqlSingleValue, "SqlSingleValue");
				Assert.AreEqual(cst.SqlStringValue, ncst.SqlStringValue, "SqlStringValue");

			} finally {
				if(dataStore.ExistsStorage(typeof(CommonSqlType))){
					dataStore.DeleteStorage(typeof(CommonSqlType));
				}
			}
		}
	}

	public class CommonValueType {
		[MemberMapping(PrimaryKey=true,Identity=true)] public int ID;

		[MemberMapping] public bool BoolValue;
		[MemberMapping] public byte ByteValue;
		[MemberMapping] public char CharValue;

		[MemberMapping] public short Int16Value;
		[MemberMapping] public int Int32Value;
		[MemberMapping] public long Int64Value;

		[MemberMapping] public double DoubleValue;
		[MemberMapping] public float SingleValue;
	}

	public class CommonObjectType {
		[MemberMapping(PrimaryKey=true,Identity=true)] public int ID;

		[MemberMapping] public DateTime DateTimeValue;
		[MemberMapping(Precision=12, Scale=4)] public decimal DecimalValue;
		[MemberMapping] public Guid GuidValue;
	}

	public class CommonSqlType {
		[MemberMapping(PrimaryKey=true,Identity=true)] public int ID;

		[MemberMapping] public SqlBoolean SqlBooleanValue;
		[MemberMapping] public SqlByte SqlByteValue;
		[MemberMapping] public SqlDateTime SqlDateTimeValue;
		[MemberMapping(Precision=12, Scale=4)] public SqlDecimal SqlDecimalValue;
		[MemberMapping] public SqlDouble SqlDoubleValue;
		[MemberMapping] public SqlGuid SqlGuidValue;
		[MemberMapping] public SqlInt16 SqlInt16Value;
		[MemberMapping] public SqlInt32 SqlInt32Value;
		[MemberMapping] public SqlInt64 SqlInt64Value;
		[MemberMapping] public SqlMoney SqlMoneyValue;
		[MemberMapping] public SqlSingle SqlSingleValue;
		[MemberMapping] public SqlString SqlStringValue;
	}

}
