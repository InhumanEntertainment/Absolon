using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject Projectile;
    public float FireStartDelay;
    public float FireDelay = 0.1f;
    public float Velocity = 1f;
    public Vector3 Offset = Vector3.zero;

    float LastFireTime;
    
    //=======================================================================================================================================================/
    void Start()
    {
        LastFireTime = Time.timeSinceLevelLoad + FireStartDelay;
	}

    //=======================================================================================================================================================/
    void Update()
    {
	
	}

    //=======================================================================================================================================================/
    public virtual void Fire()
    {
        float i = Time.timeSinceLevelLoad - LastFireTime;

        if (LastFireTime != null && i > FireDelay)
        {
            CreateProjectile();

            LastFireTime = Time.timeSinceLevelLoad + (FireDelay - i);
        }
    }

    //=======================================================================================================================================================/
    public virtual void CreateProjectile()
    {
        GameObject projectile1 = (GameObject)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody2D.velocity = new Vector2(0, Velocity);

    }
}
