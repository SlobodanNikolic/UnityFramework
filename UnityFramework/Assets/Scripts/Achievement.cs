﻿using UnityEngine;
using System.Collections;


namespace Pokega{
	
	[System.Serializable]
	public class Achievement : MonoBehaviour{
		[SerializeField]

		//iOS and Android achievement IDs
		public string idIOS;
		public string idAND;
		public string name;

		//Value in points (Google play has limited num of points total for all of the achievements)
		public double value;


		public Achievement(string _id, string _name, double _value){
			idIOS = _id;
			name = _name;
			value = _value;
		}

		
	}
}