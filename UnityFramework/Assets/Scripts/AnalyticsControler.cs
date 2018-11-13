using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Reference the Unity Analytics namespace
using UnityEngine.Analytics;

/// <summary>
/// Klasa preko koje se moze poslati custom event za Unity Analytics
/// </summary>
public class AnalyticsControler : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// Ova funkcija se zove sa custom argumentom eventData i name. U eventData smestiti
    /// sve podatke koji su nam zanimljivi za event
    /// </summary>
    public void CustomEvent(string name, IDictionary<string, object> eventData){
        AnalyticsEvent.Custom(name, eventData);
    }
}
