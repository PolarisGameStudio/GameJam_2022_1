using System;

namespace GoogleSheetsForUnity
{
	public enum QueryType
	{
		// Create
		createObject,
		createObjects,
		createTable,

		// Retrieve
		getObjectsByField,
		getCellValue,
		getTable,
		getAllTables,

		// Update
		updateObjects,
		setCellValue,

		// Delete
		deleteObjects,
	}

	[Serializable]
	public struct DataContainer
	{
		public string result;
		public string msg;
		public string payload;
		public string query;

		public string objType;
		public string column;
		public string row;
		public string searchField;
		public string searchValue;
		public string value;

		public QueryType QueryType
		{
			get { return (QueryType)Enum.Parse(typeof(QueryType), query); }
		}
	}
}