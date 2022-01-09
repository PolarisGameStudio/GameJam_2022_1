using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Popup : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 1;
    }
}
