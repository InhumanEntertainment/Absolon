using UnityEngine;
using System.Collections;

public class WeaponUber : Weapon
{
    //=======================================================================================================================================================/
    void Update()
    {
        Debug.DrawLine(Player.Instance.transform.localPosition, transform.position, Color.red);
        Fire();
    }

    //=======================================================================================================================================================/
    public override void CreateProjectile()
    {
        Vector3 dir = (Player.Instance.transform.position - transform.position).normalized;
        GameObject projectile1 = (GameObject)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody2D.velocity = dir * Velocity;            
    }
}
