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
        Destroy(this.gameObject);

        if(collision.collider != null)
        {
            collision.collider.SendMessage("ApplyDamage", 10, SendMessageOptions.DontRequireReceiver);
            if (HitEffect)
                Game.Spawn(HitEffect, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0));
        }
    }
}
