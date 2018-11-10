using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

	
[System.Serializable]
    /// <summary>
    /// Klasa koja opisuje score. Cine je ime skora i string koji predstavlja
    /// kriptovanu vrednost skora.
    /// </summary>
	public class Score{

        /// <summary>
        /// Ime skora
        /// </summary>
        public string scoreName;

        /// <summary>
        /// Kriptovani skor
        /// </summary>
        public string score;

        /// <summary>
        /// Lista UI labela u kojima se prikazuje trenutni skor
        /// </summary>
        public List<Text> scoreLabels;

        /// <summary>
        /// Konstruktor koji kriptuje datu int vrednost i postavlja skor
        /// </summary>
        public Score(int sc){
			score = Crypting.EncryptInt2(sc);
		}


	}
