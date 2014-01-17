using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameLevel : ScriptableObject
{
    public string Name;
    public List<GameObject> Objects;

    //public Vector2 Duration = new Vector2(5, 10);
    //public List<Enemy> Enemies = new List<Enemy>();

    //public float StartTime;

    //============================================================================================================================================//
    void OnEnable()
    {

    }

    //============================================================================================================================================//
    void OnDisable()
    {

    }

    //============================================================================================================================================//
    void OnDestroy()
    {

    }
}

 // One Block at a time //
// Keep track of # enemies, when all dead then new block
// Stage has a list of blocks
// Level has a list of stages


// Group object with placed spawners and enemies
// Make them prefabs
// Draw Boxes for spawners
// On current block set unload and then reload
