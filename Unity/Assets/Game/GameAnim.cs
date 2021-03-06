using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameAnim
{
    public string Animation = "Idle";
    public MenuAnimation Target;

    //============================================================================================================================================//
    public void Play()
    {
        Target.gameObject.SetActive(true);
        Target.Play(Animation);
    }
}
