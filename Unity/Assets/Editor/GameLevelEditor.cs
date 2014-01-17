using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class GameLevelEditor 
{
    //============================================================================================================================================//
    [MenuItem("Assets/Create/Game Level")]
    public static void CreateLevel()
    {
        ScriptableObjectUtility.CreateAsset<GameLevel>();
    }
}
