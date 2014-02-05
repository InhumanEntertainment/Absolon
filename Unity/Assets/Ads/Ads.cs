using UnityEngine;
using System.Collections;
using Reign;

public class Ads : MonoBehaviour 
{
    //============================================================================================================================================================================================//
    void Start()
    {
        AdDesc adDesc = new AdDesc()
        {
            Testing = true,
            Visible = true,
            EventCallback = adEvent,

            // Win8 settings
            /*Win8_MicrosoftAdvertising_ApplicationID = "",
            Win8_MicrosoftAdvertising_UnitID = "",
            Win8_MicrosoftAdvertising_AdGravity = AdGravity.BottomCenter,
            Win8_MicrosoftAdvertising_AdSize = Win8_MicrosoftAdvertising_AdSize.Wide_728x90,

            // WP8 settings
            WP8_MicrosoftAdvertising_ApplicationID = "",
            WP8_MicrosoftAdvertising_UnitID = "",
            WP8_MicrosoftAdvertising_AdGravity = AdGravity.BottomCenter,
            WP8_MicrosoftAdvertising_AdSize = WP8_MicrosoftAdvertising_AdSize.Wide_480x80,

            // iOS settings
            iOS_MicrosoftAdvertising_AdGravity = AdGravity.BottomCenter,*/

            // Android settings
            Android_AdAPI = AdAPIs.AdMob,// Choose between AdMob or AmazonAds
            Android_AdMob_UnitID = "ca-app-pub-1060625878365419/6352906285",
            Android_AdMob_AdGravity = AdGravity.BottomRight,
            //Android_AmazonAds_ApplicationKey = "",
            //Android_AmazonAds_AdGravity = AdGravity.BottomCenter,
            //Android_AmazonAds_AdSize = Android_AmazonAds_AdSize.Wide_320x50
        };

        AdManager.CreateAd(adDesc, adCreatedCallback);  
    }

    //============================================================================================================================================================================================//
    private void adCreatedCallback(bool succeeded)
    {
        string adStatus = succeeded ? "Ads Succeded" : "Ads Failed";
        print(adStatus);
    }

    //============================================================================================================================================================================================//
    private void adEvent(AdEvents adEvent, string eventMessage)
    {
        switch (adEvent)
        {
            case AdEvents.Refreshed: print("Ad Refreshed"); break;
            case AdEvents.Clicked: print("Ad Clicked"); break;
            case AdEvents.Error: print("Ad Error: " + eventMessage); break;
        }
    }
}
