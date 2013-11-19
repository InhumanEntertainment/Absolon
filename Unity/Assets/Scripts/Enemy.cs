using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public float XSpeed = 1f;
    public float YSpeed = 1f;

    //=======================================================================================================================================================/
    void Start()
    {
		rigidbody2D.velocity = new Vector3(0, -YSpeed, 0);
    }

	//=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (this.gameObject);
	}

    //=======================================================================================================================================================/
    void FixedUpdate()
    {
        Vector2 vel = Player.Instance.transform.position - transform.position;
        vel.y *= 0.7f;
        vel.Normalize();
        vel = new Vector2(vel.x * XSpeed, 0f);

		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * 0.98f, rigidbody2D.velocity.y) + vel;

		var r = (float)Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);

		transform.rotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg, Vector3.forward);
        
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play Effect //

        Destroy(this.gameObject);
    }
}
