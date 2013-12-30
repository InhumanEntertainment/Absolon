using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour 
{
	public float StartTime;
	public float Speed = 2;
	public GameObject Gem;
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
		//transform.position = GetStartPosition();
		StartTime = Time.time;
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
		transform.position += Vector3.down * Speed * Time.deltaTime;
        transform.Rotate(Vector3.forward, Time.deltaTime * RotationSpeed);

        Fire();
	}
	
	//=======================================================================================================================================================/
	public Vector3 GetStartPosition()
	{
		float x = Screen.width * Random.value;
		float y = Screen.height * Random.value;
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (x, Screen.height, 0));
		pos.z = 0;
		
		return pos;
	}
	
	//=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (this.gameObject);
	}
	
	//=======================================================================================================================================================/
	void OnTriggerEnter2D(Collider2D collider)
	{
		// Play Effect //
		for(int i=0; i<4; i++)
		{
			Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
			GameObject.Instantiate(Gem, pos, Quaternion.identity);
		}
		Destroy(this.gameObject);
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
            Projectile projectile1 = (Projectile)GameObject.Instantiate(Projectile, transform.position, Quaternion.identity);
            Vector3 dir = SpawnOffsets[i].normalized;
            Vector3 vel = transform.localRotation * dir;
            projectile1.rigidbody2D.velocity = vel * Velocity;

            
            Vector3 offset = transform.localRotation * SpawnOffsets[i];
            projectile1.transform.localPosition = transform.position + offset;
        }
    }
}

