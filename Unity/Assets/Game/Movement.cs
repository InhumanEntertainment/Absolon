using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{  
    // State //
    public enum MovementState {Setup, Move};
    public MovementState State = MovementState.Move;

    // Player Force //
    public bool PlayerEnabled = false;
    public float PlayerAcceleration;


    // Basic Force //
    public Vector2 StartVelocityMin;
    public Vector2 StartVelocityMax;
    public float Deceleration = 0;

    // Random Force //
    public bool RandomEnabled = false;
    public Vector2 RandomForce;
    public Vector2 RandomVelocity;
    public float RandomAccelerationMax = 1;
    public float VelocityMax = 1;
    public float RandomAcceleration = 0.1f;

    public Vector2 StartPosition;

    // Bounds //
    public bool BoundsEnabled = false;
    public Vector2 BoundsMin;
    public Vector2 BoundsMax;
    public float RandomDriftRadius = 1;

    // Rotation //
    public bool RotationToVelocity = false;
    public float RotationOffset = 0;

    //=================================================================================================================//
    void Start()
    {
        StartPosition = transform.position;
        rigidbody2D.velocity = new Vector2(Mathf.Lerp(StartVelocityMin.x, StartVelocityMax.x, Random.value), Mathf.Lerp(StartVelocityMin.y, StartVelocityMax.y, Random.value));
	}

    //=================================================================================================================//
    void FixedUpdate()
    {
	    if(State == MovementState.Setup)
		{

		}
		else if (State == MovementState.Move)
		{
            Vector2 Acceleration = Vector2.zero;
            // Random //
            if (RandomEnabled)
            {
                Vector2 dir = new Vector3(Random.value * RandomAcceleration * 2 - RandomAcceleration, Random.value * RandomAcceleration * 2 - RandomAcceleration);
                RandomForce = dir.normalized * RandomAcceleration;
                Acceleration += RandomForce;
            }

            // Player //
            if (PlayerEnabled)
            {
                Vector2 PlayerForce = (Player.Instance.transform.position - transform.position).normalized * PlayerAcceleration;
                Acceleration += PlayerForce;
            }

            // Bounds //
            if (BoundsEnabled)
            {
                if (transform.position.x < BoundsMin.x + StartPosition.x)
                    Acceleration.x = Mathf.Abs(Acceleration.x);
                if (transform.position.x > BoundsMax.x + StartPosition.x)
                    Acceleration.x = -Mathf.Abs(Acceleration.x);
                if (transform.position.y < BoundsMin.y + StartPosition.y)
                    Acceleration.y = Mathf.Abs(Acceleration.y);
                if (transform.position.y > BoundsMax.y + StartPosition.y)
                    Acceleration.y = -Mathf.Abs(Acceleration.y);
            }         

            // Deceleration //
            rigidbody2D.velocity += Acceleration * Time.fixedDeltaTime;

            float mag = rigidbody2D.velocity.magnitude;
            Vector2 direction = rigidbody2D.velocity.normalized;
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * Mathf.Max(mag - Deceleration * Time.fixedDeltaTime, 0.4f);

            // Clamping //
            if (rigidbody2D.velocity.magnitude > VelocityMax)
            {
                rigidbody2D.velocity = rigidbody2D.velocity.normalized * VelocityMax;
            }

            // Rotation 
            if (RotationToVelocity)
            {
                var r = (float)Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
                transform.rotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + RotationOffset, Vector3.forward);
            }
		}
	}

    //=======================================================================================================================================================/
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 end = transform.position + (Vector3)rigidbody2D.velocity.normalized * 0.1f;
        Gizmos.DrawLine(transform.position, end);

        //Gizmos.DrawCube();
    }

    //=======================================================================================================================================================/
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
