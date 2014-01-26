using UnityEngine;
using System.Collections;

public class MenuScreen : GameScreen 
{
    public GameData Data;
    public GameText HighScoreText;

    //============================================================================================================================================================================================//
    public void SlideFromTop()
    {
        Audio.Music.Pause();
        HighScoreText.Text = string.Format("{0:n0}", Data.HighScore);
    }

    //============================================================================================================================================================================================//
    public void SlideToBottom()
    {
        gameObject.SetActive(false);
    }

    //============================================================================================================================================================================================//
    public void About()
    {
        print("Frontend: About");
        App.Instance.SetScreen("About");
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
