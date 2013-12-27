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
    void OnTriggerEnter2D(Collider2D collider)
    {
		// Play Effect //
        /*if (collider.tag == "Enemy")
        {
			Destroy(collider.gameObject);
			Destroy(this.gameObject);
        } */   
    }
}
