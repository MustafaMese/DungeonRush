using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsRewarded : MonoBehaviour
{
    private RewardBasedVideoAd adObj;

    private void Start() 
    {
        MobileAds.Initialize(adSituation => { });
        adObj = RewardBasedVideoAd.Instance;
    }

    private void OnGUI() 
    {
        if(GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height- 300, 300, 300), "Reklamı Göster"))
        {
            AdRequest adRequest = new AdRequest.Builder().Build();
            adObj.LoadAd(adRequest, "ca-app-pub-3940256099942544/5224354917");
            StartCoroutine(ShowAd());
        }    
    }

    private IEnumerator ShowAd()
    {
        while(!adObj.IsLoaded())
            yield return null;
        adObj.Show();
    }
}
