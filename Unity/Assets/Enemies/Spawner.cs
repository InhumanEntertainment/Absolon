using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public GameObject Object;
    public int NumberofEntities;
    public float MaxTimeStep;
    public float MinTimeStep;
    public float StartDelay;

    float NextSpawnTime;
    public Random Rand;
    bool FirstRun = true;

    //=======================================================================================================================================================/
    void Start()
    {
    }

    //=======================================================================================================================================================/
    void Update()
    {
        if (FirstRun)
        {
            NextSpawnTime = Time.timeSinceLevelLoad + StartDelay + GetRandomTime();
            FirstRun = false;
        }
        else
        {
            if (Time.timeSinceLevelLoad > NextSpawnTime)
            {
                NextSpawnTime = Time.timeSinceLevelLoad + GetRandomTime();
                Create();
            }
        }
    }

    //=======================================================================================================================================================/
    float GetRandomTime()
    {
        float t = Random.value * (MaxTimeStep - MinTimeStep) + MinTimeStep;
        return t;
    }

    //=======================================================================================================================================================/
    public virtual void Create()
    {
        Game.Spawn(Object, GetStartPosition(), Quaternion.identity);
    }

    //=======================================================================================================================================================/
    public Vector3 GetStartPosition()
    {
        double x = Random.value * transform.localScale.x - transform.localScale.x * 0.5f + transform.position.x;
        double y = Random.value * transform.localScale.y - transform.localScale.y * 0.5f + transform.position.y;

        return new Vector3((float)x, (float)y, 0);
    }

    //=======================================================================================================================================================/
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
