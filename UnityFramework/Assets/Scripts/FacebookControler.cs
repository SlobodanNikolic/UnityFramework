using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Include Facebook namespace
using Facebook.Unity;
using System;

public class FacebookControler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


   

    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Facebook is initialized");

            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    public void Login(){
        Debug.Log("Facebook login clicked");

        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }


    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is logged in");
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);

            CheckFbid(aToken);

            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void CheckFbid(AccessToken token){
        //string localFbid = PlayerPrefs.GetString("fbid", "");
        //if(localFbid == ""){
        //    //Nema fbid u player prefs
        //    //Napravi novog usera sa sign in with credentials, proveri da li postoji fbid u bazi
        //    //Ako postoji, ucitaj podatke sa njega
        //    //Ako ne postoji, napravi novog usera u bazi i ubaci fbid u podatke

        //}
        //else{
        //    if(localFbid == token.UserId){
        //        //To je isti user koji postoji u lokalu, samo ucitaj podatke iz zapisa sa tim fbid

        //    }
        //    else{
        //        //Ako je razlicit facebook, 
        //    }
        //    App.player.fbid = token.UserId;
        //    FB.API("/me?fields=id,first_name,last_name,gender,email", HttpMethod.GET, GraphCallback);
        //    App.firebase.ConnectWithFacebook(token.TokenString);
        //}


        FB.API("/me?fields=id,first_name,last_name,gender,email", HttpMethod.GET, GraphCallback);
        App.firebase.ConnectWithFacebook(token);
    }

    private void GraphCallback(IGraphResult result)
    {
        Debug.Log("Facebook info callback : " + result.RawResult);
        string id;
        string firstname;
        string lastname;
        string gender;
        string email;
        App.player.name = "";
        App.player.gender = "";

        if (result.ResultDictionary.TryGetValue("first_name", out firstname))
        {
            App.player.name += firstname;
        }
        if (result.ResultDictionary.TryGetValue("last_name", out lastname))
        {
            App.player.name += " " + lastname;
        }
        if (result.ResultDictionary.TryGetValue("gender", out gender))
        {
            App.player.gender = gender;
        }
        if (result.ResultDictionary.TryGetValue("email", out email))
        {
            App.player.email = email;
        }

        App.firebase.Save();
    }


    public void Share(String uri){
        //uri should be like "https://developers.facebook.com/"
        Debug.Log("Facebook share clicked");
        FB.ShareLink(
            new Uri(uri),
            callback: ShareCallback
        );
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }

}
