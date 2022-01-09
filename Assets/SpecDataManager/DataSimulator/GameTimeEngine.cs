using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;


namespace DataSimulator
{
    
    /// <summary>
    /// testing
    /// </summary>
    public class GameTimeEngine : MonoBehaviour
    {
        [SerializeField] private Text textUpdate;
        [SerializeField] private Text textFixed;
        [SerializeField] private Text textFixedCo;

        private float counter1 = 0;
        private float counter2 = 0;
        private float counter3 = 0;

        private const float _Frequency = (float)(0.3f);
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            // StartCoroutine(CoFixedTimer());
        }

        void Update()
        {
            counter1 += Time.deltaTime;
            textUpdate.text = counter1.ToString();
        }


        [SerializeField] private GameFrameEngine _frameEngine;
        private void FixedUpdate()
        {        
            textFixed.text = counter2.ToString();

            ShowFPS(Time.fixedDeltaTime);
            
            if (counter2 < _Frequency)
            {
                counter2 += Time.fixedDeltaTime;
            }
            else
            {
                _frameEngine.GameFrame(counter2);
                counter2 = 0;
            }
        }

        private int _fps = 0;
        private float _fpsTime = 0;

        void ShowFPS(float frameDelta)
        {
            _fps++;
            _fpsTime += frameDelta * 1000;
            
            if (_fpsTime >= 1000)
            {
                textFixedCo.text = $"{_fps}, {_fpsTime}";
                _fpsTime = 0;
                _fps = 0;
            }
        }


        // IEnumerator CoFixedTimer()
        // {
        //     while (true)
        //     {
        //         counter3 += Time.fixedDeltaTime;
        //         textFixedCo.text = counter3.ToString();
        //         yield return new WaitForFixedUpdate();
        //     }
        // }
    }


}