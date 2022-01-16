using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoWaitToReady());

    }

    IEnumerator CoWaitToReady()
    {
        while (true)
        {
            yield return new WaitUntil(() => DataManager.Instance.IsReady);
            
#if __DATA_SIMULATOR
            SceneManager.LoadScene("DataSimulator");
#else
            SceneManager.LoadScene("02.Main");
#endif
        }
    }
}
