using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public int Health = 100;
    public int Points = 100;

    public ParticleSystem DeathEffect;
    public GameObject SpawnObject;
    public int SpawnCount = 1;

    Color DamageColor;

    //======================================================================================================================================//
    public void Update()
    {
        (renderer as SpriteRenderer).color = DamageColor;
        DamageColor = Color.white;
    }
    
    //======================================================================================================================================//
    public void ApplyDamage(int amount)
    {
        Health -= amount;
        //print("Damage: " + Health);
        DamageColor = Color.red;

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

        Destroy(gameObject);
    }

    //=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (gameObject);
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            ApplyDamage(10);

            // Play Effect //
        }
    }

}
