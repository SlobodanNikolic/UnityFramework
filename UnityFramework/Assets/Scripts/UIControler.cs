using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa koja kontrolise prikaz skrinova i popupova. Po jedan skrin i popup mogu biti aktivni
/// u datom trenutku. Koristi Unity-jev UI sistem, ali se moze upotrebiti u bilo kom frameworku,
/// jer se zasniva samo na paljenju i gasenju objekata.
/// </summary>
public class UIControler : MonoBehaviour {

    /// <summary>
    /// GameObject koji predstavlja trenutni screen
    /// </summary>
    public GameObject currentScreen;
    /// <summary>
    /// GameObject koji predstavlja trenutni popup
    /// </summary>
    public GameObject currentPopUp;

    public List<GameObject> screens;
    public List<GameObject> popUps;

	// Use this for initialization
	void Start () {
        currentScreen = screens[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Pronalazi objekat po imenu u listi Game Objekata
    /// </summary>
    public GameObject getObjectByName(string name, List<GameObject> where){
        foreach(GameObject obj in where){
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    /// <summary>
    /// Funkcija koja postavlja skrin zadat po imenu.
    /// </summary>
    public void SetScreen(string screenName){
        currentScreen.SetActive(false);
        currentScreen = getObjectByName(screenName, screens);
        currentScreen.SetActive(true);
    }

    /// <summary>
    /// Funkcija koja postavlja popup po imenu.
    /// Ukoliko je activeState == false, funkcija samo gasi trenutni popup
    /// </summary>
    public void SetPopUp(string screenName, bool activeState = true)
    {
        if(currentPopUp!=null)
            currentPopUp.SetActive(false);

        if (activeState){
            currentPopUp = getObjectByName(screenName, popUps);
            currentPopUp.SetActive(activeState);
        }
        else{
            currentPopUp.SetActive(activeState);
            currentPopUp = null;
        }


    }
}
