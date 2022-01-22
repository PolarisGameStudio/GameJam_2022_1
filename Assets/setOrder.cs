using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class setOrder : MonoBehaviour
{
    public List<SpriteRenderer> Renderers; 
    
    [Button]
    public void GetSptieRenderer()
    {
        Renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    [Button]
    public void SetOrder()
    {
        Renderers.ForEach(x => x.sortingLayerName = "Entity");
    }
}
