using UnityEngine;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour
{
    private BannerView adObj;

    private void Start() 
    {
        MobileAds.Initialize( adSituation => { });

        adObj = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.SmartBanner, AdPosition.Top);
        AdRequest adRequest = new AdRequest.Builder().Build();
        adObj.LoadAd(adRequest);    
    }

    private void OnDestroy() 
    {
        if(adObj != null)
            adObj.Destroy();    
    }
}
