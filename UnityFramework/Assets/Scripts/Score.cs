using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Pokega{
	
[System.Serializable]

	public class Score{
	
		public string scoreName;

		//Kriptovani skor
		public string score;

		//Lista labela u kojima se prikazuje skor
		public List<Text> scoreLabels;

		public Score(int sc){
			score = Crypting.EncryptInt2(sc);
		}

		public Score(string name, int sc, bool isItFloat){
			score = Crypting.EncryptInt2(sc);
			scoreName = name;
		}

	}
}