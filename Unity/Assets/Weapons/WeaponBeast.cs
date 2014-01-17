using UnityEngine;
using System.Collections;

public class WeaponBeast : Weapon
{
    //=======================================================================================================================================================/
    void Update()
    {
        Fire();
    }

    //=======================================================================================================================================================/
    /*public override void CreateProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (player.transform.position - transform.position).normalized;

        Projectile projectile1 = (Projectile)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody2D.velocity = dir * Velocity;            
    }*/
}
