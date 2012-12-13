using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public Projectile Projectile;
    public float FireDelay = 0.1f;
    public float Velocity = 1f;
    public Vector3 Offset = Vector3.zero;

    float LastFireTime;
    
    //=======================================================================================================================================================/
    void Start()
    {
	
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

            LastFireTime = Time.timeSinceLevelLoad;
        }
    }

    //=======================================================================================================================================================/
    public virtual void CreateProjectile()
    {
        Projectile projectile1 = (Projectile)GameObject.Instantiate(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody.velocity = new Vector3(0, Velocity, 0);

    }
}
