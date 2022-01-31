using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ADType
{
    Rune,
    
    Gacha,
    Reconnect,
    
    None,
}

public class AdManager : SingletonBehaviour<AdManager>
{
    private const string m_PlacementName = "CookApps";

    private ADType m_LastADType;
    
    private Action m_RewardedAdComplete;
    private Action m_RewardedAdFail;

    private AdmobModule admobModule;
    
    protected override void Awake()
    {
        base.Awake();
        
        // if (!RuntimeManager.IsInitialized())
        //     RuntimeManager.Init();
        //
        // Advertising.RewardedAdCompleted += RewardedAdCompletedHandler;
        // Advertising.RewardedAdSkipped += RewardedAdSkippedHandler;
        
        admobModule = gameObject.AddComponent<AdmobModule>();
        admobModule.Initialize();
    }
    
    // public void Start()
    // {
    //     MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
    // }
    //
    // public void ShowMediationTestSuite()
    // {
    //     MediationTestSuite.Show();
    // }
    //
    // public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
    // {
    //     MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
    // }
    
    
    // private IEnumerator LoadAd()
    // {
    //     yield return new WaitForSeconds(0f);
    //
    //     Debug.Log("LoadAd...");
    //     
    //     Advertising.LoadRewardedAd(AdPlacement.PlacementWithName(m_PlacementName));
    // }

    public bool TryShowRequest(ADType adType, Action rewardedAdComplete, Action rewardedAdFail = null)
    {
        Debug.LogError("광고 요청");
        if (DataManager.ShopData.IsAdRemove)
        {
            Debug.LogError("광고 요청 무시");
            m_RewardedAdComplete = rewardedAdComplete;
            RewardedAdCompletedHandler();
            
            return true;
        }
        
        #if UNITY_EDITOR  
        Debug.LogError("에디터 광고무시");
        m_RewardedAdComplete = rewardedAdComplete;
        RewardedAdCompletedHandler();
        
        return true;
        
        #endif
        
        if (!admobModule.CheckRewardAdLoaded())
        {
            // string title = LocalizeText.GetText("UI_Popup_Title");
            // string decript = LocalizeText.GetText("UI_Popup_Description_WaitingAds");
            //
            // UI_Popup.Instance.OpenOK(title, decript);

            return false;
        }
        
        m_RewardedAdComplete = rewardedAdComplete;
        m_RewardedAdFail = rewardedAdFail;

        admobModule.TryShowRewardAd(RewardedAdCompletedHandler,RewardedAdSkippedHandler);

        return true;
    }
    
    void RewardedAdCompletedHandler()
    {
        m_RewardedAdComplete?.Invoke();
        m_RewardedAdComplete = null;
        
        DataManager.AchievementData.ProgressAchievement(Enum_AchivementMission.Daily_AdWatch);
        DataManager.QuestData.OnAdWatch();
    }

    
    void RewardedAdSkippedHandler()
    {
        Debug.Log("광고 취소");
        
        m_RewardedAdFail?.Invoke();
        m_RewardedAdFail = null;
        
     //   MainScene.Instance.SetTimeScale();
    } 
    
    // void RewardedAdCompletedHandler(RewardedAdNetwork network, AdLocation location)
    // {
    //     Debug.Log("Rewarded ad has completed. The user should be rewarded now.");
    //
    //     FirebaseLogManager.Instance.LogAdSpent(m_LastADType);
    //     
    //     m_RewardedAdComplete?.Invoke();
    //     m_RewardedAdComplete = null;
    //     
    //     MainScene.Instance.SetTimeScale();
    // }
    //
    //
    // void RewardedAdSkippedHandler(RewardedAdNetwork network, AdLocation location)
    // {
    //     Debug.Log("광고 취소");
    //     
    //     m_RewardedAdFail?.Invoke();
    //     m_RewardedAdFail = null;
    //     
    //     MainScene.Instance.SetTimeScale();
    // }
}
