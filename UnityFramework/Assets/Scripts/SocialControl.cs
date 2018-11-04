using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokega;
using UnityEngine.SocialPlatforms;
using System.IO;
using GooglePlayGames;

/// <summary>
/// Kontroler koji je zaduzen za Google Play i Game Center Achievements, Leaderboards,
/// kao i za Native Share funkcionalnost.
/// </summary>
public class SocialControl : MonoBehaviour {

    public bool gameCenterAuthenticated;

    void Awake()
    {

    }

    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
    }

    public void Authenticate(){
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            Debug.Log("Social: Authenticated, checking achievements");

            // Request loaded achievements, and register a callback for processing them
            Social.LoadAchievements(ProcessLoadedAchievements);
        }
        else
            Debug.Log("Social : Failed to authenticate");
    }

    // This function gets called when the LoadAchievement call completes
    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");
    }

    /// <summary>
    /// Funkcija koja postavlja achievement sa id-jem achievementId na GP i GC
    /// </summary>
    public void SetAchievement(string achievementId)
    {

        // You can also call into the functions like this
        Social.ReportProgress(achievementId, 100.0, result => {
            if (result)
                Debug.Log("Successfully reported achievement progress");
            else
                Debug.Log("Failed to report achievement");
        });
    }


    /// <summary>
    /// Funkcija za prikazivanje leaderboarda
    /// </summary>
    public void ShowLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }

    /// <summary>
    /// Funkcija za prikazivanje achievementa
    /// </summary>
    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    /// <summary>
    /// Funkcija koja pokrece korutinu pomocu koje se shareuje screenshot sa odgovarajucom porukom
    /// </summary>
    public void ShareScreenshot(string subject, string text){
        StartCoroutine(TakeSSAndShare(subject, text));
    }

    /// <summary>
    /// Korutina koja uzima trenutni screenshot i shareuje ga nativno
    /// </summary>
    private IEnumerator TakeSSAndShare(string subject, string text)
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "sharedimg.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).SetSubject(subject).SetText(text).Share();

    }


}
