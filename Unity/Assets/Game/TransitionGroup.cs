using UnityEngine;
using System.Collections;

public class TransitionGroup : MonoBehaviour 
{
    public Transition[] Transitions;
    public float Delay = 0f;
    public float Spacing = 0.2f;

    //======================================================================================================================================//
    void Awake()
    {
        for (int i = 0; i < Transitions.Length; i++)
        {
            Transitions[i].Delay = Delay + Spacing * i;
        }
    }
    //======================================================================================================================================//
    void Play()
    {
	    for(int i=0; i<Transitions.Length; i++)
        {
            Transitions[i].Delay = Delay + Spacing * i;
            Transitions[i].Play();
        }
	}

    //======================================================================================================================================//
    void Update()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            Play();
        }
	}
}
