using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using Firebase.Unity.Editor;
using Firebase.Database;
using Facebook;


/// <summary>
/// Kontroler klasa koja radi sa pamcenjem i uzimanjem podataka sa servera,
/// autentifikacijom korisnika, login, logout.
/// </summary>
public class FirebaseControler : MonoBehaviour {

    public Firebase.FirebaseApp app;
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    public bool firebaseReady = false;
    public DatabaseReference dbReference;
    /// <summary>
    /// The editor database URL. Url string for the database, needed for the database to function in
    /// the editor
    /// </summary>
    public string editorDatabaseUrl;
    /// <summary>
    /// The name of the editor p12 file. This file should be located in the folder that Firebase get started
    /// guide specifies
    /// </summary>
    public string editorP12FileName;
    /// <summary>
    /// The editor service account email. Email of the account used to simulate use logins.
    /// The same is for the password.
    /// </summary>
    public string editorServiceAccountEmail;
    public string editorP12Password;
    public bool signedInAnonimously;

    // Use this for initialization
    void Start () {
        /// <summary>
        /// Provera verzije play servisa na android uredjajima, jer je novija verzija potrebna za rad sa Firebaseom
        /// </summary>
        CheckPlayServicesVersion();

    }


    /// <summary>
    /// Postavljaju se vrednosti potrebne za koriscenje Firebase-a u Editoru.
    /// Adresa baze, ime .p12 fajla i sifra za isti
    /// </summary>
    public void SetEditorAuthValues(){
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://spacewing-38e84.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("firebaseAuthIdentity-pass-notasecret.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("spacewing-38e84@appspot.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
    }

    // Update is called once per frame
    void Update () {

       
    }

    /// <summary>
    /// Inicijalizacija firebasea i dodeljivanje kolbekova
    /// </summary>
    void InitializeFirebase() {
      auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
      auth.StateChanged += AuthStateChanged;
      AuthStateChanged(this, null);
    }


    /// <summary>
    /// Ova funkcija se zove svaki put kada se promeni stanje autentifikacije.
    /// Pri prvom pokretanju na uredjaju, poziva se ova funkcija i vidi se da je 
    /// trenutno logovani auth.CurrentUser == null, kao i user kojeg smo mi deklarisali (pomocna promenljiva)
    /// Nakon toga se poziva anonimna autentifikacija i nakon uspesne autentifikacije, ponovo se aktivira ovaj kolbek,
    /// samo sto ovog puta auth.CurrentUser != null. Ovaj kolbek se poziva nakon svakog logina ili logouta.
    /// </summary>
  
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        Debug.Log("Checking Auth State");
        Debug.Log(auth.CurrentUser);
        Debug.Log(user);

        //TODO: dodati mozda proveru da li postoji uid u player prefs?
        //odnosno fbid ili mail (uid) i pass (auto)
        /// <summary>
        /// Ako je prvo pokretanje na uredjaju
        /// </summary>
        if (auth.CurrentUser == null && !signedInAnonimously){
            signedInAnonimously = true;
            SignInAnonimously();
        }

