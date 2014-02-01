using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBlock : MonoBehaviour
{
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
        // Remove Must Kill Objects //
        List<GameObject> KillUs = new List<GameObject>();
        foreach (GameObject enemy in Game.Instance.Enemies)
        {
            if (enemy == null)
                KillUs.Add(enemy);
        }

        foreach (GameObject enemy in KillUs)
            Game.Instance.Enemies.Remove(enemy);

        // Next Block //
        if (Skip || (Game.Instance.Enemies.Count == 0 && Time.timeSinceLevelLoad - StartTime > Duration))
        {         
            int index =  Random.Range(0, NextBlocks.Count - 1);
            GameBlock NextBlock = NextBlocks[index];
            Game.Instance.SetBlock(NextBlock);
            Skip = false;

            foreach (GameObject enemy in Game.Instance.Enemies)
            {
                Destroy(enemy);
            }
            Game.Instance.Enemies.Clear();
        }
    }
}
