using UnityEngine;
using System.Collections;

public class MenuScreen : GameScreen 
{
    public GameText HighScoreText;
    public GameObject RateButton;

    //============================================================================================================================================================================================//
    public void SlideFromTop()
    {
        Audio.Music.Pause();
        HighScoreText.Text = string.Format("{0:n0}",  PlayerPrefs.GetInt("HighScore"));

#if UNITY_WP8 || UNITY_ANDROID || UNITY_IPHONE || UNITY_METRO
        RateButton.SetActive(true);
#else
        RateButton.SetActive(false);
#endif
    }

    //============================================================================================================================================================================================//
    public void SlideToBottom()
    {
        gameObject.SetActive(false);
    }

    //============================================================================================================================================================================================//
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //============================================================================================================================================================================================//
    public void About()
    {
        print("Frontend: About");
        App.Instance.SetScreen("About");
    }

    //============================================================================================================================================================================================//
    public void Rate()
    {
        print("Frontend: Rate");

#if UNITY_WP8
        Application.OpenURL("http://www.windowsphone.com/s?appid=4b00f91e-b277-4573-8dd3-910f7d649b12");

#elif UNITY_ANDROID
        string id = "com.inhuman.absolon";
        if(App.Instance.AndroidStore == App.AndroidStores.Amazon)
		    Application.OpenURL("amzn://apps/android?p=" + id);
	    else
		    Application.OpenURL("market://details?id=" + id);

#elif UNITY_METRO
        Application.OpenURL("ms-windows-store:PDP?PFN=Absolon_k47tbw9vkrfs6");

#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.com/apps/absolon");

#endif
    }

    //============================================================================================================================================================================================//
    public void Facebook()
    {
        print("Frontend: Facebook");

        Application.OpenURL("http://www.facebook.com/inhumanentertainment");
    }

    //============================================================================================================================================================================================//
    public void Twitter()
    {
        print("Frontend: Twitter");

        Application.OpenURL("http://twitter.com/InhumanEnt");
    }

    //============================================================================================================================================================================================//
    public void PlayGame()
    {
        print("Frontend: Play");
        App.Instance.SetScreen("Game");
        Game.Instance.NewGame();
    }

}
