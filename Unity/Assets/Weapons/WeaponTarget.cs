using UnityEngine;
using System.Collections;

public class WeaponTarget : Weapon
{
    public Transform Reticule;

    void Awake()
    {
        Reticule = GameObject.Find("Reticule").transform;
    }

    //=======================================================================================================================================================/
    void Update()
    {
        // Find closest enemy //
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortest = 2000;
        Vector3 closestEnemy = player.transform.localPosition;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < shortest)
            {
                shortest = distance;
                closestEnemy = enemy.transform.position;
            }
        }

        Reticule.position = closestEnemy;
        Debug.DrawLine(player.transform.localPosition, closestEnemy, Color.cyan);
    }

    //=======================================================================================================================================================/
    public override void CreateProjectile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = (Reticule.position - player.transform.position).normalized;

        Projectile projectile1 = (Projectile)Game.Spawn(Projectile, transform.position, Quaternion.identity);
        projectile1.rigidbody2D.velocity = dir * Velocity;            
    }
}
