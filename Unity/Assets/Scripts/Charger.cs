using UnityEngine;
using System.Collections;

public class Charger : MonoBehaviour 
{
	public enum ChargerState {Idle, Waiting, Charging};

	public ChargerState State = ChargerState.Idle;
	public float WaitTime = 2;
	public float IdleTime = 1;
	public float StartTime;
	public Vector3 TargetPosition;
	public float Speed = 2;

	public Vector3 Direction;
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
		float time = Time.time - StartTime;
		Player player = (Player)GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		if(State == ChargerState.Idle)
		{
			if(time > IdleTime)
			{
				State = ChargerState.Waiting;
				StartTime = Time.time;
			}
		}
		else if (State == ChargerState.Waiting)
		{
			if(time > WaitTime)
			{
				State = ChargerState.Charging;
				StartTime = Time.time;
			}

			// Set Rotation to aim at player //
			Direction = (transform.position - player.transform.position).normalized;
			float r = (float)Mathf.Atan2(Direction.y, Direction.x);		
			transform.localRotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg + 90, Vector3.forward);

		}
		else if (State == ChargerState.Charging)
		{
			transform.position -= Direction * Speed * Time.deltaTime;
		}
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
		// Play Effect //
		for(int i=0; i<10; i++)
		{
			Vector3 pos = transform.position + new Vector3(Random.value * 0.4f - 0.2f, Random.value * 0.4f - 0.2f, 0);
			GameObject.Instantiate(Gem, pos, Quaternion.identity);
		}
		Destroy(this.gameObject);
	}
}
