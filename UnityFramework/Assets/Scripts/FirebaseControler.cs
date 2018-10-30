using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using Firebase.Unity.Editor;
using Firebase.Database;
using Facebook;


/// <summary>
/// Controler class that encompasses all Firebase functionality,
/// such as database, storage, push notifications
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

    // Use this for initialization
    void Start () {
        CheckPlayServicesVersion();

    }

    /// <summary>
    /// Sets the editor auth values. Needed for login and auth in editor mode.
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
    /// Initializes Firebase. Sets callback methods
    /// </summary>
    void InitializeFirebase() {
      auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
      auth.StateChanged += AuthStateChanged;
      AuthStateChanged(this, null);
    }


    /// <summary>
    /// Called when auth state changes (user login, logout)
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="eventArgs">Event arguments.</param>
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        Debug.Log("Checking Auth State");
        Debug.Log(auth.CurrentUser);
        Debug.Log(user);

        //TODO: dodati mozda proveru da li postoji uid u player prefs?
        //odnosno fbid ili mail (uid) i pass (auto)
        if(auth.CurrentUser == null){
            SignInAnonimously();
        }

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
    /// Signs the user anonimously. Ads a user to firebase auth users table, with a new id.
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
    /// </summary>
    public void Save()
    {
        if (App.player != null)
        {
            Debug.Log("Save function. Uid : " + App.player.uid);
            string json = JsonUtility.ToJson(App.player);
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
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(App.player.uid)
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Database load error");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("Database load success");
                        DataSnapshot snapshot = task.Result;

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
                            Debug.Log("Uid from Load : " + snapshot.Child("uid").Value.ToString());
                            App.player.name = snapshot.Child("name").Value.ToString();
                            App.player.email = snapshot.Child("email").Value.ToString();
                            App.player.fbid = snapshot.Child("fbid").Value.ToString();
                            App.player.uid = snapshot.Child("uid").Value.ToString();
                            App.localDB.Save();
                        }
                    }
                });
        }
    }

    public void CreateNewDBUser(){
        Debug.Log("Create new DB user");
        if (App.player != null)
        {

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
            Load();
        });
    }

    public void ConnectWithFacebook(Facebook.Unity.AccessToken accessToken){

        Firebase.Auth.Credential credential =
                    Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken.TokenString);

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
            Save();

        });
    }


}
