using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jednostavna klasa zaduzena za firebase cloud messaging
/// </summary>
public class CloudMessaging : MonoBehaviour {

    public void Start()
    {
        /// <summary>
        /// Dodeljujemo kolbek funkcije
        /// </summary>
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    /// <summary>
    /// Funkcija koja se pokrece kada se dobije jedinstveni token za korisnikov device,
    /// nakon automatske inicijalizacije firebase cloud messaginga
    /// </summary>
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);

    }

    /// <summary>
    /// Funkcija koja se pokrece kada se primi nova cloud poruka
    /// </summary>
    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }


}
