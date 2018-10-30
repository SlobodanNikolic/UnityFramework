﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureTester : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UIShop()
    {
        App.ui.SetScreen("UIShop");
    }

    public void UIPopUp()
    {
        App.ui.SetPopUp("UIPopUp");
    }

    public void FBLogin()
    {
        App.fb.Login();
    }

    public void ShowInterstitial()
    {
        App.ads.ShowUnityAdsInterstitial();
    }

    public void ShowVideo()
    {
        App.ads.ShowUnityAdsSkippableVideo();
    }

    public void ShowRewarded()
    {
        App.ads.ShowUnityAdsRewarded();
    }

    public void RewardPlayer()
    {
        Debug.Log("Player rewarded");
    }

    public void BuyConsumable()
    {
        App.purchaser.BuyConsumableByIndex(0);
    }

    public void BuyNonConsumable()
    {
        App.purchaser.BuyNonConsumableByIndex(0);
    }

    public void ConsumableBought()
    {
        Debug.Log("Consumable bought succesfuly!");
    }

    public void NonConsumableBought()
    {
        Debug.Log("Ads removed succesfuly!");
    }

    public void SetAchievement()
    {
        App.social.SetAchievement("CgkIyN6tnc4CEAIQAQ");

    }

    public void ReportScore(){
        App.score.ReportScore(App.score.GetScore("points"), "CgkIyN6tnc4CEAIQAg");
    }

    public void ScorePlus(){
        App.score.ScorePlus(1, "points");
    }

    public void ScoreMinus()
    {
        App.score.ScoreMinus(1, "points");
    }

    public void SetBestScore(){
        App.score.SetAndSaveBestScore();
    }

}
