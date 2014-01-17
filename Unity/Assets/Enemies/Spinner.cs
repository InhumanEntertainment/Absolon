using UnityEngine;
using System.Collections;

public class Spinner : Enemy 
{
	public float StartTime;
	public float Speed = 2;
    public Projectile Projectile;
    public Vector3[] SpawnOffsets; 
    public float RotationSpeed = 100;
    public float FireDelay = 0.5f;
    public float Velocity = 1f;
    public Vector3 Offset = Vector3.zero;
    float LastFireTime;
	
	//=======================================================================================================================================================/
	void Start () 
	{
		StartTime = Time.time;
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
		transform.position += Vector3.down * Speed * Time.deltaTime;
        transform.Rotate(Vector3.forward, Time.deltaTime * RotationSpeed);

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(player.isAlive)
            Fire();
	}
	
    //=======================================================================================================================================================/
    public void Fire()
    {
        float i = Time.timeSinceLevelLoad - LastFireTime;

        if (LastFireTime != null && i > FireDelay)
        {
            CreateProjectile();

            LastFireTime = Time.timeSinceLevelLoad;
        }
    }

    //=======================================================================================================================================================/
    public void CreateProjectile()
    {
        for(int i=0; i < SpawnOffsets.Length; i++)
        {
            Projectile projectile1 = (Projectile)Game.Spawn(Projectile, transform.position, Quaternion.identity);
            Vector3 dir = SpawnOffsets[i].normalized;
            Vector3 vel = transform.localRotation * dir;
            projectile1.rigidbody2D.velocity = vel * Velocity;

            
            Vector3 offset = transform.localRotation * SpawnOffsets[i];
            projectile1.transform.localPosition = transform.position + offset;
        }
    }
}

