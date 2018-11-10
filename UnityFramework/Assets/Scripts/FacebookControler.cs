using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Include Facebook namespace
using Facebook.Unity;
using System;

/// <summary>
/// Klasa zaduzena za facebook login i autentifikaciju
/// </summary>
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
        /// <summary>
        /// Inicijalizacija facebooka
        /// </summary>
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

    /// <summary>
    /// Funkcija koju treba pozvati pri kliku na facebook dugme
    /// </summary>
    public void Login(){
        Debug.Log("Facebook login clicked");

        /// <summary>
        /// Odredjujemo permisije i pozivamo login funkciju
        /// </summary>
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }

    /// <summary>
    /// Kolbek koji se aktivira pri loginu
    /// </summary>
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is logged in");
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            App.player.fbid = aToken.UserId;

            /// <summary>
            /// Pozivamo funkciju koja moze da proveri jos nesto u vezi fbid-ja, ako je potrebno i pozove ConnectWithFacebook
            /// funkciju u FirebaseControler-u
            /// </summary>
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
   
        /// <summary>
        /// Pozivamo fb API da bismo uzeli podatke korisnika koji se logovao
        /// </summary>
        FB.API("/me?fields=id,first_name,last_name,gender,email", HttpMethod.GET, GraphCallback);
        App.firebase.ConnectWithFacebook(token);
    }

    /// <summary>
    /// Uzimamo podatke za logovanog korisnika
    /// </summary>
    private void GraphCallback(IGraphResult result)
    {
        Debug.Log("Facebook info callback : " + result.RawResult);
        string id;
        string firstname;
        string lastname;
        string gender;
        string email;
        string fbid;
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


    /// <summary>
    /// Ovu funkciju mozemo koristiti ukoliko zelimo eksplicitni facebook share
    /// </summary>
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
