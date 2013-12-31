using UnityEngine;
using System.Collections;

public class Swarm : MonoBehaviour 
{
	public float Speed = 1f;
	public float Radius = 2.0f;
	public GameObject Prefab;

	//=======================================================================================================================================================/
	void Update () 
	{
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");

		Vector3 dir = (player.transform.position - transform.position).normalized;
		float distance = Vector3.Distance (transform.position, player.transform.position);

		float strength = Mathf.Pow((1 - Mathf.Clamp (distance / Radius, 0, 1)), 3f) * Speed * Time.deltaTime;
		transform.position += dir * strength;
	}

	//=======================================================================================================================================================/
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{

			Destroy(this.gameObject);
		}    
	}
}
