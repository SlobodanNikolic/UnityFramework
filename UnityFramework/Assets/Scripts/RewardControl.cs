using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardControl : MonoBehaviour {


	public GameObject rewardButton;
	public int rewardAmount;
	public int rewardCounter = 0;
	public int startRewardAmount;
	public static RewardControl instance;

	void Awake(){
		if(instance==null){
		
	        instance = this;
		}
	}

	void Start () {
		if (PlayerPrefs.GetString ("timer") == "")
		{

            /// <summary>
            /// Treba odluciti kada ce da se prvi put pozove enable button,
            ///da li odmah po ulasku u igricu ili ako je broj odigranih partija > 0
            /// (bolje tako, po mom misljenju)
            /// </summary>
            Debug.Log ("==> Enabling button");
			enableButton ();
			
		} else 
		{
			disableButton ();
			StartCoroutine ("CheckTime");
		}
	}
	
	// Update is called once per frame


	public void CheckRewardCondition(){
		
	}

   


	public void Collect(){

		//GiveReward and play sound
	}


    /// <summary>
    /// Funkcija koja se poziva kada se klikne na nagradu i otvori se skrin
    /// na kome pise kolika bi nagrada trebala da bude
    /// </summary>
    public void OpenRewardScreen(){

		//Check how much the reward should be
		DateTime lastCollectionDate = DateTime.MinValue;
		if (PlayerPrefs.GetString ("collectionDate", "") != "") {
			lastCollectionDate = DateTime.Parse(PlayerPrefs.GetString ("collectionDate", DateTime.MinValue.ToString()));
		}
		rewardCounter = int.Parse(PlayerPrefs.GetString ("rewardCounter", "0"));
		DateTime collectionDate = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
			DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

		if (rewardCounter == 0) {
			rewardCounter++;
			PlayerPrefs.SetString ("collectionDate", collectionDate.ToString());
			PlayerPrefs.SetString ("rewardCounter", rewardCounter.ToString());
			rewardAmount = startRewardAmount;
			//Izbaciti korisniku na skrin rewardAmount (koliko je dobio)

			return;
		} else {
			
			if ((collectionDate - lastCollectionDate).Days > 1 || rewardCounter == 7) {
				rewardCounter = 0;
				PlayerPrefs.SetString ("collectionDate", collectionDate.ToString ());
				PlayerPrefs.SetString ("rewardCounter", rewardCounter.ToString ());
				rewardAmount = startRewardAmount;
                //Izbaciti korisniku na skrin rewardAmount (koliko je dobio)
                
				return;
			} else {
				rewardCounter++;
				PlayerPrefs.SetString ("collectionDate", collectionDate.ToString());
				PlayerPrefs.SetString ("rewardCounter", rewardCounter.ToString());
				rewardAmount = rewardCounter*rewardAmount;
                //Izbaciti korisiku na skrin rewardAmount (koliko je dobio)

			}
        }

	}

   
	//REWARD PART

	//TIME ELEMENTS
	public int hours; //to set the hours
	public int minutes; //to set the minutes
	public int seconds; //to set the seconds
	public bool timerComplete = false;
	public bool timerIsReady;
	public TimeSpan startTime;
	public TimeSpan endTime;
	public TimeSpan remainingTime;
	//progress filler
	public float value = 1f;
	//reward to claim
	public int RewardToEarn;




	//update the time information with what we got from the internet
	public void updateTime()
	{
		if (PlayerPrefs.GetString ("timer") == "Standby") {
			Debug.Log ("Timer on stand by, setting time and date to " + TimeManager.sharedInstance.getCurrentTimeNow () + 
				"-----" + TimeManager.sharedInstance.getCurrentDateNow().ToString());
			PlayerPrefs.SetString ("timer", TimeManager.sharedInstance.getCurrentTimeNow ());
			PlayerPrefs.SetInt ("date", TimeManager.sharedInstance.getCurrentDateNow());
		}
		else if (PlayerPrefs.GetString ("timer") != "" && PlayerPrefs.GetString ("timer") != "Standby"){
			int old = PlayerPrefs.GetInt("date");
			int now = TimeManager.sharedInstance.getCurrentDateNow();

			Debug.Log ("Timer not on standby nor empty");
			Debug.Log ("Old date: " + old.ToString());
			Debug.Log ("New date: " + now.ToString());


			//check if a day as passed
			if(now > old)
			{//day as passed
				Debug.Log("Day has passed");
				enableButton ();
				return;
			}else if (now == old)
			{//same day
				Debug.Log("Same Day - configuring now");
				configTimerSettings();
				return;
			}else
			{
				Debug.Log("error with date");
				return;
			}
		}
		Debug.Log("Day had passed - configuring now");
		configTimerSettings();
	}

	//setting up and configureing the values
	//update the time information with what we got from the internet
	public void configTimerSettings()
	{
		Debug.Log ("PlayerPrefs timer: start time: " + PlayerPrefs.GetString ("timer"));

		startTime = TimeSpan.Parse (PlayerPrefs.GetString ("timer"));
		Debug.Log ("End time from rewardControl: " + hours + ":" + minutes + ":" + seconds);
		endTime = TimeSpan.Parse (hours + ":" + minutes + ":" + seconds);
		TimeSpan temp = TimeSpan.Parse (TimeManager.sharedInstance.getCurrentTimeNow ());
		Debug.Log ("TimeSpan temp: " + temp.ToString());
		TimeSpan diff = temp.Subtract (startTime);
		remainingTime = endTime.Subtract (diff);
		

		if(diff >= endTime)
		{
			timerComplete = true;
			enableButton ();
		}else
		{
			timerComplete = false;
			disableButton();
			timerIsReady = true;
		}
	}
   
	//enable button function
	public void enableButton()
	{
		rewardButton.SetActive (true);
	}


	//disable button function
	public void disableButton()
	{
		rewardButton.SetActive (false);
	}


	//use to check the current time before compleating any task. use this to validate
	public IEnumerator CheckTime()
	{
		disableButton ();
		Debug.Log ("==> Checking for new time");
		yield return StartCoroutine (
			TimeManager.sharedInstance.getTime()
		);
		updateTime ();
		Debug.Log ("==> Time check complete!");

	}


	//trggered on button click
	public void rewardClicked()
	{
		Debug.Log ("==> Claim Button Clicked");
		PlayerPrefs.SetString ("timer", "Standby");
		StartCoroutine ("CheckTime");
	}


	void OnApplicationFocus(bool focusStatus){
		if (focusStatus) {
			if (PlayerPrefs.GetString ("timer") == "") {
				if (App.player.timesPlayed > 0) {
					Debug.Log ("==> Enabling button");
					enableButton ();
				}
			} else {
				StartCoroutine ("CheckTime");
			}
		}
	}


	//update method to make the progress tick
	void Update()
	{
		if(timerIsReady)
		{
			if (!timerComplete && PlayerPrefs.GetString ("timer") != "")
			{
				value -= Time.deltaTime * 1f / (float)endTime.TotalSeconds;

				//this is called once only
				if (value <= 0 && !timerComplete) {
					//when the timer hits 0, let do a quick validation to make sure no speed hacks.
					validateTime ();
					timerComplete = true;
				}
			}
		}
			
	}
    

	//validator
	public void validateTime()
	{
		Debug.Log ("==> Validating time to make sure no speed hack!");
		StartCoroutine ("CheckTime");
	}


	public void claimReward(int x)
	{
		Debug.Log ("YOU EARN "+ x +" REWARDS");
	}
}
