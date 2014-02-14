using UnityEngine;
using System.Collections;

public class GameOverScreen : GameScreen
{
    public float ScoreTime = 2;
    float StartTime = 0;
    bool IsPlaying = false;

    public GameText ScoreText;
    public ParticleSystem ScoreEffect;
    public GameObject[] Awards;

    //============================================================================================================================================================================================//
    public void SlideFromTop()
    {
        Time.timeScale = 0f;
        if (!IsPlaying)
        {
            IsPlaying = true;
            StartTime = Time.realtimeSinceStartup;
        }

        Awards[0].SetActive(false);
        Awards[1].SetActive(false);
        Awards[2].SetActive(false);
        if (Game.Instance.Score > 100000)
            Awards[2].SetActive(true);
        else if(Game.Instance.Score > 50000)
            Awards[1].SetActive(true);
        else if(Game.Instance.Score > 25000)
            Awards[0].SetActive(true);
    }

    //============================================================================================================================================================================================//
    public void SlideToBottom()
    {
        gameObject.SetActive(false);
    }

    //============================================================================================================================================//
    void Update()
    {
        if (IsPlaying)
        {
            float delta = (Time.realtimeSinceStartup - StartTime) / ScoreTime;

            if (delta > 1)
            {
                IsPlaying = false;
                ScoreText.Text = string.Format("{0:n0}", Game.Instance.Score);

                Game.Spawn(ScoreEffect, transform.position);
            }
            else
            {
                ScoreText.Text = string.Format("{0:n0}", Game.Instance.Score * delta);
            }
        }
    }

    //============================================================================================================================================================================================//
    public void Restart()
    {
        print("Frontend: Restart");

        Time.timeScale = 1;
        Audio.Music.Play();
        Game.Instance.NewGame();
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
