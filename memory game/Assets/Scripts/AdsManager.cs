using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    RewardedAd rewardedAd;
    public static AdsManager Instance;
    public string rewardedId = "";

    private void Awake()
    {
        //singleton method
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Debug.Log("hc " + PlayerPrefs.GetInt("Heart"));
        //MobileAds.Initialize(initStatus => { });
        rewardedAd = new RewardedAd(rewardedId);
        rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
    }

    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        PlayerPrefs.SetInt("Heart", 3);
        Debug.Log("odul verildi");
        var b = PlayerPrefs.GetInt("Heart");
        Debug.Log(b);
    }

    public void LoadRewardedAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }
}
