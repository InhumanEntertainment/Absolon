using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBlock : MonoBehaviour
{
    public List<GameObject> Objects = new List<GameObject>();
    public List<GameBlock> NextBlocks = new List<GameBlock>();
    public Vector2 DurationMinMax = new Vector2(5, 10);
    public float Duration;
    float StartTime;

    static public bool Skip = false;

    //=================================================================================================================//
    void Awake()
    {
        StartTime = Time.timeSinceLevelLoad;
        Duration = Mathf.Lerp(DurationMinMax.x, DurationMinMax.y, Random.value);
        gameObject.SetActive(true);
    }

    //=================================================================================================================//
    void Update()
    {
        foreach (GameObject obj in Objects)
        {
            if (obj == null)
            {
                print("Dead!");
            }
        }

        if(Skip || (Objects.Count == 0 && Time.timeSinceLevelLoad - StartTime > Duration))
        {
            // Next Block //
            int index =  Random.Range(0, NextBlocks.Count - 1);
            GameBlock NextBlock = NextBlocks[index];
            Game.Instance.SetBlock(NextBlock);
            Skip = false;
        }
    }
}
