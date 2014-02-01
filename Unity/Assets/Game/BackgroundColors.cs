using UnityEngine;
using System.Collections;

public class BackgroundColors : MonoBehaviour 
{
    public enum ColorState { Idle, Transition };
    public ColorState State = ColorState.Idle;

    public Color[] Colors;
    public Vector2 DurationMinMax = new Vector2(10, 20);
    public float TransitionDuration = 4;
    public int CurrentColor = 0;
    public int LastColor = 0;
    public float ChangeTime = 0;

    //=======================================================================================================================================================/
    void Start() 
    {
	    // Set Random Color //
        CurrentColor = Random.Range(0, Colors.Length);
        LastColor = CurrentColor;
        renderer.material.SetColor("_BackColor", Colors[CurrentColor]);
        ChangeTime = Time.timeSinceLevelLoad;
	}

    //=======================================================================================================================================================/
    void Update()
    {
        if(State == ColorState.Idle)
        {
            float time = Time.timeSinceLevelLoad - ChangeTime;
            if (time > DurationMinMax.x)
            {
                LastColor = CurrentColor;
                CurrentColor = Random.Range(0, Colors.Length);                  
                ChangeTime = Time.timeSinceLevelLoad;
                State = ColorState.Transition;
            }
        }
        else if(State == ColorState.Transition)
        {
            float delta = (Time.timeSinceLevelLoad - ChangeTime) / TransitionDuration;

            if (delta >= 1)
            {
                ChangeTime = Time.timeSinceLevelLoad;
                State = ColorState.Idle;
            }
            else
            {
                renderer.material.SetColor("_BackColor", Color.Lerp(Colors[LastColor], Colors[CurrentColor], delta));
            }
        }

	}
}
