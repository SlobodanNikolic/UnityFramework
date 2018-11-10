using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class, with all the data that needs to be saved
/// </summary>
public class Player{

    public string uid;
    public string name;
    public string email;
    public string fbid;
    public string gender;
    public bool ads;
    public bool sound;
    public int timesPlayed;
    public List<string> bestScoreNames;
    public List<string> bestScoreValues;

    public Player()
    {
        uid = "";
        name = "";
        email = "";
        fbid = "";
        gender = "";
        ads = true;
        sound = true;
        timesPlayed = 0;
        bestScoreNames = new List<string>();
        bestScoreValues = new List<string>();

    }

    public Player(string fbid){
        this.fbid = fbid;
        uid = "";
        name = "";
        email = "";
        gender = "";
        ads = true;
        sound = true;
        timesPlayed = 0;
        bestScoreNames = new List<string>();
        bestScoreValues = new List<string>();

    }


    public string ToString(){
        string print = "Uid: " + uid + "; " +
            "Name: " + name + "; " +
            "Email: " + email + "; " +
            "Fbid: " + fbid + "; " +
            "Gender: " + gender + "; " +
            "Ads: " + ads + "; " +
            "Sound: " + sound + "; " +
            "TimesPlayed: " + timesPlayed + "; " + "Best scores: ";

        for (int i = 0; i < bestScoreNames.Count; i++){
            print += bestScoreNames[i] + ": " + Crypting.DecryptInt (bestScoreValues[i]) + ";";
        }

        return print;
    }

}
