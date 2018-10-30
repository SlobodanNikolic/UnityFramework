using UnityEngine;
using System.Collections;
//using GameAnalyticsSDK;
using System.Collections.Generic;
//using Facebook.MiniJSON;
//Edit StefAN
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Pokega{

	public class ScoreControl : MonoBehaviour {

		//Lista skorova
		public List<Score> scores;
		public List<Score> bestScores;	
		public List<Score> totalScores;	

		void Awake(){
			InitializeScoresToZero();
			InitializeBestScoresToZero();
		}
 
		public void InitializeScoresToZero(){
			foreach(Score s in scores) {
				s.score = Crypting.EncryptInt2(0);
			}
		}
			
		public void InitializeBestScoresToZero(){
			for(int i=0; i<bestScores.Count; i++) {
				bestScores[i].score = Crypting.EncryptInt2(0);
				bestScores[i].scoreName = scores[i].scoreName;
			}
		}

		
		public void SetAndSaveBestScore() {
			Debug.Log("Checking and setting best scores");
			for (int i=0; i<scores.Count; i++) 
			{
				if(Crypting.DecryptInt2 (scores[i].score) > Crypting.DecryptInt2 (bestScores[i].score))
				{
					bestScores[i].score = scores[i].score;
                    foreach(Text label in bestScores[i].scoreLabels){
                        label.text = (Crypting.DecryptInt2(bestScores[i].score)).ToString();
                    }
				}
			}
			//OVDE TREBA PRVO DA SE POSALJU SKOROVI LOCAL DB-u i SERVER API-ju
            //todo: Save na server i lokal
		}

		//Postavljanje vrednosti skora u odgovarajuce labele
		public void SetScoreLabels(){
			foreach(Score sc in scores){
				foreach(Text labela in sc.scoreLabels){
					labela.text = GetScore(sc.scoreName).ToString();
				}
			}
		}

        //Postavljanje vrednosti najboljih skorova u odgovarajuce labele

        public void SetBestScoreLabels()
        {
            foreach (Score sc in bestScores)
            {
                foreach (Text labela in sc.scoreLabels)
                {
                    labela.text = GetBestScore(sc.scoreName).ToString();
                }
            }
        }


		public void ScorePlus(int amount, string name){
			
			for(int i=0; i<scores.Count; i++){
				if(scores[i].scoreName.CompareTo(name)==0){
					scores[i].score = Crypting.EncryptInt2(Crypting.DecryptInt2(scores[i].score) + amount);
					foreach(Text labela in scores[i].scoreLabels)
						labela.text = GetScore(scores[i].scoreName).ToString();
				}
			}

		}

		public void ScoreMinus(int amount, string name){
		
			for(int i=0; i<scores.Count; i++){
				if(scores[i].scoreName.CompareTo(name)==0) {
					scores[i].score = Crypting.EncryptInt2(Crypting.DecryptInt2(scores[i].score) - amount);
					foreach(Text labela in scores[i].scoreLabels)
						labela.text = GetScore(scores[i].scoreName).ToString();
				}
			}
		}
		
		
		//Geteri i Seteri vracaju/postavljaju skor sa odgovarajucim imenom za na odgovarajucu vrednost "amount"
		public int GetScore(string name){
			for(int i=0; i<scores.Count; i++){
				if(scores[i].scoreName.CompareTo(name)==0)
				    return Crypting.DecryptInt2(scores[i].score);
			}
			return -1;
		}

		public void SetScore(int amount, string name){
			for(int i=0; i<scores.Count; i++){
				if(scores[i].scoreName.CompareTo(name)==0) 
                {
                    scores[i].score = Crypting.EncryptInt2(amount);
                    foreach (Text labela in scores[i].scoreLabels)
                        labela.text =amount.ToString();
                }
            }
		}


		public int GetBestScore(string name){
			for(int i=0; i<bestScores.Count; i++)
				if(bestScores[i].scoreName.CompareTo(name)==0) 
				    return Crypting.DecryptInt2(bestScores[i].score);
			return -1;
		}

		public void SetBestScores(List<Score> bs){
			for(int i = 0; i<bestScores.Count; i++){ 
				bestScores[i].score = bs[i].score;
				Debug.Log("Set best scores");
				Debug.Log(bestScores[i].scoreName + " " + bestScores[i].score);
			}
		}

        public void ReportScore(int score, string leaderboardID)
        {
            Debug.Log("Reporting score " + (long)score + " on leaderboard " + leaderboardID);
            Social.ReportScore(score, leaderboardID, success => {
                Debug.Log(success ? "Reported score successfully" : "Failed to report score");
            });
        }

    }
}