using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

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


    public void Start()
    {

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


}

