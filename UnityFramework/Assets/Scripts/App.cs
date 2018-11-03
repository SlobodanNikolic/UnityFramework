using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokega;

/// <summary>
/// Klasa koja sadrzi staticke reference ka svim bitnim komponentama
/// koje igra koristi. Preko ove klase pristupamo svim ostalim komponentama frameworka
/// </summary>
public class App : MonoBehaviour {

    public static Player player;
    public static LocalDBControler localDB;
    public static FirebaseControler firebase;
    public static UIControler ui;
    public static Purchaser purchaser;
    public static FacebookControler fb;
    public static AdsControler ads;
    public static SocialControl social;
    public static ScoreControl score;
    public static RewardControl gift;

    /// <summary>
    /// Instancira se prazan Player objekat i dodeljuju reference za sve komponente
    /// </summary>
    void Awake()
    {
        player = new Player();
        localDB = GameObject.Find("LocalDBControler").GetComponent<LocalDBControler>();
        firebase = GameObject.Find("FirebaseControler").GetComponent<FirebaseControler>();
        ui = GameObject.Find("UIControler").GetComponent<UIControler>();
        fb = GameObject.Find("FacebookControler").GetComponent<FacebookControler>();
        ads = GameObject.Find("AdsControler").GetComponent<AdsControler>();
        purchaser = GameObject.Find("Purchaser").GetComponent<Purchaser>();
        social = GameObject.Find("SocialControler").GetComponent<SocialControl>();
        score = GameObject.Find("ScoreControler").GetComponent<ScoreControl>();
        //gift = GameObject.Find("RewardControler").GetComponent<RewardControl>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

     
    }
}
