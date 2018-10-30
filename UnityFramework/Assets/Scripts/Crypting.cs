using UnityEngine;
using System.Collections;

namespace Pokega{

	//Klasa koja se bavi kriptovanjem
	public class Crypting : MonoBehaviour{
	
		public static int salt = 25;
		public static float floatSalt = 25f;

		public static string EncryptInt(int num){
			return (((num + salt) * salt) + salt).ToString();
		}

		//Kriptovanje inta
		public static string EncryptInt2(int num) {
			string encrypted = (((num + salt) * salt) + salt).ToString();
			int sum = 0;
			//Debug.Log ("E: Start encrypted num is " + encrypted);
			string encrypted2 = "";
			int randomNumOfSpaces = Random.Range (1, 4);
			for (int i = 0; i < encrypted.Length; i++) {
				encrypted2 += Number2String (int.Parse(encrypted[i].ToString()));
				sum += encrypted [i];
				for (int j = 0; j < randomNumOfSpaces; j++) {
					int randomLetter = Random.Range(0, 25);
					char let = (char)('a' + randomLetter);
					encrypted2 += let.ToString ();
				}
			}
			encrypted2 += Number2String (randomNumOfSpaces);
			string finalSum = sum.ToString ();
			while(finalSum.Length > 3){
				finalSum = finalSum.Remove (finalSum.Length - 1);
			}
			while(finalSum.Length < 3){
				finalSum.Insert (0, "0");
			}
			encrypted2 += finalSum;
			//Debug.Log ("E: START SUM IS " + finalSum);
			//Debug.Log ("E: ENCRYPTED NUM IS " + encrypted2);

			return encrypted2;
		}

		public static int DecryptInt2(string crypted){
			string randomNumLetter = (crypted[crypted.Length - 4] - 'a').ToString();
			string checker = crypted.Substring (crypted.Length - 3);
			int randomNumSeparator = int.Parse (randomNumLetter);
			int sum = 0;
			string startingCrString = crypted[0].ToString();
			string alphaStartingCr = StringToNumber(crypted[0].ToString()).ToString();
			int i = 0;
			while(i < crypted.Length){
				i += 1 + randomNumSeparator;
				if (i >= crypted.Length || i + 4 + randomNumSeparator >= crypted.Length)
					break;
				startingCrString += crypted [i].ToString();
				alphaStartingCr += StringToNumber(crypted[i].ToString());
			}

			//Debug.Log ("D: Start crypted sum is " + alphaStartingCr);
			for (int j = 0; j < alphaStartingCr.Length; j++) {
				sum += alphaStartingCr [j];
			}
			//Debug.Log ("D: Decrypted sum is " + sum.ToString());
			int realNum = DecryptInt (alphaStartingCr);
			//Debug.Log ("D: Decrypted real number is " + realNum.ToString());
			//Debug.Log ("D: Checker is " + checker);

			if (checker.CompareTo (sum.ToString ()) != 0) {
				Debug.Log ("CHEATER!!!");
				return -1;
			}
			else return realNum;
		}

		public static string EncryptString(string cleanString){
			int sum = 0;
			string encrypted2 = "";
			int randomNumOfSpaces = Random.Range (0, 0);
			for (int i = 0; i < cleanString.Length; i++) {
				encrypted2 += Number2String ((int)cleanString[i]);
				sum += cleanString [i];
				for (int j = 0; j < randomNumOfSpaces; j++) {
					int randomLetter = Random.Range(0, 25);
					char let = (char)('a' + randomLetter);
					encrypted2 += let.ToString ();
				}
			}
			encrypted2 += Number2String (randomNumOfSpaces);
			string finalSum = sum.ToString ();
			while(finalSum.Length > 3){
				finalSum = finalSum.Remove (finalSum.Length - 1);
			}
			while(finalSum.Length < 3){
				finalSum.Insert (0, "0");
			}
			encrypted2 += finalSum;
			//Debug.Log ("E: START SUM IS " + finalSum);
			//Debug.Log ("E: ENCRYPTED NUM IS " + encrypted2);

			return encrypted2;
		}

		public static string DecryptString(string crypted){
			string randomNumLetter = (crypted[crypted.Length - 4] - 'a').ToString();
			string checker = crypted.Substring (crypted.Length - 3);
			int randomNumSeparator = int.Parse (randomNumLetter);
			int sum = 0;
			string clean = "";

			for(int i = 0; i < crypted.Length - 4; i++){
				char ch = (char)StringToNumber (crypted[i].ToString());
				clean += ch.ToString ();
				sum += ch;
			}

			string finalSum = sum.ToString ();

			while(finalSum.Length > 3){
				finalSum = finalSum.Remove (finalSum.Length - 1);
			}
			while(finalSum.Length < 3){
				finalSum.Insert (0, "0");
			}

		//	Debug.Log (clean);
		//	Debug.Log (sum.ToString());
		//	Debug.Log (finalSum);
			if (checker.CompareTo (finalSum) != 0) {
		//		Debug.Log ("CHEATER!!!");
				return "FAILED";
			}
			else return clean;
		}

		public static string Number2String(int number)
		{
			char c = (char)(97 + (number));
			return c.ToString();
		}

		public static int StringToNumber(string str){
			char[] charArr = str.ToCharArray ();
			string num = (charArr [0] - 97).ToString();
			return int.Parse (num);
		}

		public static int DecryptInt(string num) {
			if(num != null){
				int n = System.Int32.Parse(num);
				return (((n - salt) / salt) - salt);
			}
			else return -1;
		}
		
		//Kriptovanje floata
		public static string EncryptFloat(float num) {
			return (((num + salt) * salt) + salt).ToString();
		}
		
		public static float DecryptFloat(string num) {
			if(num != null){
				float n = System.Single.Parse(num);
				return (((n - salt) / salt) - salt);
			}
			else return -1;
		}

	}
}