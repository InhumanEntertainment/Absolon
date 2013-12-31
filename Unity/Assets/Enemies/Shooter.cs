using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	public float StartTime;
	public Vector3 TargetPosition;
	public float Speed = 2;
	
	public Vector3 Direction;
	public GameObject Gem;
	
	//=======================================================================================================================================================/
	void Start () 
	{
		//transform.position = GetStartPosition();
		StartTime = Time.time;
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
		float time = Time.time - StartTime;
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");

        transform.position -= Direction * Speed * Time.deltaTime;
        
        // Set Rotation to aim at player //
		Direction = (transform.position - player.transform.position).normalized;
		float r = (float)Mathf.Atan2(Direction.y, Direction.x);		
		transform.localRotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + 90, Vector3.forward);
			
		
	}
	
	//=======================================================================================================================================================/
	public Vector3 GetStartPosition()
	{
		float x = Screen.width * Random.value;
		float y = Screen.height * Random.value;
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3 (x, y, 0));
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
        GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");
        

		// Play Effect //
		for(int i=0; i<3; i++)
		{
			Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
			GameObject obj = (GameObject)GameObject.Instantiate(Gem, pos, Quaternion.identity);
            //GameObject obj = (GameObject)GameObject.Instantiate(Gem, Vector3.zero, Quaternion.identity);
            //obj.transform.parent = player.transform;

		}
		Destroy(this.gameObject);
	}
}

