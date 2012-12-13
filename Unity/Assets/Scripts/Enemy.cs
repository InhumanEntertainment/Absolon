using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public float XSpeed = 1f;
    public float YSpeed = 1f;

    //=======================================================================================================================================================/
    void Start()
    {
        rigidbody.velocity = new Vector3(0, -YSpeed, 0);
    }

    //=======================================================================================================================================================/
    void FixedUpdate()
    {
        Vector2 vel = Player.Instance.transform.position - transform.position;
        vel.y *= 0.7f;
        vel.Normalize();
        vel = new Vector2(vel.x * XSpeed, 0f);

        rigidbody.velocity = new Vector2(rigidbody.velocity.x * 0.98f, rigidbody.velocity.y) + vel;

        var r = (float)Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x);

        rigidbody.rotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg, Vector3.forward);
        
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter(Collision collision)
    {
        // Play Effect //

        Destroy(this.gameObject);
    }
}
