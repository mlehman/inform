using System;

namespace DataCommand {
	/// <summary>
	/// Summary description for PropertyMappingError.
	/// </summary>
	public class PropertyMappingError {

		private IPropertyMapping propertyMapping;
		private PropertyMappingErrorCode errorCode;


		public enum PropertyMappingErrorCode {
			ColumnMissing
		}

		public PropertyMappingError(PropertyMappingErrorCode errorCode, IPropertyMapping propertyMapping) {
			this.propertyMapping = propertyMapping;
			this.errorCode = errorCode;
		}

		public IPropertyMapping PropertyMapping{
			get{ return propertyMapping; }
		}

		public PropertyMappingErrorCode ErrorCode {
			get{ return errorCode; }
		}

		public string GetDescription(){
			switch(ErrorCode){
				case PropertyMappingErrorCode.ColumnMissing:
					return "The column '" + propertyMapping.ColumnName + "' is missing.";
				default:
					return "Unknown error.";
			}
		}



	}
}
