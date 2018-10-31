﻿using UnityEngine;
using System.Collections.Generic;
//Edit StefAN
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;

namespace Pokega{
	
	[System.Serializable]
	public class AchievementMaster : MonoBehaviour {
	
		public List<Achievement> achievements;
        public bool gameCenterAuthenticated;
	
		void Awake(){

			//Adds all achievements to the list
			achievements = new List<Achievement>();
			Achievement[] achs = gameObject.GetComponentsInChildren<Achievement>();
			for(int i=0; i<this.gameObject.transform.childCount; i++)
				achievements.Add(achs[i]);
		}

        void Start()
        {
            // Authenticate and register a ProcessAuthentication callback
            // This call needs to be made before we can proceed to other calls in the Social API
            Social.localUser.Authenticate(ProcessAuthentication);
        }

        // This function gets called when Authenticate completes
        // Note that if the operation is successful, Social.localUser will contain data from the server. 
        void ProcessAuthentication(bool success)
        {
            if (success)
            {
                Debug.Log("Authenticated, checking achievements");

                // Request loaded achievements, and register a callback for processing them
                Social.LoadAchievements(ProcessLoadedAchievements);
            }
            else
                Debug.Log("Failed to authenticate");
        }

        // This function gets called when the LoadAchievement call completes
        void ProcessLoadedAchievements(IAchievement[] achievements)
        {
            if (achievements.Length == 0)
                Debug.Log("Error: no achievements found");
            else
                Debug.Log("Got " + achievements.Length + " achievements");
        }

        public void SetAchievementGC(string achievementId){

            // You can also call into the functions like this
            Social.ReportProgress(achievementId, 100.0, result => {
                if (result)
                    Debug.Log("Successfully reported achievement progress");
                else
                    Debug.Log("Failed to report achievement");
            });
        }

        public void ShowLeaderboardsGC(){
            Social.ShowLeaderboardUI();
        }

        public void setAchievementGP(string achievemenId){
        	
            Social.ReportProgress(achievemenId, 100.0f, (bool success) => {
				// handle success or failure
			});
            Debug.Log("Achievement " + achievemenId + " set on GooglePlay");
        		
        }


    }
}