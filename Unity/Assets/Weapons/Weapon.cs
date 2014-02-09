using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject Projectile;
    public float FireStartDelay;
    public float FireDelay = 0.1f;
    public Vector2 FireDelayMinMax = new Vector2(0.1f, 0.1f);
    public Vector2 VelocityMinMax = new Vector2(1, 1);
    public Vector3 Offset = Vector3.zero;

    float LastFireTime;
    
    //=======================================================================================================================================================/
    void Start()
    {
        LastFireTime = Time.timeSinceLevelLoad + FireStartDelay;
	}

    //=======================================================================================================================================================/
    public virtual void Fire()
    {
        float i = Time.timeSinceLevelLoad - LastFireTime;

        FireDelay = Mathf.Lerp(FireDelayMinMax.x, FireDelayMinMax.y, Game.Instance.Difficulty);
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
        float velocity = Mathf.Lerp(VelocityMinMax.x, VelocityMinMax.y, Game.Instance.Difficulty);
        projectile1.rigidbody2D.velocity = new Vector2(0, velocity);

    }
}
