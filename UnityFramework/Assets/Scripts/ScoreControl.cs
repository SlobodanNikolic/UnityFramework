using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


	public class ScoreControl : MonoBehaviour {

		//Lista skorova
		public List<Score> scores;
		public List<Score> bestScores;	

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

		
		public void SetAndSaveBestScores() {
			Debug.Log("Checking and setting best scores");
			for (int i=0; i<scores.Count; i++) 
			{
				if(Crypting.DecryptInt2 (scores[i].score) > Crypting.DecryptInt2 (bestScores[i].score))
				{
					bestScores[i].score = scores[i].score;
                    foreach(Text label in bestScores[i].scoreLabels){
                        label.text = (Crypting.DecryptInt2(bestScores[i].score)).ToString();
                    }
                    AddBestScoreToPlayer(bestScores[i].scoreName, bestScores[i].score);
				}
			}
            //todo: Save na server i lokal
            App.firebase.Save();
		}


        /// <summary>
        /// Pomocna funkcija koja dodaje skor pod imenom scoreName i vrednosti score, 
        /// player objektu. Pri pamcenju skorova na server, pre toga ih updateujemo 
        /// na player objektu.
        /// </summary>
        public void AddBestScoreToPlayer(string scoreName, string score){
            bool inserted = false;
            for (int i = 0; i < App.player.bestScoreNames.Count; i++){
                if (App.player.bestScoreNames[i] == scoreName)
                {
                    App.player.bestScoreValues[i] = score;
                    inserted = true;
                }
            }
            if(!inserted){
                App.player.bestScoreNames.Add(scoreName);
                App.player.bestScoreValues.Add(score);
            }
        }

        //Postavljanje vrednosti skora u odgovarajuce labele
        /// <summary>
        /// Ova funkcija treba da se zove svaki put kada se poveca skor u igri, kako bi se
        /// sve labele apdejtovale i prikazivale odgovarajuci skor
        /// </summary>
        public void SetScoreLabels(){
			foreach(Score sc in scores){
				foreach(Text labela in sc.scoreLabels){
					labela.text = GetScore(sc.scoreName).ToString();
				}
			}
		}


        //Postavljanje vrednosti najboljih skorova u odgovarajuce labele
        /// <summary>
        /// Ova funkcija se zove npr. u game overu, kako bi se apdejtovale
        /// labele sa najboljim skorovima.
        /// </summary>
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


        /// <summary>
        /// Funkcija dodaje amount vrednost na skor pod imenom name
        /// </summary>
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

        /// <summary>
        /// Funkcija koja upisuje najbolje skorove ucitane sa servera u ScoreControler.
        /// Kada ucitamo najbolje skorove sa servera, upisujemo ih u ScoreControler, jer tu 
        /// manipulisemo njima i podaci moraju da budu apdejtovani.
        /// </summary>
        public void SetBestScoresFromFirebase(List<string> bestScoreNames, List<string> bestScoreValues){
            for (int i = 0; i < bestScoreNames.Count; i++){
                for (int j = 0; i < bestScores.Count; j++){
                    if(bestScoreNames[i] == bestScores[j].scoreName){
                        bestScores[j].score = bestScoreValues[i];
                    }
                }
            }
        }

        /// <summary>
        /// Ovu funkciju zovemo na kraju svake partije, kako bismo najbolji skor upisali u
        /// leaderboardove na google play i game center. Ova funkcija se moze zvati bilo kada,
        /// cak i kada skor nije najbolji, jer ce se na GP i GC preracunati da li je bolji od postojeceg
        /// </summary>
        public void ReportScore(int score, string leaderboardID)
        {
            Debug.Log("Reporting score " + (long)score + " on leaderboard " + leaderboardID);
            Social.ReportScore(score, leaderboardID, success => {
                Debug.Log(success ? "Reported score successfully" : "Failed to report score");
            });
        }

    }
