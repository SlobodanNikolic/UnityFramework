using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using GoogleMobileAds.Api;


/// <summary>
/// Klasa koja kontrolise UnityAds servis i prikazivanje reklama
/// </summary>
public class AdsControler : MonoBehaviour
{
    /// <summary>
    /// Bool promenljive koje opisuju koje se reklame koriste i da li se koriste
    /// </summary>
    public bool adsEnabled = true;
    public bool interstitialEnabled;
    public bool rewardedEnabled;

    /// <summary>
    /// Lista Eventova (funkcija) koji se pozivaju kada se odgleda rewarded reklama
    /// </summary>
    public List<UnityEvent> rewardedEvents;

    /// <summary>
    /// AdMob reklame - promenljive
    /// </summary>
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;

    public string AdMobInterIdiOS;
    public string AdMobInterIdAnd;
    public string AdMobRewardedIdiOS;
    public string AdMobRewardedIdAnd;

    public string AdMobInterIdiOSTEST = "ca-app-pub-3940256099942544/4411468910";
    public string AdMobInterIdAndTEST = "ca-app-pub-3940256099942544/1033173712";
    public string AdMobRewardedIdiOSTEST = "ca-app-pub-3940256099942544/1712485313";
    public string AdMobRewardedIdAndTEST = "ca-app-pub-3940256099942544/5224354917";

    public void Start()
    {
        /// <summary>
        /// Podesavanje za Admob
        /// </summary>
#if UNITY_ANDROID
            string appId = "ca-app-pub-3405361179188325~5469892951";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-3405361179188325~7905033343";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        this.RequestRewardBasedVideo();
    }

    /// <summary>
    /// Prikaz rewarded videa
    /// </summary>
    public void ShowUnityAdsRewarded()
    {
        if (adsEnabled && rewardedEnabled)
        {
            Debug.Log("Show rewarded");

            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions { resultCallback = RewardedShowResult };
                Advertisement.Show("rewardedVideo", options);
            }
        }
    }

    /// <summary>
    /// Prikaz interstitial reklame (ona koja moze da se skipuje)
    /// </summary>
    public void ShowUnityAdsInterstitial()
    {
        if (adsEnabled && interstitialEnabled)
        {
            Debug.Log("Show interstitial");
            if (Advertisement.IsReady("interstitial"))
            {
                var options = new ShowOptions { resultCallback = InterstitialShowResult };
                Advertisement.Show("interstitial", options);
            }
        }
    }

    /// <summary>
    /// Prikaz skippable videa
    /// </summary>
    public void ShowUnityAdsSkippableVideo()
    {
       
        if (adsEnabled && interstitialEnabled)
        {
            Debug.Log("Show video");

            if (Advertisement.IsReady("video"))
            {
                var options = new ShowOptions { resultCallback = VideoShowResult };
                Advertisement.Show("video", options);
            }
        }

    }

    /// <summary>
    /// Kolbek koji se poziva nakon sto se zatvori rewarded video
    /// </summary>
    private void RewardedShowResult(ShowResult result)
    {
        switch (result)
        {
            /// <summary>
            /// Odgledan do kraja
            /// </summary>
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                foreach (UnityEvent e in rewardedEvents)
                    e.Invoke();
                //Nagradtiti igraca

                break;
            /// <summary>
                /// Preskocen
            /// </summary>
            case ShowResult.Skipped:
                //TODO: Popup
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            /// <summary>
                /// Prikazivanje nije uspelo
            /// </summary>
            case ShowResult.Failed:
                //todo: popup
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    /// <summary>
    /// Kolbek koji se aktivira nakon prikaza interstial reklame
    /// </summary>
    private void InterstitialShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    /// <summary>
    /// Kolbek nakon zavrsetka video reklame
    /// </summary>
    private void VideoShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void RequestAdMobInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = AdMobInterIdAndTEST;
#elif UNITY_IPHONE
        string adUnitId = AdMobInterIdiOSTEST;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        Debug.Log("Interstitial requested");

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }


    public void ShowAdMobInterstitial(){
        Debug.Log("Show ad mob inter");
        if (this.interstitial.IsLoaded())
        {
            Debug.Log("Showing inter");
            this.interstitial.Show();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitial.Destroy();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
            string adUnitId = AdMobRewardedIdAndTEST;
#elif UNITY_IPHONE
        string adUnitId = AdMobRewardedIdiOSTEST;

#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }


    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        this.RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        Debug.Log("Rewarded video - reward player");
        foreach(UnityEvent e in rewardedEvents){
            e.Invoke();
        }
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    public void ShowAdMobRewarded(){
        Debug.Log("Show ad mob rewarded video");
        if (rewardBasedVideo.IsLoaded())
        {
            Debug.Log("Showing ad mob rewarded video");
            rewardBasedVideo.Show();
        }
    }
}

