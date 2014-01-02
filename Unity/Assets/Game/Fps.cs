using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour 
{
    float FPS = 0;

    //============================================================================================================================================//
    void Update() 
    {
        FPS = FPS * 0.98f + (1 / Time.deltaTime) * 0.02f;
        if (FPS == Mathf.Infinity)
            FPS = 0;

        string fps = FPS.ToString("n2");
        GameText text = GetComponent<GameText>();
        text.Text = fps;	
	}
}
