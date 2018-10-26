using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalityTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConsumablePurchased()
    {
        Debug.Log("Consumable purchased!");
    }

    public void NonConsumablePurchased()
    {
        Debug.Log("Non consumable purchased!");
    }

    public void BuyConsumable(){
        //App.purchaser.BuyConsumableByIndex(0);
    }

    public void BuyNonConsumable(){
        //App.purchaser.BuyNonConsumableByIndex(0);
    }

    public void ShowInterstitial(){
        
    }

    public void ShowRewarded(){

    }
}
