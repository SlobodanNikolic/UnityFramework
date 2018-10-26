using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {

    public static Player player;
    public static LocalDBControler localDB;
    public static FirebaseControler firebase;
    public static UIControler ui;
    //public static Purchaser purchaser;
    public static FacebookControler fb;

    void Awake()
    {
        player = new Player();
        localDB = GameObject.Find("LocalDBControler").GetComponent<LocalDBControler>();
        firebase = GameObject.Find("FirebaseControler").GetComponent<FirebaseControler>();
        ui = GameObject.Find("UIControler").GetComponent<UIControler>();
        fb = GameObject.Find("FacebookControler").GetComponent<FacebookControler>();
        //purchaser = GameObject.Find("Purchaser").GetComponent<Purchaser>();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.S))
        {
            firebase.Save();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            firebase.Load();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            ui.SetScreen("UIGame");
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            ui.SetPopUp("UIShop");
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            ui.SetPopUp("UIShop", false);
        }
    }
}
