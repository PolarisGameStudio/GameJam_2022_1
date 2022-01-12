using System.Collections;
using System.Collections.Generic;
using Toast.Kit.Ui;
using UnityEngine;

public class MultiLayoutSample : MonoBehaviour
{
    private MultiLayout multiLayout = null;
    private int layout = 0;

    private void Awake()
    {
        multiLayout = GetComponent<MultiLayout>();
    }

    public void ChangeLayout()
    {
        multiLayout.SelectLayout(layout++ % multiLayout.layout.count);
    }
}
