using UnityEngine;
using System.Collections;

public class Shooter : Enemy
{
    public Vector3 TargetPosition;
    public float Speed = 2;
    public Vector3 Direction;

    //=======================================================================================================================================================/
    void Update()
    {
        GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");
        transform.position -= Direction * Speed * Time.deltaTime;

        // Set Rotation to aim at player //
        Direction = (transform.position - player.transform.position).normalized;
        float r = (float)Mathf.Atan2(Direction.y, Direction.x);
        transform.localRotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + 90, Vector3.forward);
    }
}

