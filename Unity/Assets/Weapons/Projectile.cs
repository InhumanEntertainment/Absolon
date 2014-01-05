using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public ParticleSystem HitEffect;

    //=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (this.gameObject);
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play Effect //
        if (HitEffect)
            Game.Spawn(HitEffect, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0));

        Destroy(this.gameObject);
    }
}
