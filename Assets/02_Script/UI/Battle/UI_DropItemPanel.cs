using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class UI_DropItemPanel : MonoBehaviour ,GameEventListener<RefreshEvent>
{
    // todo: 정식버전에서 아이템정보가 생기면 해당 정보를 큐값으로 큐 쌓기
    // private Queue<DropItemInfo> _dropItemInfos;
    private Queue<int> _dropItemInfos;


    private void OnEnable()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    private void OnDisable()
    {
        this.RemoveGameEventListening<RefreshEvent>();
    }

    private void Start()
    {
        _dropItemInfos = new Queue<int>();
        StartCoroutine(DropItemInfoHandler());
    }

    IEnumerator DropItemInfoHandler()
    {
        var delay = new WaitForSecondsRealtime(0.1f);
        
        while (true)
        {
            ShowDropItemInfo();
            yield return delay;
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        //todo: 프로토 타입용
        if (e.Type == Enum_RefreshEventType.Currency)
        {
            AddDropItemInfo();
        }
    }

    //todo:아이템 정보 받아서 넘겨주기
    private void ShowDropItemInfo()
    {
        if (_dropItemInfos.Count <= 0)
        {
            return;
        }
        
        var dropItem = _dropItemInfos.Dequeue();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).GetComponent<UI_DropItemInfo>().Init(dropItem);
                return;
            }
        }

        var uiDropItemInfo = transform.GetChild(0).GetComponent<UI_DropItemInfo>();

        uiDropItemInfo.gameObject.SetActive(false);
        uiDropItemInfo.Init(dropItem);
    }
    
    private void AddDropItemInfo()
    {
        _dropItemInfos.Enqueue(UnityEngine.Random.Range(1,11));
    }
}