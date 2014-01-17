using UnityEngine;
using System.Collections;

public class Nano : Enemy 
{
    // State //
    public enum NanoState {Idle, Firing, Moving};
    public NanoState State = NanoState.Idle;

    // Movement //
    public Vector3 Acceleration;
    public float MaxAcceleration = 1;
    public float MaxVelocity = 1;
    public float RandomAcceleration = 0.1f;

    public float PlayerFollow = 0.1f;
	
    //=======================================================================================================================================================/
    void Start()
    {
    }

    //=======================================================================================================================================================/
	void FixedUpdate () 
	{
        base.FixedUpdate();

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 dir = new Vector3(Random.value * RandomAcceleration * 2 - RandomAcceleration, Random.value * RandomAcceleration * 2 - RandomAcceleration, 0) * Time.deltaTime;
        Vector3 player_dir = (player.position - transform.position).normalized * PlayerFollow * Time.deltaTime;
        Acceleration += dir + player_dir;

        if (Acceleration.magnitude > MaxAcceleration)
        {
            Acceleration = Acceleration.normalized * MaxAcceleration;
        }

        rigidbody2D.velocity += new Vector2(Acceleration.x, Acceleration.y);

        if (rigidbody2D.velocity.magnitude> MaxVelocity)
        {
            float magnitude = rigidbody2D.velocity.magnitude;
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxVelocity;
        }

        var r = (float)Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
        transform.rotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + 90, Vector3.forward);
	}

    //=======================================================================================================================================================/
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Acceleration * 1);
    }
}

