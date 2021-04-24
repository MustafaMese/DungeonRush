using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class AdsInterstitial : MonoBehaviour
{
    private InterstitialAd adObj;

    private void Start() 
    {
        MobileAds.Initialize( adSituation => { });    
    }

    private void OnGUI() 
    {
        if(GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 300, 300, 300), "Reklam Göster"))    
        {
            if(adObj != null)
                adObj.Destroy();
            
            adObj = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
            AdRequest adRequest = new AdRequest.Builder().Build();
            adObj.LoadAd(adRequest);

            StartCoroutine(ShowAd());
        }
    }

    private IEnumerator ShowAd()
    {
        while(!adObj.IsLoaded())
            yield return null;
        adObj.Show();
    }

    private void OnDestroy() 
    {
        if(adObj != null)
            adObj.Destroy();    
    }
}
