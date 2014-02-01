using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public GameObject Object;
    public bool MustKill = false;
    public int MaxCount = 1000;
    int SpawnCount = 0;
    public float StartDelay;

    public Vector2 EasyTimeStep = new Vector2(1, 1);
    public Vector2 HardTimeStep = new Vector2(0.1f, 0.1f);

    float NextSpawnTime;
    public Random Rand;
    bool FirstRun = true;

    //=======================================================================================================================================================/
    void Update()
    {
        if(Game.Instance != null)
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
    }

    //=======================================================================================================================================================/
    float GetRandomTime()
    {
        float MinTimeStep = Mathf.Lerp(EasyTimeStep.x, HardTimeStep.x, Game.Instance.Difficulty);
        float MaxTimeStep = Mathf.Lerp(EasyTimeStep.y, HardTimeStep.y, Game.Instance.Difficulty);

        float t = Random.value * (MaxTimeStep - MinTimeStep) + MinTimeStep;
        return t;
    }

    //=======================================================================================================================================================/
    public virtual void Create()
    {
        SpawnCount++;

        if (SpawnCount > MaxCount)
            Destroy(gameObject);
        else
        {
            GameObject obj = (GameObject)Game.Spawn(Object, GetStartPosition(), Quaternion.identity);
            if (MustKill)
            {
                Game.Instance.Enemies.Add(obj);
                print("Must Kill:" + obj.name);
            }
        }
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
