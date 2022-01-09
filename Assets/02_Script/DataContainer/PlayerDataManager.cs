using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataManager : SingletonBehaviour<PlayerDataManager>
{
	private PlayerDataContainer _playerData;
	public static PlayerDataContainer PlayerDataContainer => Instance._playerData;
	
	
	#region ready flag

	[Flags]
	private enum DataManagerReadyFlag
	{
		None = 0,
		PlayerData = 1,
		SpecData = 1 << 1,
		Ready = PlayerData | SpecData,
	}

	private DataManagerReadyFlag _isDataReady = DataManagerReadyFlag.None;

	public bool IsReady => _isDataReady == DataManagerReadyFlag.Ready;

	void Ready(DataManagerReadyFlag flag)
	{
		_isDataReady |= flag;
	}

	#endregion

	
	protected override void Awake()
	{
		base.Awake();

		InitAllData();
	}


	#region DataLoader
	void InitAllData()
	{
		LoadDataFromCache<PlayerDataContainer>(PlayerDataContainer.DefFileName, (ret) =>
		{
			if (ret == null)
			{
				this._playerData = new PlayerDataContainer();
			}
			else
			{
				Debug.Log("파일 로드 완료 " + PlayerDataContainer.DefFileName);
				this._playerData = ret;
			}
			this._playerData.Init();
			Ready(DataManagerReadyFlag.PlayerData);
		});
		
		
		// TODO : spec data load 작업
		Ready(DataManagerReadyFlag.SpecData);
	}
	
	
	void LoadDataFromCache<T>(string fileName, Action<T> callback)
	{
		string cachedPath = Path.Combine(Application.persistentDataPath, fileName);
		string path;
		if (File.Exists(cachedPath))
		{
			path = cachedPath;
		}
		else
		{
			callback(default);
			return;
		}
		DownloadDataFromPath(path, (data) =>
		{
			T retData;
			if (string.IsNullOrEmpty(data) == false)
				retData = JsonUtility.FromJson<T>(data);
			else
				retData = default;
			Debug.Log($"{fileName} is initialized.");
			callback(retData);
		});
	}
	public void DownloadDataFromPath(string filePath, Action<string> callback)
	{
		StartCoroutine(LoadStreamingAsset(filePath, callback));
	}

    
	IEnumerator LoadStreamingAsset(string filePath, Action<string> callback)
	{
		string result = "";

		if (filePath.Contains("://") || filePath.Contains(":///"))
		{
			UnityWebRequest www = UnityWebRequest.Get(filePath);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
				result = "";
			}
			else
			{
				result = www.downloadHandler.text;
				//   Debug.Log(result);
			}
			callback(result);
		}
		else
		{
			if (File.Exists(filePath))
				result = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
			else
				Debug.LogError("File not found : " + filePath);

			callback(result);
		}
	}
	private void OnApplicationFocus(bool focus)
	{
		if (focus == false)
		{
			SaveAllDataToFile();
		}
	}

	protected void OnApplicationQuit()
	{
		SaveAllDataToFile();
	}

	
	void SaveAllDataToFile()
	{
		_playerData?.SaveDataToTextFile();
	}

	#endregion

}