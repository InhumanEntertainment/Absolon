using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class GameFontEditor 
{
    //============================================================================================================================================//
    [MenuItem("Inhuman/Create Game Font")]
    public static void CreateFont()
    {
        GameFont asset = GameFont.CreateInstance<GameFont>();
        AssetDatabase.CreateAsset(asset, "Assets/Font.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
