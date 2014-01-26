﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : GameScreen
{
    static public Game Instance;
    public Player PlayerObject;
    public Vector3 PlayerStartPosition;

    // Data //
    public GameData Data;
    public GameText HighScoreText;

    // Blocks //
    public GameObject[] Blocks;
    public GameBlock CurrentBlock;
    public GameBlock StartBlock;

    // Gameplay //
    public int Lives = 3;
    public GameText LivesText;
    public int Score = 0;
    public GameText ScoreText;
    float SmoothScore = 0;
    public Weapon[] Weapons;
    public Player Player;

    // Energy //
    public int EnergyCount = 0;
    public int EnergyPerBomb = 100;
    public SpriteRenderer EnergyBar;

    // Bombs //
    public int InitialBombCount = 3;
    public int BombCount = 3;
    public ParticleSystem NewBombEffect;
    public GameText BombText;
    public Bomb BombObject;

    // Difficulty //
    float StartTime;
    public float DifficultyRampDuraction = 60;
    public float DifficultyAcceleration = 1;
    public float Difficulty = 0;
    public float DifficultyVelocityMax = 0.1f;
    public int DifficultyLineLength = 500;
    float DifficultyVelocity = 0;
    float DifficultyMin = 0;
    float DifficultyMax = 1;
    List<float> DifficultyLine = new List<float>();

    public Transform[] LifeObjects;

    //============================================================================================================================================================================================//
    public void Awake()
    {
        // Singleton //
        if (Instance == null)
        {
            Instance = this;          
            SetHighScore();
        }
    }

    //============================================================================================================================================================================================//
    void Update()
    {
        if (App.Instance.CurrentScreen.name == "Game")
        {
            UpdateScore();

            // Difficulty Ramp //
            /*
            DifficultyVelocity += (Random.value * DifficultyAcceleration - (DifficultyAcceleration * 0.5f));
            if (Mathf.Abs(DifficultyVelocity) > DifficultyVelocityMax)
            {
                DifficultyVelocity = Mathf.Sign(DifficultyVelocity) * DifficultyVelocityMax;
            }

            Difficulty += DifficultyVelocity * Time.deltaTime;
            if (Difficulty < DifficultyMin)
            {
                Difficulty = DifficultyMin;
                DifficultyVelocity *= 0.95f;
            }

            if (Difficulty > DifficultyMax)
            {
                DifficultyMax = Difficulty;
                DifficultyMin = DifficultyMax * 0.25f;
                DifficultyMin = Mathf.Max(0, DifficultyMin);
                DifficultyVelocity *= 0.95f;
            }*/

            Difficulty = (Time.timeSinceLevelLoad - StartTime) / DifficultyRampDuraction;
        }
    }

    //============================================================================================================================================================================================//
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Draw Bounds //
            Gizmos.color = Color.white;
            Vector3 TopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 101));
            Vector3 BottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
            Vector3 Minimum = Camera.main.ScreenToWorldPoint(new Vector3(0, DifficultyMin / DifficultyMax * 101, 0));
            Gizmos.DrawLine(new Vector3(TopLeft.x, TopLeft.y, 0), new Vector3(BottomRight.x, TopLeft.y, 0));
            Gizmos.DrawLine(new Vector3(TopLeft.x, BottomRight.y, 0), new Vector3(BottomRight.x, BottomRight.y, 0));
            //Gizmos.DrawLine(new Vector3(TopLeft.x, Minimum.y, 0), new Vector3(BottomRight.x, Minimum.y, 0));

            // Draw Graph //
            Gizmos.color = Color.cyan;
            DifficultyLine.Insert(0, Difficulty);
            if (DifficultyLine.Count > DifficultyLineLength)
                DifficultyLine.RemoveRange(DifficultyLineLength, DifficultyLine.Count - DifficultyLineLength);

            Vector3 To = Vector3.zero;
            for (int i = 0; i < DifficultyLine.Count; i++)
            {
                Vector3 From = new Vector3((DifficultyLineLength - 1 - i) / (float)DifficultyLineLength * Screen.width, DifficultyLine[i] / DifficultyMax * 100, 0);
                From = Camera.main.ScreenToWorldPoint(From);
                From.z = 0;
                if (i == 0)
                    To = From;

                Gizmos.DrawLine(From, To);
                To = From;
            }
        }
    }

    //============================================================================================================================================================================================//
    public void NewGame()
    {
        print("Frontend: Play");

        // Create Player and Level //
        CleanupScene();
        SetLives(3);
        SetScore(0);
        SetEnergy(0);
        SetBombs(5);
        SmoothScore = 0;
        Difficulty = 0;
        DifficultyMin = 0;
        DifficultyMax = 1;
        DifficultyVelocity = 0;
        Player = (Player)Game.Spawn(PlayerObject, PlayerStartPosition);
        SetBlock(StartBlock);
        StartTime = Time.timeSinceLevelLoad;

        Time.timeScale = 1;
        App.Instance.SetScreen("Game");
        Audio.PlayMusic("Music", true);
    }

       //============================================================================================================================================================================================//
    public void NextBlock()
    {
        GameBlock.Skip = true;
    }

    //============================================================================================================================================================================================//
    public void Pause()
    {
        print("Frontend: Pause");   
   
        Time.timeScale = 0f;
        Audio.Music.Pause();
        App.Instance.SetScreen("Pause");
    }

    //============================================================================================================================================================================================//
    public void GameOver()
    {
        print("GAME OVER!");

        Time.timeScale = 0;
        CleanupScene();
        SetHighScore();
        Audio.PlaySound("Game Over");
        App.Instance.SetScreen("Game Over");
        Game.Instance.gameObject.SetActive(false);              
    }

    //============================================================================================================================================================================================//
    public void SetBlock(GameBlock block)
    {
        // Remove old spawners //
        if (CurrentBlock != null)
        {
            Destroy(CurrentBlock.gameObject);
        }
        
        // Spawn new Block //
        CurrentBlock = (GameBlock)Game.Spawn(block);
    }

    //============================================================================================================================================================================================//
    public void AddEnergy(int value)
    {
        SetEnergy(EnergyCount + value);
    }

    //============================================================================================================================================================================================//
    public void SetEnergy(int value)
    {
        EnergyCount = value;

        if (EnergyCount > EnergyPerBomb)
        {
            EnergyCount -= EnergyPerBomb;
            EnergyCount = 0;
            SetBombs(BombCount + 1);
            Game.Spawn(NewBombEffect, BombText.transform.position);
        }

        EnergyBar.transform.localScale = new Vector3(EnergyCount / (float)EnergyPerBomb * 76, 1, 1);
    }

    //============================================================================================================================================================================================//
    public void SetBombs(int value)
    {
        BombCount = value;
        BombText.Text = "x" + BombCount.ToString();
        NewBombEffect.Play();
    }

    //============================================================================================================================================================================================//
    public void ActivateBomb()
    {
        if (BombCount > 0)
        {
            BombCount--;
            SetBombs(BombCount);
            Game.Spawn(BombObject, Player.transform.position);
        }
    }

    //============================================================================================================================================================================================//
    public void AddScore(int value)
    {
        SetScore(Score + value);
    }

    //============================================================================================================================================================================================//
    public void SetScore(int value)
    {
        Score = value;
        ScoreText.Text = string.Format("{0:n0}", SmoothScore);
    }

    //============================================================================================================================================================================================//
    public void SetHighScore()
    {
        if (Score > Data.HighScore)
        {
            Data.HighScore = Score;
            print("New High Score: " + Score.ToString());
        }
    }

    //============================================================================================================================================================================================//
    public void UpdateScore()
    {
        SmoothScore = SmoothScore * 0.8f + Score * 0.2f;
        ScoreText.Text = string.Format("{0:n0}", SmoothScore);
    }

    //============================================================================================================================================================================================//
    public void SetLives(int value)
    {
        Lives = value;
        
        if (Lives < 0)
        {
            GameOver();
        }
        else
        {
            LivesText.Text = "x" + Lives.ToString();     
        }
    }

    //============================================================================================================================================================================================//
    public void Death()
    {      
        Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        Buzzer[] buzzers = GameObject.FindObjectsOfType<Buzzer>();
        foreach (Buzzer buzzer in buzzers)
        {
            Destroy(buzzer.gameObject);
        }

        Audio.PlaySound("Player Death");
        SetLives(Lives - 1);
    }

    //============================================================================================================================================================================================//
    public void CleanupScene()
    {
        GameObject objectsGroup = GameObject.Find("Objects");
        if (objectsGroup != null)
        {
            for (int i = 0; i < objectsGroup.transform.childCount; i++)
            {
                Destroy(objectsGroup.transform.GetChild(i).gameObject);
            }
        }
    }

    //============================================================================================================================================================================================//
    // Spawn objects into group so they can be easily cleanup up //
    //============================================================================================================================================================================================//
    static public Object Spawn(Object original)
    {
        return Game.Spawn(original, Vector3.zero, Quaternion.identity, false);
    }

    static public Object Spawn(Object original, Vector3 position)
    {
        return Game.Spawn(original, position, Quaternion.identity, false);
    }

    static public Object Spawn(Object original, Vector3 position, Quaternion rotation)
    {
        return Game.Spawn(original, position, rotation, false);
    }

    static public Object Spawn(Object original, Vector3 position, Quaternion rotation, bool isPermanent)
    {
        Object obj = Instantiate(original, position, rotation);

        if (!isPermanent)
        {
            GameObject objectsGroup = GameObject.Find("Objects");
            if (objectsGroup != null)
            {
                Transform xform = null;
                if (obj is GameObject)
                    xform = ((GameObject)obj).transform;
                else if (obj is Component)
                    xform = ((Component)obj).transform;

                if (xform != null)
                    xform.parent = objectsGroup.transform;
            }
        }

        return obj;
    }
}
