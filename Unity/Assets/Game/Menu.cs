using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public string Message = "";
	
	//=======================================================================================================================================================/
	void Update()
	{
		Vector3 mouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		bool mouse_over = collider2D.OverlapPoint(mouse);
		if(mouse_over)
		{
			if(Input.GetMouseButtonDown(0))
			{
				print ("Menu: " + Message);
				SendMessage (Message);
			}
		}
	}

	//=======================================================================================================================================================/
	void Pause()
	{
		// Do Stuff //
	}

    //=======================================================================================================================================================/
    void Play()
    {
        // Do Stuff //
    }
	
	//=======================================================================================================================================================/
	void Start()
	{

	}
}
