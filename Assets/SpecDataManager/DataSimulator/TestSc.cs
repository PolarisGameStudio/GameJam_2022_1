using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSc : MonoBehaviour
{

	public void OnLoadScene()
	{
		SceneManager.LoadScene("DataSimulatorSimple", LoadSceneMode.Additive);
	}
}
