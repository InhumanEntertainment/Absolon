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
    
    public bool TransitionEnabled = false;
    public Vector2 TransitionPosition;
    public Vector2 TransitionScale = new Vector2(1, 1);
    public float TransitionDuration = 2;
    public bool TransitionHorizontalLock = false;

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
            Vector3 start = GetStartPosition();          
            GameObject obj = (GameObject)Game.Spawn(Object, start, Quaternion.identity);

            if (TransitionEnabled)
            {
                Movement mover = obj.GetComponent<Movement>();
                if (mover != null)
                {
                    mover.TransitionEnabled = true;
                    mover.Mode = Movement.TweenMode.FastOut;
                    mover.Duration = TransitionDuration;                  
                    Vector3 end = GetEndPosition();
                    if (TransitionHorizontalLock)
                        end.x = start.x;
                    mover.EndPosition = end;                      
                }
            }

            if (MustKill)
            {
                Game.Instance.Enemies.Add(obj);
                //print("Must Kill:" + obj.name);
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
    public Vector3 GetEndPosition()
    {
        double x = Random.value * TransitionScale.x - TransitionScale.x * 0.5f + TransitionPosition.x + transform.position.x;
        double y = Random.value * TransitionScale.y - TransitionScale.y * 0.5f + TransitionPosition.y + transform.position.y;

        return new Vector3((float)x, (float)y, 0);
    }

    //=======================================================================================================================================================/
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        if (TransitionEnabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector3)TransitionPosition + transform.position, TransitionScale);
        }
    }
}
