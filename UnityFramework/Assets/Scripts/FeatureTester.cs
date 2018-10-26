using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UIShop(){
        App.ui.SetScreen("UIShop");
    }

    public void UIPopUp()
    {
        App.ui.SetPopUp("UIPopUp");
    }

    public void FBLogin(){
        App.fb.Login();
    }

}
