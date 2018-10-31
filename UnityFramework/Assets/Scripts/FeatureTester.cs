using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa koju koristimo za testiranje svih feature-a frameworka
/// </summary>
public class FeatureTester : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            Debug.Log(App.player.ToString());
    }

    /// <summary>
    /// Otvaramo shop ui
    /// </summary>
    public void UIShop()
    {
        App.ui.SetScreen("UIShop");
    }

    /// <summary>
    /// Otvaramo jedan popup
    /// </summary>
    public void UIPopUp()
    {
        App.ui.SetPopUp("UIPopUp");
    }

    /// <summary>
    /// Facebook povezivanje
    /// </summary>
    public void FBLogin()
    {
        App.fb.Login();
    }

    /// <summary>
    /// Prikazivanje reklama
    /// </summary>
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

    /// <summary>
    /// Funkcija koja bi trebalo da se izvrsi kada korisnik odgleda rewarded reklamu do kraja
    /// </summary>
    public void RewardPlayer()
    {
        Debug.Log("Player rewarded");
    }

    /// <summary>
    /// Kupovina jedne consumable stavke (kao npr. bag of coins)
    /// </summary>
    public void BuyConsumable()
    {
        App.purchaser.BuyConsumableByIndex(0);
    }

    /// <summary>
    /// Npr. no ads
    /// </summary>
    public void BuyNonConsumable()
    {
        App.purchaser.BuyNonConsumableByIndex(0);
    }

    /// <summary>
    /// Funkcija koja treba da se izvrsi nakon kupovine consumable
    /// </summary>
    public void ConsumableBought()
    {
        Debug.Log("Consumable bought succesfuly!");
    }

    public void NonConsumableBought()
    {
        Debug.Log("Ads removed succesfuly!");
    }

    /// <summary>
    /// Postavlja achievement sa ovim id-jem na Google Play i Game Center
    /// </summary>
    public void SetAchievement()
    {
        App.social.SetAchievement("CgkIyN6tnc4CEAIQAQ");

    }

    /// <summary>
    /// Prijavljujemo score na google play i game center leaderboard
    /// </summary>
    public void ReportScore(){
        App.score.ReportScore(App.score.GetScore("points"), "CgkIyN6tnc4CEAIQAg");
    }

    /// <summary>
    /// Dodajemo 1 na score pod nazivom points
    /// </summary>
    public void ScorePlus(){
        App.score.ScorePlus(1, "points");
    }

    public void ScoreMinus()
    {
        App.score.ScoreMinus(1, "points");
    }

    /// <summary>
    /// Svi skorovi koji su veci od trenutnih najboljih skorova se pamte na serveru
    /// </summary>
    public void SetBestScore(){
        App.score.SetAndSaveBestScores();
    }

    public void Share(){
        App.social.ShareScreenshot("Great game!", "Gotta play this one, try it! " + "https://www.makonda.com");
    }


}
