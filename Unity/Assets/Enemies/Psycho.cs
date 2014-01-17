using UnityEngine;
using System.Collections;

public class Psycho : Enemy 
{
    // State //
    public enum PsychoState {Idle, Firing, Moving};
    public PsychoState State = PsychoState.Idle;

    // Movement //
    public Vector3 Acceleration;
    public float MaxAcceleration = 1;
    public float MaxVelocity = 1;
    public float RandomAcceleration = 0.1f;
    public float DriftRadius = 1;
    public Vector3 StartPosition;

    // Weapon //
    public float FireDuration = 4;
    public float FireTimeMin = 1;
    public float FireTimeMax = 2;
    float NextFireTime = 0;
    float LastFireTime = 0;
	
    //=======================================================================================================================================================/
    void Start()
    {
        GameObject laser = transform.GetChild(0).gameObject;
        laser.SetActive(false);
        StartPosition = transform.position;
        NextFireTime = Time.timeSinceLevelLoad + Mathf.Lerp(FireTimeMin, FireTimeMax, Random.value);
    }

	//=======================================================================================================================================================/
    void Update()
    {
        // Get Laser //
        switch (State)
	    {
            case PsychoState.Idle:
                if(Time.timeSinceLevelLoad > NextFireTime)
                {
                    GameObject laser = transform.GetChild(0).gameObject;
                    laser.SetActive(true);
                    State = PsychoState.Firing;
                    LastFireTime = Time.timeSinceLevelLoad;
                    NextFireTime = Time.timeSinceLevelLoad + FireDuration + Mathf.Lerp(FireTimeMin, FireTimeMax, Random.value); 
                }
                break;

            case PsychoState.Firing:
                if(Time.timeSinceLevelLoad > LastFireTime + FireDuration)
                {
                    GameObject laser = transform.GetChild(0).gameObject;
                    laser.SetActive(false);
                    State = PsychoState.Idle;
                }

                break;

            case PsychoState.Moving:
                break;
	    }
    }

    //=======================================================================================================================================================/
	void FixedUpdate () 
	{
        base.FixedUpdate();

        Vector3 dir = new Vector3(Random.value * RandomAcceleration * 2 - RandomAcceleration, Random.value * RandomAcceleration * 2 - RandomAcceleration, 0) * Time.deltaTime;
        Acceleration += dir;

        if (Acceleration.magnitude > MaxAcceleration)
        {
            Acceleration = Acceleration.normalized * MaxAcceleration;
        }

        if (Vector3.Distance(transform.position, StartPosition) > DriftRadius)
        {
            float magnitude = Acceleration.magnitude;
            Acceleration = (StartPosition - transform.position).normalized * magnitude;
        }

        rigidbody2D.velocity += new Vector2(Acceleration.x, Acceleration.y);

        if (rigidbody2D.velocity.magnitude> MaxVelocity)
        {
            float magnitude = rigidbody2D.velocity.magnitude;
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxVelocity;
        }
	}

    //=======================================================================================================================================================/
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartPosition, DriftRadius);
        Gizmos.DrawLine(transform.position, transform.position + Acceleration * 50);
    }
}

