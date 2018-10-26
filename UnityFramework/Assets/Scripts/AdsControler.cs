using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;


public class AdsControler : MonoBehaviour
{
    public bool adsEnabled = true;
    public bool interstitialEnabled;
    public bool rewardedEnabled;

    public List<UnityEvent> rewardedEvents;


    public void Start()
    {

    }


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

    private void RewardedShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                foreach (UnityEvent e in rewardedEvents)
                    e.Invoke();
                //Nagradtiti igraca

                break;
            case ShowResult.Skipped:
                //TODO: Popup
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                //todo: popup
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

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


}

