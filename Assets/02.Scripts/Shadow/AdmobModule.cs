using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;


public class AdmobModule : MonoBehaviour
{
    private RewardedAd rewardedAd;

    private Action onRewardAdSuccessCallback;
    private Action onRewardAdSkippedCallback;

#if !UNITY_IOS
    private string adUnitId = "ca-app-pub-1076272347919893/1081039694";
#elif UNITY_IOS
    private string adUnitId = "ca-app-pub-1076272347919893/4916800748";
#endif


    private bool isAdComplete;

    private Action onAdClosedCallback;

    public void Initialize()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("애드몹 초기화 완료"); });

        onRewardAdSuccessCallback = null;
        onRewardAdSkippedCallback = null;

        LoadRewardAd();
    }

    private void Update()
    {
        if (onAdClosedCallback != null)
        {
            onAdClosedCallback.Invoke();
            onAdClosedCallback = null;
        }
    }

    public void LoadRewardAd()
    {
        rewardedAd = new RewardedAd(adUnitId);

        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleOnAdClosed;

        this.rewardedAd.OnUserEarnedReward += HandleOnEarnReward;
        

        AdRequest request = new AdRequest.Builder()
            .Build();
        
        // Load the interstitial with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void HandleOnEarnReward(object sender, GoogleMobileAds.Api.Reward e)
    {
        Debug.Log("광고 시청완료");

        isAdComplete = true;
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        Debug.Log("광고 Closed");

        onAdClosedCallback = CheckAdComplete;
    }

    private void CheckAdComplete()
    {
        if (isAdComplete)
        {
            Debug.Log("보상 콜백");
            onRewardAdSuccessCallback?.Invoke();
        }
        else
        {
            Debug.Log("스킵 콜백");
            onRewardAdSkippedCallback?.Invoke();
        }

        onRewardAdSuccessCallback = null;
        onRewardAdSkippedCallback = null;

        isAdComplete = false;

        LoadRewardAd();
    }

    public bool CheckRewardAdLoaded()
    {
        bool isLoaded = rewardedAd.IsLoaded();

        if (!isLoaded)
        {
            LoadRewardAd();
        }

        return isLoaded;
    }

    public void TryShowRewardAd(Action onSuccessCallback, Action onSkippedCallback)
    {
        onRewardAdSuccessCallback = onSuccessCallback;
        onRewardAdSkippedCallback = onSkippedCallback;

        if (CheckRewardAdLoaded())
        {
            rewardedAd.Show();
        }
    }
}