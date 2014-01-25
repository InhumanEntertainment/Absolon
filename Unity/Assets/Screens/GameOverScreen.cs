using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    //============================================================================================================================================================================================//
    public void Restart()
    {
        print("Frontend: Restart");

        Time.timeScale = 1;
        Audio.Music.Play();
        Game.Instance.Play();
        App.Instance.SetScreen("Game");      
    }

    //============================================================================================================================================================================================//
    public void Quit()
    {
        print("Frontend: Quit");

        Game.Instance.CleanupScene();
        App.Instance.SetScreen("Menu");
    }
}