        /// <summary>
        /// Ovde se uvek ulazi na pocetku, osim kad je prvo pokretanje i oba su null
        /// </summary>
        if (auth.CurrentUser != user) {
            Debug.Log("Current auth user: " + auth.CurrentUser);
            if(user !=null){
                Debug.Log("Local user: " + user.UserId);
            }

            //TODO: ovo nesto ne valja
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
              Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;
            App.player.uid = user.UserId;
            Debug.Log("Player uid : " + App.player.uid);

            if (signedIn) {
              Debug.Log("Signed in " + user.UserId);
                Load();
            }
      }
    }

    /// <summary>
    /// Ako je prvi pot pokrenuta igra na uredjaju, korisnik se loguje anonimno,
    /// dobija neki id i svaki sledeci put ce imati taj id. Anonimni login se samo jednom desi na uredjaju,
    /// samo pri prvom pokretanju.
    /// </summary>
    public void SignInAnonimously(){
        Debug.Log("Signing in anonimously.");
        if (auth != null)
        {
            auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }
                else
                {
                    //TODO: Nakon ovoga treba raditi load i save operacije
                    user = task.Result;
                    App.player.uid = user.UserId;
                    Debug.Log("Signing in anonimously: Completed. UID: " + App.player.uid);
                    //CreateNewDBUser();
                    //Samo treba u Auth state changed zvati load, koji ako nema tog korisnika, pravi novog u bazi
                }

            });
        }
    }

    /// <summary>
    /// Ukoliko zelimo da apdejtujemo email za nekog korisnika, treba to uraditi koriscenjem ove funkcije
    /// </summary>
    public void UpdateUsersEmail(string email){
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.UpdateEmailAsync(email).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User email updated successfully.");
            });
        }
    }

    /// <summary>
    /// The play services version must be up to date, so the Firebase features could work.
    /// This function checks the play services version.
    /// </summary>
    public void CheckPlayServicesVersion(){

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
          var dependencyStatus = task.Result;
          if (dependencyStatus == Firebase.DependencyStatus.Available) {

            // Create and hold a reference to your FirebaseApp, i.e.
                app = Firebase.FirebaseApp.DefaultInstance;
                firebaseReady = true;

                // where app is a Firebase.FirebaseApp property of your application class.
                // Set a flag here indicating that Firebase is ready to use by your
                // application.
                InitializeFirebase();
#if UNITY_EDITOR
                SetEditorAuthValues();
#endif
                // Get the root reference location of the database.
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase dbReference: " + dbReference);
            }
            else {
            UnityEngine.Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                firebaseReady = false;
          }
        });
    }

    /// <summary>
    /// Save this users data. Make a json object from the Player class, and save it to the 
    /// users table, under his uid from the auth table.
    /// 
    /// 
    /// </summary>
    public void Save()
    {
        if (App.player != null)
        {
            Debug.Log("Save function. Uid : " + App.player.uid);

            /// <summary>
            /// Player objekat iz App se prebacuje u JSON i kao takav se pamti u bazi,
            /// u kolekciji users, pod id-jem autentifikovanog igraca.
            /// </summary>
            string json = JsonUtility.ToJson(App.player);
            Debug.Log(json);
            dbReference.Child("users").Child(App.player.uid).SetRawJsonValueAsync(json)
               .ContinueWith(task =>
               {
                   if (task.IsFaulted)
                   {
                       Debug.Log("Database write error");
                   }
                   else if (task.IsCompleted)
                   {
                       Debug.Log("Database write success");
                       /// <summary>
                       /// Pamtimo sve iz App.player objekta u Player Prefs, takodje
                       /// </summary>
                       App.localDB.Save();
                   }
               });
        }
    }

    /// <summary>
    /// Load the user from the users table, under the id from the Player class (and the auth table)
    /// </summary>
    public void Load()
    {
        if (App.player != null)
        {
            Debug.Log("Firebase load. Player uid : " + App.player.uid);

            /// <summary>
            /// Trazimo igraca po user id-ju u bazi
            /// </summary>
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(App.player.uid)
                .GetValueAsync().ContinueWith(task =>
                {
                    /// <summary>
                    /// Ovo se poziva samo u slucaju greske, ne i kada nema trazenog korisnika
                    /// </summary>
                    if (task.IsFaulted)
                    {
                        Debug.Log("Database load error");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("Database load success");
                        DataSnapshot snapshot = task.Result;

                        /// <summary>
                        /// Ako korisnik sa ovim id-jem ne postoji
                        /// </summary>
                        if (!snapshot.Exists)
                        {
                            Debug.Log("No user with uid " + App.player.uid);
                            string json = JsonUtility.ToJson(App.player);
                            /// <summary>
                            /// Napravi korisnika sa tim id-jem
                            /// </summary>
                            dbReference.Child("users").Child(App.player.uid).SetRawJsonValueAsync(json)
                               .ContinueWith(task2 =>
                               {
                                   if (task2.IsFaulted)
                                   {
                                       Debug.Log("Creating new user - Database write error");
                                   }
                                   else if (task2.IsCompleted)
                                   {
                                       Debug.Log("Creating new user - Database write success");
                                       App.localDB.Save();
                                   }
                               });
                        }
                        else
                        {

                            /// <summary>
                            /// Ako postoji korisnik sa tim id-jem, ucitaj podatke sa servera u player objekat
                            /// </summary>
                            Debug.Log("Uid from Load : " + snapshot.Child("uid").Value.ToString());
                            Debug.Log(snapshot.HasChild("bestScoreNames"));
                            Debug.Log(snapshot.Child("bestScoreNames").Value.ToString());

                            App.player.name = snapshot.Child("name").Value.ToString();
                            App.player.email = snapshot.Child("email").Value.ToString();

                            App.player.fbid = snapshot.Child("fbid").Value.ToString();
                            App.player.uid = snapshot.Child("uid").Value.ToString();
                            App.player.timesPlayed = int.Parse(snapshot.Child("timesPlayed").Value.ToString());
                            Debug.Log(snapshot.Child("bestScoreNames").Value.ToString());
                            Debug.Log(snapshot.Child("bestScoreValues").Value.ToString());

                            /// <summary>
                            /// Best scores se loaduju u player objekat
                            /// </summary>
                            App.player.bestScoreNames = ListObjToString(snapshot.Child("bestScoreNames").Value as List<System.Object>);
                            App.player.bestScoreValues = ListObjToString(snapshot.Child("bestScoreValues").Value as List<System.Object>);

                            /// <summary>
                            /// Best scores treba loadovati i u ScoreControler, jer se odatle citaju trenutne vrenosti 
                        /// i vrse operacije sa skorom.
                            /// </summary>
                            App.score.SetBestScoresFromFirebase(App.player.bestScoreNames, App.player.bestScoreValues);

                            App.localDB.Save();
                        }
                    }
                });
        }
    }

    /// <summary>
    /// Funkcija koja prebacuje Listu objekata tipa Object u Listu stringova.
    /// Potrebna nam je jer listu skorova Firebase vraca kao listu objekata, a ne stringova, sto bi dovelo do greske
    /// </summary>
    public List<string> ListObjToString(List<System.Object> list){
        List<string> stringList = new List<string>();
        foreach(System.Object obj in list){
            stringList.Add(obj.ToString());
        }
        return stringList;
    }

    /// <summary>
    /// Kreira se novi user u bazi, pod id-jem koji dobijemo iz auth.CurrentUser objekta.
    /// Ovo se poziva samo prilikom prvog logina na uredjaju.
    /// </summary>
    public void CreateNewDBUser(){
        Debug.Log("Create new DB user");
        if (App.player != null)
        {

            /// <summary>
            /// Za svaki slucaj proveravamo da li korisnik vec postoji
            /// </summary>
            FirebaseDatabase.DefaultInstance.GetReference("users")
            .Child(App.player.uid)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("Checking if user already exists - Database load error");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Checking if user already exists - Database load success");
                    DataSnapshot snapshot = task.Result;

                    /// <summary>
                    /// Ako ne postoji, pravimo novog korisnika
                    /// </summary>
                    if (!snapshot.Exists)
                    {

                        Debug.Log("No user with uid " + App.player.uid);
                        string json = JsonUtility.ToJson(App.player);
                        dbReference.Child("users").Child(App.player.uid).SetRawJsonValueAsync(json)
                           .ContinueWith(task2 =>
                           {
                               if (task2.IsFaulted)
                               {
                                   Debug.Log("Creating new user - Database write error");
                               }
                               else if (task2.IsCompleted)
                               {
                                   Debug.Log("Creating new user - Database write success");
                                   App.localDB.Save();
                               }
                           });
                    }
                    else
                    {
                        /// <summary>
                        /// Ako korisnik vec postoji, loadujemo ga sa servera.
                        /// </summary>
                        Debug.Log("User already exists, no need to create new.");
                        App.player.name = snapshot.Child("name").Value.ToString();
                        App.player.email = snapshot.Child("email").ToString();
                        App.player.fbid = snapshot.Child("fbid").ToString();
                        App.player.uid = snapshot.Child("uid").ToString();
                        App.localDB.Save();
                    }
                }
            });


        }
    }

    /// <summary>
    /// Ova funkcija se zove kada se proba linkovanje trenutnog naloga sa fejsbukom(klikom na dugme), a neki drugi nalog je vec linkovan sa
    /// istim fb id-jem. Onda se poziva ova funkcija, gde se ucitavaju podaci korisnika koji je vec linkovan od ranije.
    /// </summary>
    public void SignInWithFacebook(string accessToken){

        Firebase.Auth.Credential credential =
        Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            user = newUser;
            App.player.uid = user.UserId;
            //Save();
            //Ovde ne treba save, nego load
            /// <summary>
            /// Ucitavamo podatke tog korisnika koji je linkovan sa zadatim fb id-jem
            /// </summary>
            Load();
        });
    }

    /// <summary>
    /// Funkcija koja linkuje facebook nalog (fb id) sa korisnickim nalogom na Firebase-u
    /// Uz anonimnog korisnika se dodaju novi kredencijali (fb) i svako sledece pozivanje ove funkcije nece uspeti
    /// jer je fbid vec linkovan.
    /// </summary>

    public void ConnectWithFacebook(Facebook.Unity.AccessToken accessToken){

        Firebase.Auth.Credential credential =
                    Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken.TokenString);

        /// <summary>
        /// Pokusava se linkovanje sa fejsbukom
        /// </summary>
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("LinkWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                //Nalog je vec linkovan
                //Treba ucitati podatke tog usera
                Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
                SignInWithFacebook(accessToken.TokenString);
                return;
            }

            /// <summary>
            /// Uspelo je linkovanje
            /// </summary>
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Credentials successfully linked to Firebase user: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Debug.Log("Old user id : " + user.UserId);
            Debug.Log("New user id : " + newUser.UserId);

            //newUser ostaje isti ako se uspesno linkuje
            user = newUser;
            App.player.uid = user.UserId;
            App.player.fbid = accessToken.UserId;
            Debug.Log("Player fbid : " + App.player.fbid);

            /// <summary>
            /// Pamtimo stare podatke sa novim id-jem na server
            /// </summary>
            Save();

        });
    }

    /// <summary>
    /// Koristiti ovu funkciju ukoliko zelimo da izlogujemo korisnika
    /// </summary>
    public void LogOut(){
        auth.SignOut();
    }

}
