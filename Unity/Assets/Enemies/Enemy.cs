using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public int Health = 100;
    int StartHealth = 100;
    public int Points = 100;

    public ParticleSystem DeathEffect;
    public GameObject SpawnObject;
    public int SpawnCount = 1;

    public Transform HealthObject;
    public int HealthWidth = 20;

    Color DamageColor;
    Color PreviousDamageColor;
    bool KillMe = false;

    //======================================================================================================================================//
    void Awake()
    {
        StartHealth = Health;
    }
    
    //======================================================================================================================================//
    public void FixedUpdate()
    {
        if(PreviousDamageColor == Color.red)
            DamageColor = Color.white;

        (renderer as SpriteRenderer).color = DamageColor;
        PreviousDamageColor = DamageColor;
        DamageColor = Color.white;
    }

    //======================================================================================================================================//
    public void LateUpdate()
    {
        if (KillMe)
        {
            DestroyImmediate(gameObject);
        }
    }
    
    //======================================================================================================================================//
    public void ApplyDamage(int amount)
    {
        Health -= amount;
        //print("Damage: " + Health);
        DamageColor = Color.red;
        if(HealthObject != null)
            HealthObject.localScale = new Vector3(Health / (float)StartHealth * HealthWidth, 1, 1);

        if(Health <= 0)
        {
            Health = 0;
            Game.Instance.AddScore(Points);
            Kill();
        }
    }

    //======================================================================================================================================//
    void Kill()
    {
        if(DeathEffect)
            Game.Spawn(DeathEffect, transform.position);

        if (SpawnObject)
        for (int i = 0; i < SpawnCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
            GameObject obj = (GameObject)Game.Spawn(SpawnObject, pos, Quaternion.identity);
        }

        KillMe = true;
    }

    //=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy(gameObject);
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            ApplyDamage(1000);
        }
    }
}
