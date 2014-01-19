using UnityEngine;
using System.Collections;

public class Shooter : Enemy
{
    public Vector3 TargetPosition;
    public Vector2 Speed = new Vector2(1, 1.2f);
    public Vector3 Direction;
    public Vector3 PullForceMin = Vector3.zero;
    public Vector3 PullForceMax = Vector3.zero;

    //=======================================================================================================================================================/
    void Awake()
    {
        Speed.x = Mathf.Lerp(Speed.x, Speed.y, Random.value);
        PullForceMin = Vector3.Lerp(PullForceMin, PullForceMax, Random.value);
    }

    //=======================================================================================================================================================/
    void Update()
    {
        GameObject player = Game.Instance.Player.gameObject;
        
        // Set Rotation to aim at player //
        Direction = (transform.position - player.transform.position);
        Direction.Normalize();
        Direction += PullForceMin;
        Direction.Normalize();

        transform.position -= Direction * Speed.x * Time.deltaTime;
        float r = (float)Mathf.Atan2(Direction.y, Direction.x);
        transform.localRotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + 90, Vector3.forward);
    }

    //=======================================================================================================================================================/
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, Game.Instance.Player.transform.position);
    }*/
}

