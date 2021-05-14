using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    private string appID = "ca-app-pub-6351610926806883~8466502596";

    // test ids for every ad, change origional when publishing to play store

// **************Banner Ad Id & var****************
    private BannerView bannerView;
    private string bannerID = "ca-app-pub-6351610926806883/5126857538";//ca-app-pub-6351610926806883/5126857538

// **************Interstitial Ad Id & var****************
    private InterstitialAd fullscreenAd;
    private string fullscreenAdID = "ca-app-pub-6351610926806883/9006376886";//ca-app-pub-6351610926806883/9006376886

// **************Reward Ad Id & var ****************
    private RewardBasedVideoAd rewardedAd;
    private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";

// *****************************************************************
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Start() {
        RequestFullScreenAd();
        RequestBanner();

        rewardedAd = RewardBasedVideoAd.Instance;
        RequestRewardedAd();

        //********** Events*********
        rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;

        rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFaildToLoad;

        rewardedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;

        rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;


    }

// ************************Banner Ad********************
    public void RequestBanner(){
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        //we make an empty request sothat we can load our banner
        AdRequest request = new AdRequest.Builder().Build();

        // after above line we load our ad in empty request
        bannerView.LoadAd(request);

        bannerView.Show();
    }

    // for Hiding in the game lvl
    public void HideBanner(){
        bannerView.Hide();
    }

// ********************Interstitial Ad********************
    public void RequestFullScreenAd(){
        fullscreenAd = new InterstitialAd(fullscreenAdID);

        AdRequest request = new AdRequest.Builder().Build();

        fullscreenAd.LoadAd(request);
    }

    // for interstitial we make another fn and check that ad is loaded or not
    public void ShowFullScreenAd(){
        if(fullscreenAd.IsLoaded()){
            fullscreenAd.Show();
        }else{
            RequestFullScreenAd();
        }
    }

// ***********************Reward Ad***********************
    public void RequestRewardedAd(){
        AdRequest request = new AdRequest.Builder().Build();

        rewardedAd.LoadAd(request, rewardedAdID);
    }

    public void ShowRewardedAd(){
        if(rewardedAd.IsLoaded()){
            rewardedAd.Show();
        }else{
            RequestRewardedAd();
        }
    }

// ************************************************************************************
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Rewarded Video ad loaded successfully");
    }
    public void HandleRewardBasedVideoFaildToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load rewarded video ad :" + args.Message);
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("You have been rewarded with " + amount.ToString() + " " + type);
    }
    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Rewarded Video has closed");
        RequestRewardedAd();
    }


}
