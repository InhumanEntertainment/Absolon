using UnityEngine;
using System.Collections;

public class Charger : Enemy 
{
	public enum ChargerState {Idle, Waiting, Charging};	
	public ChargerState State = ChargerState.Idle;

	public float WaitTime = 2;
	public float IdleTime = 1;
	public float StartTime;

	public Vector3 TargetPosition;
	public float Speed = 2;
	public Vector3 Direction;
	
	//=======================================================================================================================================================/
	void Start () 
	{
		StartTime = Time.time;
	}
	
	//=======================================================================================================================================================/
	void Update () 
	{
		float time = Time.time - StartTime;
		GameObject player = (GameObject)GameObject.FindGameObjectWithTag("Player");

        Player playera = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!playera.isAlive)
        {
            State = ChargerState.Waiting;
            StartTime = Time.time;
        }


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
}

