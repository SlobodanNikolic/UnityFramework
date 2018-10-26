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
    public string ads;

    public Player()
    {
        name = "";
        email = "";
        fbid = "";
        uid = "";
    }

    public Player(string fbid){
        this.fbid = fbid;
    }

}
