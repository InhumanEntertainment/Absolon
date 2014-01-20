using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour 
{
    public float XSpeed = 1f;
    public float YSpeed = 1f;
	public GameObject Gem;

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
	void OnTriggerEnter2D(Collider2D collider)
	{
		// Play Effect //
		for(int i=0; i<10; i++)
		{
			Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
            Game.Spawn(Gem, pos, Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
}
