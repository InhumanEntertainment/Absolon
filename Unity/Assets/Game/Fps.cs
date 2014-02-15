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

        uint memory = Profiler.GetTotalAllocatedMemory();
        text.Text = fps + " : " + Game.Instance.Difficulty.ToString("N2") + " : " + string.Format("{0:n0}", memory / 1024f) + "mb";	
	}
}
