using UnityEngine;
using System.Collections;

public class SlowMo : MonoBehaviour 
{
	public float StartTime;
	public float Speed = 2;
	public GameObject Gem;
	
	//=======================================================================================================================================================/
	void Start () 
	{
		transform.position = GetStartPosition();
		StartTime = Time.time;
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
		transform.position += Vector3.down * Speed * Time.deltaTime;
	}
	
	//=======================================================================================================================================================/
	public Vector3 GetStartPosition()
	{
		float x = Screen.width * Random.value;
		float y = Screen.height * Random.value;
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (x, Screen.height, 0));
		pos.z = 0;
		
		return pos;
	}
	
	//=======================================================================================================================================================/
	void OnBecameInvisible() 
	{
		Destroy (this.gameObject);
	}
	
	//=======================================================================================================================================================/
	void OnTriggerEnter2D(Collider2D collider)
	{
		// Play Effect //
		for(int i=0; i<4; i++)
		{
			Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
			GameObject.Instantiate(Gem, pos, Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
}

