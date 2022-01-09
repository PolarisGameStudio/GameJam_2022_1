using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace GoogleSheetsForUnity
{
	public class GoogleDocsManager
	{
		private ConnectionData _connectionData = null;

		private IGoogleDocsConnection _connection = null;
		public GoogleDocsManager(ConnectionData connectionData)
		{
			_connectionData = connectionData;

#if UNITY_EDITOR
			_connection = UnityEditor.Editor.CreateInstance<GoogleDocsConnectionEditor>();
#else
			_connection = GameObject.FindObjectOfType<GoogleDocsConnection>();
			if (_connection == null)
			{
				GameObject go = new GameObject();
				_connection = go.AddComponent<GoogleDocsConnection>();
			}
#endif
		}

		private Dictionary<string, string> CompleteForm(ref Dictionary<string, string> form)
		{
			form.Add("ssid", _connectionData.spreadsheetId);
			form.Add("pass", _connectionData.servicePassword);

			return form;
		}

		private void CreateRequest(Dictionary<string, string> dataForm, Action<DataContainer> onCallback = null)
		{
			var form = CompleteForm(ref dataForm);
			UnityWebRequest www;
			
			if (_connectionData.usePOST)
			{
				Debug.Log($"Establishing Connection at URL {_connectionData.webServiceUrl}");
				www = UnityWebRequest.Post(_connectionData.webServiceUrl, form);
			}
			else
			{
				string urlParams = "?";
				foreach (KeyValuePair<string, string> item in form)
				{
					urlParams = $"{urlParams}{item.Key}={item.Value}&";
				}

				Debug.LogError($"Establishing Connection at URL {_connectionData.webServiceUrl}, {urlParams}");
				www = UnityWebRequest.Get($"{_connectionData.webServiceUrl}{urlParams}");
			}

			_connection.ExecuteRequest(www, (response, time) => OnResponse(response, time, onCallback));
		}

		void OnResponse(string response, float time, Action<DataContainer> onCallback = null)
		{
			Debug.Log($"{response}\n{time}");
			
			DataContainer dataContainer;
			try
			{
				dataContainer = JsonUtility.FromJson<DataContainer>(response);
			}
			catch (Exception)
			{
				Debug.LogError($"Undefined server response: {response} {time}");
				return;
			}

			if (dataContainer.result == "ERROR")
			{
				Debug.LogError($"{dataContainer.msg}, {time}");
				return;
			}

			if (string.IsNullOrEmpty(dataContainer.result) || dataContainer.result != "ERROR" && dataContainer.result != "OK")
			{
				Debug.LogError($"Undefined server response: {response} {time}");
				return;
			}

			onCallback?.Invoke(dataContainer);
		}
		
		
		/// <summary>
		/// sheet
		/// </summary>
		/// <param name="onComplete"></param>
		public void GetAllSheetData(Action<string, DataTable> onComplete)
		{
			Dictionary<string, string> form = new Dictionary<string, string>();
			form.Add("action", QueryType.getAllTables.ToString());
		
			CreateRequest(form, data =>
			{
				DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(data.payload);

				for (var i = 0; i < dataTable.Rows.Count; i++)
				{
					string keyType = dataTable.Rows[i][0].ToString();
					Debug.LogError(keyType);
					if(keyType.StartsWith("~"))
						continue;
					DataTable dataTable2 = JsonConvert.DeserializeObject<DataTable>(dataTable.Rows[i][1].ToString());
					onComplete(keyType, dataTable2);
				}
			});
		}

		/// <summary>
		/// Retrieves from the spreadsheet an array of all the objects found in the specified table. 
		/// Expects the table name. 
		/// </summary>
		/// <param name="tableTypeName">The name of the table to be retrieved.</param>
		public void GetTable(string tableTypeName, Action<string, DataTable> onComplete)
		{
			Dictionary<string, string> form = new Dictionary<string, string>();
			form.Add("action", QueryType.getTable.ToString());
			form.Add("type", tableTypeName);
		
			CreateRequest(form, data =>
			{
				Debug.Log(data.payload);

				DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(data.payload);

				onComplete(data.objType, dataTable);
			});
		}
	}
}