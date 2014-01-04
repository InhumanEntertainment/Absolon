using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    //=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (this.gameObject);
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play Effect //
        //if (collider.tag == "Enemy")
        //{
        //Destroy(collider.gameObject);
        Destroy(this.gameObject);
        //} 
    }
}
