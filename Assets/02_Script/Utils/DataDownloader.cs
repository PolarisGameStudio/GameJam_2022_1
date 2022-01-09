using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DataDownloader : SingletonBehaviour<DataDownloader>
{
    
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
}
