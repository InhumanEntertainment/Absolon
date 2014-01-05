using UnityEngine;
using System.Collections;

public class SlowMo : Enemy 
{
	public float Speed = 2;
	
	//=======================================================================================================================================================/
	void Start () 
	{
		transform.position = GetStartPosition();
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
        base.Update();

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
}

