using UnityEngine;
using System.Collections;

public class WeaponUber : Weapon
{
    //=======================================================================================================================================================/
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.DrawLine(player.transform.localPosition, transform.position, Color.red);

        Fire();
    }

    //=======================================================================================================================================================/
    public override void CreateProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (player.transform.position - transform.position).normalized;

        GameObject projectile1 = (GameObject)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody2D.velocity = dir * Velocity;            
    }
}
