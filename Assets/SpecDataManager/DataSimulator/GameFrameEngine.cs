using System;
using UnityEngine;


namespace DataSimulator
{
	public class GameFrameEngine : MonoBehaviour
	{
		private float _frameDelay;
		private float _lastCalled;

		private void Awake()
		{
			_lastCalled = Time.realtimeSinceStartup;
		}

		public void GameFrame(float delay)
		{
			_frameDelay = Time.realtimeSinceStartup - _lastCalled;
			_lastCalled = Time.realtimeSinceStartup;

//			Debug.Log($"Game frame called {delay}, {_frameDelay}");
		}
	}
}
