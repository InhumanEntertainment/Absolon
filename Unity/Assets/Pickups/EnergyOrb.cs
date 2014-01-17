using UnityEngine;
using System.Collections;

public class EnergyOrb : Enemy 
{
    public Vector3 SpawnSpeed;

    //============================================================================================================================================//
    void Update()
    {
	
	}

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            ApplyDamage(10);

            // Play Effect //

            Buzzer buzzer = (Game.Spawn(SpawnObject, collision.contacts[0].point) as GameObject).GetComponent<Buzzer>();
            buzzer.Velocity = new Vector3(SpawnSpeed.x * Random.value - (SpawnSpeed.x / 2f), SpawnSpeed.y * Random.value - (SpawnSpeed.y / 2f), 0);

        }
    }
}


