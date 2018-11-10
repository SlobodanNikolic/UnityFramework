using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
/// <summary>
/// Klasa zaduzena za kupovinu in-app purchasa na iOSu
/// </summary>
public class Purchaser : MonoBehaviour, IStoreListener
{
    /// <summary>
    /// Lista id-jeva consumable proizvoda sa itunesconnect.com
    /// </summary>
    public List<string> consumableIds;
    /// <summary>
    /// Lista akcija koje se izvrsavaju nakon svakog kupljenog consumable proizvoda
    /// </summary>
    public List<UnityEvent> consumablePurchasedActions;
    /// <summary>
    /// Lista id-jeva non consumable proizvoda sa itunesconnect.com
    /// </summary>
    public List<string> nonConsumableIds;
    /// <summary>
    /// Lista akcija koje se izvrsavaju nakon svakog kupljenog non consumable proizvoda
    /// </summary>
    public List<UnityEvent> nonConsumablePurchasedActions;

	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    /// <summary>
    /// Samo test vrednosti, ne obracati paznju na ovo
    /// </summary>
    public static string kProductIDConsumable =    "consumable";   
	public static string kProductIDNonConsumable = "nonconsumable";
	public static string kProductIDSubscription =  "subscription"; 

	// Apple App Store-specific product identifier for the subscription product.
	private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

	// Google Play Store-specific product identifier subscription product.
	private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original"; 

	void Start()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}

	}

    /// <summary>
    /// Inicijalizacija sistema za kupovinu
    /// </summary>
	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
		builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        //builder.AddProduct (smallPackId, ProductType.Consumable);
        //builder.AddProduct (mediumPackId, ProductType.Consumable);
        //builder.AddProduct (bigPackId, ProductType.Consumable);

        /// <summary>
        /// Svaki od proizvoda mora da se "prijavi" pri inicijalizaciji i doda u builder
        /// </summary>
        foreach (string id in consumableIds){
            builder.AddProduct(id, ProductType.Consumable);
        }

        foreach (string id in nonConsumableIds)
        {
            builder.AddProduct(id, ProductType.NonConsumable);
        }

        /// <summary>
        /// Ovo je testna vrednost
        /// </summary>
        // Continue adding the non-consumable product.
        builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

		// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
		// if the Product ID was configured differently between Apple and Google stores. Also note that
		// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
		// must only be referenced here. 
		builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
			{ kProductNameAppleSubscription, AppleAppStore.Name },
			{ kProductNameGooglePlaySubscription, GooglePlay.Name },
		});

		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		
        UnityPurchasing.Initialize(this, builder);
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

    /// <summary>
    /// Pomocna funkcija za kupovinu consumable po indeksu
    /// </summary>
    public void BuyConsumableByIndex(int index){
        BuyProductID(consumableIds[index]);
    }


    /// <summary>
    /// Pomocna funkcija za kupovinu non consumable po indeksu
    /// </summary>
    public void BuyNonConsumableByIndex(int index)
	{
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(nonConsumableIds[index]);
    }

    /// <summary>
    /// Neimplementirane stvari, ali pod komentarima je preporuka sta treba uraditi.
    /// Ovu funkciju zvati ako se dugo ne dobije kolbek o uspesnoj ili neuspesnoj kupovini
    /// </summary>
	public void CheckForPurchaseDelay(){
  //      if (App.ui.currentPopUp.name == "UIWaitPurchase") {
		//	App.ui.SetPopUp ("UIWaitPurchase", true);
		//	App.ui.SetPopUp ("UIWaitPurchaseFailed");
		//}
	}

	public void BuySubscription()
	{
		// Buy the subscription product using its the general identifier. Expect a response either 
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		// Notice how we use the general product identifier in spite of this ID being mapped to
		// custom store-specific identifiers above.
		BuyProductID(kProductIDSubscription);
	}

    /// <summary>
    /// Funkcija za kupovinu itema po id-ju
    /// </summary>
	void BuyProductID(string productId)
	{
		//App.ui.SetPopUp ("UIWaitPurchase");
		//Invoke ("CheckForPurchaseDelay", 30f);

		Debug.Log ("Buy product id " + productId);
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			Debug.Log ("Purchasing initialized already.");
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
				//App.ui.SetPopUp ("UIWaitPurchase", true);

			}
			// Otherwise ...
			else
			{
                //App.ui.SetPopUp ("UIWaitPurchase", false);
                //TODO: Otvoriti popup koji kaze da je failovao purchase
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
            //TODO:Otvoriti popup koji kaze zasto je fail
            //App.ui.SetPopUp ("UIWaitPurchase", false);

			InitializePurchasing();
		}
	}


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    /// <summary>
    /// Funkcija koja se zove pri kliku na restore dugme
    /// </summary>
    public void RestorePurchases()
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
            //TODO: Popup
			InitializePurchasing ();
			//Invoke ("RestorePurchases", 1f);
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer)
		{
            //todo:
			//Ovde izbaciti popup
  			//App.ui.SetPopUp ("UIRestorePurchase");

			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
                //App.ui.SetPopUp("UIRestorePurchase", false);
				if(result){
					Debug.Log("Purchases restored");
                    //TODO: Popup
				}
				else{
					Debug.Log("Purchases not restored");
                    //todo: popup
				}
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		// Otherwise ...
		else
		{
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;


        //Ovo stavljam pod komentare zbog toga sto odmah na pocetku trazi apple id
		//extensions.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
		//	if (result) {
		//		Debug.Log("Restoration process succedeed");

		//		// This does not mean anything was restored,
		//		// merely that the restoration process succeeded.
		//	} else {
		//		Debug.Log("Restorations failed");
		//		// Restoration failed.
		//	}
		//});
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
        //TODO: popup
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

    /// <summary>
    /// Funkcija koja se zove nakon uspesne kupovine
    /// </summary>
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
        /// <summary>
        /// Ovde treba ugasiti popup za wait purchase
        /// </summary>
        //App.ui.SetPopUp("UIWaitPurchase", false);

        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            return PurchaseProcessingResult.Complete;
		}

        for (int i = 0; i < consumableIds.Count; i++){
            if (String.Equals(args.purchasedProduct.definition.id, consumableIds[i], StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add coins to the player's in-game score.
                consumablePurchasedActions[i].Invoke();
                return PurchaseProcessingResult.Complete;

            }
        }
		

		// Or ... a non-consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            return PurchaseProcessingResult.Complete;

        }

        for (int i = 0; i < nonConsumableIds.Count; i++)
        {
            if (String.Equals(args.purchasedProduct.definition.id, nonConsumableIds[i], StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add coins to the player's in-game score.
                nonConsumablePurchasedActions[i].Invoke();
                return PurchaseProcessingResult.Complete;

            }
        }


        // Or ... a subscription product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            return PurchaseProcessingResult.Complete;

        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else 
		{
            //TODO: popup
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            //App.ui.SetPopUp ("UIWaitPurchase", true);
            return PurchaseProcessingResult.Complete;

        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 

    }

    /// <summary>
    /// Funkcija koja se zove nakon neuspesne kupovine
    /// </summary>
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		//App.ui.SetPopUp ("UIWaitPurchase", true);
        //TODO:Popup

	}

}
