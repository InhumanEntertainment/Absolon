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

    public Rect SpawnLocation;


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
        Instantiate(Object, GetStartPosition(), Quaternion.identity);
    }

    //=======================================================================================================================================================/
    public Vector3 GetStartPosition()
    {
        double x = Random.value * (SpawnLocation.width) + SpawnLocation.xMin;
        double y = Random.value * (SpawnLocation.height) + SpawnLocation.yMin;

        return new Vector3((float)x, (float)y, 0);
    }

}
