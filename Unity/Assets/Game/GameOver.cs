using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
    public float ScoreTime = 2;
    float StartTime = 0;
    bool IsPlaying = false;

    public GameText ScoreText;
    public ParticleSystem ScoreEffect;

    //============================================================================================================================================//
    public void Play()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            StartTime = Time.realtimeSinceStartup;
        }
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
}
