using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{  
    // State //
    public enum MovementState {Transition, Move};
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

    
    // Bounds //
    public bool BoundsEnabled = false;
    public Vector2 BoundsMin;
    public Vector2 BoundsMax;
    public float RandomDriftRadius = 1;

    // Rotation //
    public bool RotationToVelocity = false;
    public float RotationOffset = 0;

    // Transition //
    public bool TransitionEnabled = false;
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float Value = 0;
    public float Duration = 1;
    public float Delay = 0;

    float LastTime = 0F;
    float CurrentTime = 0F;
    float DeltaTime = 0F;
    float StartTime;

    public enum TweenMode { Linear, Smooth, FastOut, SlowOut, Berp };
    public TweenMode Mode = TweenMode.Smooth;
    bool isPlaying = false;

    //=================================================================================================================//
    void Start()
    {
        StartPosition = transform.position;
        rigidbody2D.velocity = new Vector2(Mathf.Lerp(StartVelocityMin.x, StartVelocityMax.x, Random.value), Mathf.Lerp(StartVelocityMin.y, StartVelocityMax.y, Random.value));

        // Transition //
        StartTime = Time.timeSinceLevelLoad;
        if(TransitionEnabled)
            PlayTransition();
	}


    //=================================================================================================================//
    void FixedUpdate()
    {
	    if(State == MovementState.Transition)
		{
            CurrentTime = Time.realtimeSinceStartup;
            DeltaTime = CurrentTime - LastTime;
            LastTime = CurrentTime;

            //Value = (Time.realtimeSinceStartup / 1 % 3) - 1;
            //Value = (Time.realtimeSinceStartup - Delay) / Duration % 1;

            if (isPlaying)
            {
                Value = (Time.timeSinceLevelLoad - Delay - StartTime) / Duration;
                Value = Mathf.Clamp(Value, 0, 1);
                float ease = Ease(Value);
                Vector3 newPosition = StartPosition * (1 - ease) + EndPosition * ease;
                transform.position = newPosition;

                if (Time.timeSinceLevelLoad - StartTime > Delay + Duration)
                {
                    isPlaying = false;
                    transform.position = EndPosition;
                    State = MovementState.Move;
                }

                // Rotation //
                if (RotationToVelocity)
                {
                    Vector2 dir = EndPosition - StartPosition;
                    var r = (float)Mathf.Atan2(dir.y, dir.x);
                    transform.rotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + RotationOffset, Vector3.forward);
                }
            }
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
                if (transform.position.x < BoundsMin.x + EndPosition.x)
                    Acceleration.x = Mathf.Abs(Acceleration.x);
                if (transform.position.x > BoundsMax.x + EndPosition.x)
                    Acceleration.x = -Mathf.Abs(Acceleration.x);
                if (transform.position.y < BoundsMin.y + EndPosition.y)
                    Acceleration.y = Mathf.Abs(Acceleration.y);
                if (transform.position.y > BoundsMax.y + EndPosition.y)
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
        Gizmos.color = Color.green;
        Vector3 end = transform.position + (Vector3)rigidbody2D.velocity.normalized * 0.1f;
        Gizmos.DrawLine(transform.position, end);

        //Gizmos.DrawCube();

        // Transtion //
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition, .01f);
        Gizmos.DrawWireSphere(EndPosition, 0.01f);
        Gizmos.DrawLine(transform.localPosition, EndPosition);

        /*Vector3 lastpos = new Vector3(0, Ease(0) * 2);
        for(float i=0; i<=1.02; i+=0.02f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.localPosition, StartPosition, Ease(i));
            Gizmos.DrawWireSphere(newPosition, 0.02f);

            Vector3 newpos = new Vector3(i * 2, Ease(i) * 2);
            Gizmos.DrawLine(newpos, lastpos);
            lastpos = newpos;
        }*/
    }

    //=======================================================================================================================================================/
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //=======================================================================================================================================================/
    public void PlayTransition()
    {
        isPlaying = true;
        State = MovementState.Transition;
        StartTime = Time.timeSinceLevelLoad;
    }

    //=======================================================================================================================================================/
    float Ease(float value)
    {
        float blend;
        if (Mode == TweenMode.Smooth)
            blend = Smooth(value);
        else if (Mode == TweenMode.FastOut)
            blend = FastOut(value);
        else if (Mode == TweenMode.SlowOut)
            blend = SlowOut(value);
        else if (Mode == TweenMode.Berp)
        {
            value = Mathf.Clamp01(value);
            //blend = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            //blend = (Mathf.Sin(value * Mathf.PI) * 1f) + value;
            
            //blend = (Mathf.Cos((value * 360 * 4 + 180) * Mathf.Deg2Rad) * 0.1f) * 0 + Mathf.Pow(value, 0.5f);// * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            blend = Mathfx.Berp(0, 1, value);
        }       
        else
            blend = value;

        return blend;
    }

    //=======================================================================================================================================================/
    float Smooth(float value)
    {
        return Mathf.SmoothStep(0, 1, value);
    }

    //=======================================================================================================================================================/
    float FastOut(float value)
    {
        return Mathf.Pow(value, 0.5f);
    }

    //=======================================================================================================================================================/
    float SlowOut(float value)
    {
        return Mathf.Pow(value, 2f);
    }
}

