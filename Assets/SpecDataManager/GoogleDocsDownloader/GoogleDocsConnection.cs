using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GoogleSheetsForUnity
{
	interface IGoogleDocsConnection
	{
		void ExecuteRequest(UnityWebRequest www, Action<string, float> onSuccess, float timeOutLimit = 30);
	}
	
	public class GoogleDocsConnection : MonoBehaviour, IGoogleDocsConnection
	{
		private float _timeOutLimit;

		public void ExecuteRequest(UnityWebRequest www, Action<string, float> onSuccess, float timeOutLimit)
		{
			_timeOutLimit = timeOutLimit;
			StartCoroutine(CoExecuteRequest(www, onSuccess));
		}
		
		private IEnumerator CoExecuteRequest(UnityWebRequest www, Action<string, float> onSuccess)
		{
			www.SendWebRequest();
		
			float elapsedTime = 0.0f;
		
			while (!www.isDone)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime >= _timeOutLimit)
				{
					Debug.LogError($"Operation timed out, connection aborted. Check your internet connection and try again. {elapsedTime}");
					yield break;
				}
		
				yield return null;
			}
		
			if (www.isNetworkError)
			{
				Debug.LogError($"Connection error after {elapsedTime} seconds: {www.error}");
				yield break;
			}
		
			onSuccess?.Invoke(www.downloadHandler.text, elapsedTime);
		}
	}
}