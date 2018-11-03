using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class TimeManager : MonoBehaviour {
	/* 
	  	necessary variables to hold all the things we need.
		php url
		timedata, the data we get back
		current time
		current date
	*/ 

	public static TimeManager sharedInstance = null;
	public string url;
	public string timeData;
	public string currentTime;
	public string currentDate;
	public DateTime currDate;
	public int year, month, day, hour, min, sec;

	//make sure there is only one instance of this always.
	void Awake() {
		if (sharedInstance == null) {
			sharedInstance = this;
		}
        else if (sharedInstance != this) {
			Destroy (gameObject);  
		}
	}


	//time fether coroutine
	public IEnumerator getTime()
	{
		Debug.Log ("connecting to php");
		WWW www = new WWW (url);
		yield return www;
		Debug.Log ("got the php information");
		timeData = www.text;
		string[] words = timeData.Split('T');	
		//timerTestLabel.text = www.text;

		string pat = @"(\d+)-(\d+)-(\d+)T(\d+):(\d+):(\d+)\+";

		// Instantiate the regular expression object.
		Regex r = new Regex(pat, RegexOptions.IgnoreCase);

		// Match the regular expression pattern against a text string.
		Match m = r.Match(timeData);
		int matchCount = 0;
		if (m.Success) {
			//Date
			Debug.Log (m.Groups[1]);
			Debug.Log (m.Groups[2]);
			Debug.Log (m.Groups[3]);
			//Time
			Debug.Log (m.Groups[4]);
			Debug.Log (m.Groups[5]);
			Debug.Log (m.Groups[6]);

		}
			
		Debug.Log ("The date is : "+words[0]);
		Debug.Log ("The time is : "+words[1]);

		//setting current time
		currentDate = m.Groups[2]+ "-" + m.Groups[3]+ "-" + m.Groups[1];
		currentTime = m.Groups[4]+ ":" + m.Groups[5]+ ":" + m.Groups[6];

		year = int.Parse (m.Groups[1].ToString());
		month = int.Parse (m.Groups[2].ToString());
		day = int.Parse (m.Groups[3].ToString());
		hour = int.Parse (m.Groups[4].ToString());
		min = int.Parse (m.Groups[5].ToString());
		sec = int.Parse (m.Groups[6].ToString());

	}


	//get the current time at startup
	void Start()
	{
		//Debug.Log ("TimeManager script is Ready.");
		//StartCoroutine ("getTime");
	}

	//get the current date - also converting from string to int.
	//where 12-4-2017 is 1242017
	public int getCurrentDateNow()
	{
		string[] words = currentDate.Split('-');
		int x = int.Parse(words[0]+ words[1] + words[2]);
		return x;
	}


	//get the current Time
	public string getCurrentTimeNow()
	{
		return currentTime;
	}


}