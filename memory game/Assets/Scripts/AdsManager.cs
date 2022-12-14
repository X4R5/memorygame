using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Common;
using GoogleMobileAds.Api;
using TMPro;

public class AdsManager : MonoBehaviour
{
    RewardedAd rewardedAd;
    public static AdsManager Instance;
    public string rewardedId = "";
    bool give;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Heart") == 0) PlayerPrefs.SetInt("Heart", 3);
        give = true;
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
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " aciliyor";
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " gosteremiyor" + e.AdError.GetMessage();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " ödül";
        if (give)
        {
            PlayerPrefs.SetInt("Heart", 3);
            Debug.Log(PlayerPrefs.GetInt("Heart"));
        }
    }

    public void LoadRewardedAd(bool a)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " fonksiyon";
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " yükleniyor";
        }
        else
        {
            GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " yüklenemedi";
        }
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " yüklendi";
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        GameObject.Find("s").transform.GetChild(0).GetComponent<TMP_Text>().text += " başarısız";
    }
}
