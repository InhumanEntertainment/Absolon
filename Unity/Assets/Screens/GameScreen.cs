using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameScreen
{
    public string Name = "Default";
    public GameAnim[] AnimationsOpen;
    public GameAnim[] AnimationsClose;

    //============================================================================================================================================//
    public void Open(App game)
    {
        for (int i = 0; i < AnimationsOpen.Length; i++)
        {
            AnimationsOpen[i].Play();
        }
    }

    //============================================================================================================================================//
    public void Close(App game)
    {
        for (int i = 0; i < AnimationsClose.Length; i++)
        {
            AnimationsClose[i].Play();
        }
    }
}
