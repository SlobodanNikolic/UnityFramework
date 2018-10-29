using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responcible for saving and loading data to PlayerPrefs
/// </summary>
public class LocalDBControler : MonoBehaviour {

    public string uid;
    public string name;
    public string email;
    public string fbid;
    public string ads;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save(){
        PlayerPrefs.SetString("uid", App.player.uid);
        PlayerPrefs.SetString("name", App.player.name);
        PlayerPrefs.SetString("email", App.player.email);
        PlayerPrefs.SetString("fbid", App.player.fbid);
        PlayerPrefs.SetString("ads", App.player.ads.ToString());

    }

    public void Load(){
        uid = PlayerPrefs.GetString("uid", "");
        name = PlayerPrefs.GetString("name", "");
        email = PlayerPrefs.GetString("email", "");
        fbid = PlayerPrefs.GetString("fbid", "");
        ads = PlayerPrefs.GetString("ads", "");

    }

}
