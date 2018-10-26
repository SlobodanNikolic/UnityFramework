using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControler : MonoBehaviour {

    public GameObject currentScreen;
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

    public GameObject getObjectByName(string name, List<GameObject> where){
        foreach(GameObject obj in where){
            if (obj.name == name)
                return obj;
        }
        return null;
    }

    public void SetScreen(string screenName){
        currentScreen.SetActive(false);
        currentScreen = getObjectByName(screenName, screens);
        currentScreen.SetActive(true);
    }

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
