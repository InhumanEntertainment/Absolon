using UnityEngine;
using System.Collections;

public class PlayerTest : MonoBehaviour 
{
	public float PivotRadius = 0.4f;
	Vector3 PivotPosition = Vector3.zero;
	Vector3 CurrentPosition = Vector3.zero;

	public GameObject Projectile;
	float LastFire;
	public float FireTime = 0.2f;

	//======================================================================================================================================//
	void Update() 
	{
		// Mouse Down //
		if (Input.GetMouseButton(0))
		{    
			CurrentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CurrentPosition.z = 0;

			float distance = Vector3.Distance(CurrentPosition, PivotPosition);
			Vector3 direction = (CurrentPosition - PivotPosition).normalized;

			if(distance > PivotRadius)
			{
				PivotPosition = CurrentPosition - direction * PivotRadius;
			}

			transform.position = PivotPosition;

			float r = (float)Mathf.Atan2(direction.y, direction.x);		
			transform.localRotation = Quaternion.AngleAxis(r * Mathf.Rad2Deg - 90, Vector3.forward);

			// Fire Weapon //
			if(Time.time - LastFire > FireTime)
			{
				LastFire = Time.time;
				GameObject projectile = (GameObject)Game.Spawn(Projectile, PivotPosition, Quaternion.identity);
				projectile.rigidbody2D.velocity = new Vector2(direction.x, direction.y) * 3f;
			}
		}
	}

	//======================================================================================================================================//
	void OnDrawGizmos() 
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(PivotPosition, PivotRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(CurrentPosition, 0.1f);
	}

}
