using UnityEngine;
using System.Collections;

public class Buzzer : MonoBehaviour 
{
    Vector3 Velocity;
    Vector3 Acceleration;
    public Vector3 Target;
    Vector3 OldTarget;
    public float TargetRadius = 3;
    public float TargetMinRadius = 1;
    public float TargetStrength = 1;
    public float Drag = 1;
    public Vector3 RandomForce;
    Vector3 Buffer;

    //============================================================================================================================================//
    void Awake()
    {
        Buffer = transform.localPosition;
        OldTarget = Target;

        /*for (int i = 0; i < 600; i++)
        {
            Calculate(0.1f, true);
        }*/
        transform.localPosition = Buffer;
	}

    //============================================================================================================================================//
    void Update()
    {
        GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");
        Target = player.transform.position;
        Calculate(Time.deltaTime, false);
    }

    //============================================================================================================================================//
    void Calculate(float delta, bool buffer)
    {
        Vector3 targetforce = (Target - Buffer);
        
        float strength = 0;
        if (targetforce.magnitude < TargetRadius)
        {
            strength = (TargetRadius - targetforce.magnitude) / TargetRadius;
            strength = Mathf.Pow(strength, 2);
            if (targetforce.magnitude < TargetMinRadius)
                strength = -strength;

            strength *= TargetStrength;
        }
        
        targetforce = targetforce.normalized * strength;
        Vector3 randomforce = new Vector3(Random.value * RandomForce.x - RandomForce.x * 0.5f, Random.value * RandomForce.y - RandomForce.y * 0.5f, 0);

        Acceleration = targetforce + randomforce;

        Velocity += (Acceleration * delta);
        Velocity *= Drag;

        Buffer = new Vector3(Buffer.x + Velocity.x, Buffer.y + Velocity.y, -1);
 
        if (!buffer)
        {
            transform.localPosition = Buffer;
        }    
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
