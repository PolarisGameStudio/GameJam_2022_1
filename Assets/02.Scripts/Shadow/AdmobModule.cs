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



    private string adUnitId = "ca-app-pub-1076272347919893/1172881253";
    //private string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    private bool isAdComplete;

    private Action onAdClosedCallback;

    public void Initialize()
    {
        MobileAds.Initialize(initStatus => { Debug.LogError("애드몹 초기화 완료"); });

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
        this.rewardedAd.OnAdFailedToLoad += OnAdFailedToLoad;
        

        AdRequest request = new AdRequest.Builder()
            .Build();
        
        // Load the interstitial with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        LoadAdError loadAdError = args.LoadAdError;

        // Gets the domain from which the error came.
        string domain = loadAdError.GetDomain();
        Debug.LogError(domain); 

        // Gets the error code. See
        // https://developers.google.com/android/reference/com/google/android/gms/ads/AdRequest
        // and https://developers.google.com/admob/ios/api/reference/Enums/GADErrorCode
        // for a list of possible codes.
        int code = loadAdError.GetCode();
        Debug.LogError(code); 

        // Gets an error message.
        // For example "Account not approved yet". See
        // https://support.google.com/admob/answer/9905175 for explanations of
        // common errors.
        string message = loadAdError.GetMessage();
        Debug.LogError(message); 

        // Gets the cause of the error, if available.
        AdError underlyingError = loadAdError.GetCause();

        // All of this information is available via the error's toString() method.
        Debug.LogError("Load error string: " + loadAdError.ToString());

        // Get response information, which may include results of mediation requests.
        ResponseInfo responseInfo = loadAdError.GetResponseInfo();
        Debug.LogError("Response info: " + responseInfo.ToString());
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