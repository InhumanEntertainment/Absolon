using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Game : MonoBehaviour 
{
	static public Game Instance;	
	public int TargetFramerate = 60;
	float FPS = 60;

    // Data //
    public GameData Data;
    public GameText HighScoreText;

    // Blocks //
    public GameObject[] Blocks;
    public int CurrentBlock;

	// Levels //
    public Level[] Levels;
	public string CurrentLevel;
	public bool LoadingLevel;
	public AsyncOperation Async;
	public bool LoadSavedLevel;
	
	// Frontend //
	public GameScreen[] Screens;
	public GameScreen CurrentScreen;
	public GameScreen LastScreen;
    public Transform[] LifeObjects;
	
	// Data //
	//public MutationData Data;
	//public MutationSocial Social = new MutationSocial();
	public bool FullVersion = true;
	
	// Gameplay //
    public int Lives = 3;
    public int Score = 0;
    public GameText ScoreText;
    float SmoothScore = 0;
    public Weapon[] Weapons;
    Player Player;
    public GameOver GameOverObject;

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
	
	//============================================================================================================================================================================================//
	void Awake()
	{
		// Singleton //
		if (Game.Instance == null)
		{
			Instance = this;
			Application.targetFrameRate = TargetFramerate;
			
			if(Screens.Length > 0)
			{
				CurrentScreen = Screens[0];
			}

            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            SetHighScore();
			
			//Data = MutationData.Load();
			
			// Mute Music if Ipod is playing already //
			//if (InhumanIOS.IsMusicPlaying())
			//Audio.MusicMute = true;	
			
			// Connect to GameCenter //
			//Social.Authenticate();
		}
	}
	
	//============================================================================================================================================================================================//
	void OnApplicationQuit()
	{
		//Data.Save ();
	}
	
	//============================================================================================================================================================================================//
	void Update()
	{
		if (TargetFramerate != Application.targetFrameRate)
		{
			Application.targetFrameRate = TargetFramerate;
		}
		
		// Update FPS Counter //
		FPS = Mathf.Lerp(FPS, Time.deltaTime > 0 ? 1f / Time.deltaTime : 0, 0.03f);   
 
        if(CurrentScreen.Name == "Game")
        {
            UpdateScore(); 
        }
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
	public GameScreen GetScreen(string name)
	{
		for (int i = 0; i < Screens.Length; i++)
		{
			if (name == Screens[i].Name)
			{
				return Screens[i];
			}
		}
		
		return null;
	}
	
	//============================================================================================================================================================================================//
	void SetScreen(GameScreen screen)
	{
		print("Set Screen: " + screen.Name);
		
		if (screen != CurrentScreen)
		{
            CurrentScreen.Close(this);
            LastScreen = CurrentScreen;
            CurrentScreen = screen;
            screen.Open(this);
		}
	}
	
	void SetScreen(string name)
	{
		SetScreen(GetScreen(name));
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

        if(EnergyCount > EnergyPerBomb)
        {
            EnergyCount -= EnergyPerBomb;
            EnergyCount = 0;
            SetBombs(BombCount + 1);
            Game.Spawn(BombObject, BombText.transform.position);
        }

        // Update Energy UI //
        EnergyBar.transform.localScale = new Vector3(EnergyCount / (float)EnergyPerBomb * 76, 1, 1);
    }

    //============================================================================================================================================================================================//
    public void SetBombs(int value)
    {
        BombCount = value;

        // Update Bomb UI //
        BombText.Text = BombCount.ToString();

        // Play Effect //
        NewBombEffect.Play();
    }

    //============================================================================================================================================================================================//
    public void ActivateBomb()
    {
        if(BombCount > 0)
        {
            BombCount--;
            SetBombs(BombCount);
            Game.Spawn(BombObject, Player.transform.position);
        }
    }




    //============================================================================================================================================================================================//
    public void NextLevel()
    {
        CleanupScene();
        Blocks[CurrentBlock].SetActive(false);

        CurrentBlock++;
        if(CurrentBlock >= Blocks.Length)
            CurrentBlock = 0;

        Blocks[CurrentBlock].SetActive(true);
        GameText level = GameObject.Find("LevelText").GetComponent<GameText>();
        level.Text = Blocks[CurrentBlock].name.ToLower();
    }
    
    //============================================================================================================================================================================================//
	public void About()
	{
		print("Frontend: About");
		SetScreen("About");
	}
	
	//============================================================================================================================================================================================//
	public void Menu()
	{
		print("Frontend: Menu");
		SetScreen("Menu");
	}
	
	//============================================================================================================================================================================================//
	public void Pause()
	{       
		print("Frontend: Pause");

		SetScreen("Pause");
		Time.timeScale = 0f;
		//Audio.Music.Pause();
	}
	
	//============================================================================================================================================================================================//
	public void GameOver()
	{
        //Time.timeScale = 0;
        CleanupScene();
        SetHighScore();
        //Audio.PlaySound("Game Over");

        SetScreen("Game Over");
        GameOverObject.Play();
	}
	
	//============================================================================================================================================================================================//
	public void Resume()
	{
		print("Frontend: Resume");

		SetScreen("Game");
		Time.timeScale = 1;
		//Audio.Music.Play();
	}
	
	//============================================================================================================================================================================================//
	public void Restart()
	{
		Time.timeScale = 1;
		Audio.Music.Play();
	}
	
	//============================================================================================================================================================================================//
	public void Quit()
	{
		print("Frontend: Quit");
		
		CleanupScene();
		SetScreen("Menu");
		//Time.timeScale = 1;
		if (CurrentLevel != "LevelSelect")
		{
			Audio.PlayMusic("Menu", true);
		}        
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
	public void GameCenter()
	{
		print("Frontend: GameCenter");
		
		UnityEngine.Social.ShowAchievementsUI();
	}
	
	//============================================================================================================================================================================================//
	public void BuyFullVersion()
	{
		print("Frontend: Buy Full Version");
		
#if UNITY_IPHONE
		//StoreKitBinding.purchaseProduct("FullVersion", 1);
#endif
    }

    //============================================================================================================================================================================================//
    public void Sycamore()
    {
        print("Frontend: Sycamore");
        Application.OpenURL("http://www.sycamoredrive.co.uk");
	}
    
	//============================================================================================================================================================================================//
	public void Play()
	{
		print("Frontend: Play");

        // Create Player and Level //

        SetLives(5);
        SetScore(0);
        SetEnergy(0);
        SmoothScore = 0;

        Time.timeScale = 1;
        SetScreen("Game");
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

        HighScoreText.Text = string.Format("{0:n0}", Data.HighScore);        
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
            for (int i = 0; i < LifeObjects.Length; i++)
            {
                LifeObjects[i].gameObject.SetActive(i < Lives);
            }
        }
    }

    //============================================================================================================================================================================================//
    public void Death()
    {               
        SetLives(Lives - 1);

        Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
        foreach(Projectile projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        Buzzer[] buzzers = GameObject.FindObjectsOfType<Buzzer>();
        foreach (Buzzer buzzer in buzzers)
        {
            Destroy(buzzer.gameObject);
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
		
		if(!isPermanent)
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
	
	[System.Serializable]
	public class Level
	{
		public string Name = "Default";
	}
	
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
