using System;
using UnityEditor;
using UnityEngine.Networking;

#if UNITY_EDITOR
namespace GoogleSheetsForUnity
{
	public class GoogleDocsConnectionEditor : Editor, IGoogleDocsConnection
	{
		private UnityWebRequest _www;
		private double _elapsedTime = 0.0f;
		private double _startTime = 0.0f;
		private float _timeOutLimit = 30f;

		private Action<string, float> _onSuccess;
		
		public void ExecuteRequest(UnityWebRequest www, Action<string, float> onSuccess, float timeOutLimit)
		{
			_onSuccess = onSuccess;
			_timeOutLimit = timeOutLimit;
			
			EditorApplication.update += EditorUpdate;
			_startTime = EditorApplication.timeSinceStartup;
			_www = www;
			_www.SendWebRequest();
		}

		private void EditorUpdate()
		{
			while (!_www.isDone)
			{
				_elapsedTime = EditorApplication.timeSinceStartup - _startTime;
				if (_elapsedTime >= _timeOutLimit)
				{
					Debug.LogError($"TIME_OUT. {_elapsedTime}");
					EditorApplication.update -= EditorUpdate;
				}
				return;
			}

			if (_www.isNetworkError)
			{
				Debug.LogError($"Connection error after {_elapsedTime} seconds: {_www.error}");
				return;
			}
			
			_onSuccess?.Invoke(_www.downloadHandler.text, (float)_elapsedTime);

			EditorApplication.update -= EditorUpdate;
		}
		//
		// private IEnumerator CoExecuteRequest(UnityWebRequest www, Action<string, float> onSuccess)
		// {
		// 	www.SendWebRequest();
		//
		// 	float elapsedTime = 0.0f;
		//
		// 	while (!www.isDone)
		// 	{
		// 		elapsedTime += Time.deltaTime;
		// 		if (elapsedTime >= connectionData.timeOutLimit)
		// 		{
		// 			Debug.LogError($"Operation timed out, connection aborted. Check your internet connection and try again. {elapsedTime}");
		// 			yield break;
		// 		}
		//
		// 		yield return null;
		// 	}
		//
		// 	if (www.result == UnityWebRequest.Result.ConnectionError)
		// 	{
		// 		Debug.LogError($"Connection error after {elapsedTime} seconds: {www.error}");
		// 		yield break;
		// 	}
		//
		// 	onSuccess?.Invoke(www.downloadHandler.text, elapsedTime);
		// }

	}
}
#endif
