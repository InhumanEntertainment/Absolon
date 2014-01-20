using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour 
{
    public float Speed = 1;
    public int Damage = 200;

    public float StartTime;
    public float MaxRadius = 2;

    List<GameObject> DamagedObjects = new List<GameObject>();

    //=======================================================================================================================================================/
    void Awake()
    {
        StartTime = Time.timeSinceLevelLoad;

        Player player = FindObjectOfType<Player>();
        transform.position = player.transform.position;

        Audio.PlaySound("Bomb");
    }

    //=======================================================================================================================================================/
    void Update()
    {
        float time = Time.timeSinceLevelLoad - StartTime;
        float radius = time * Speed;

        transform.localScale = Vector3.one * radius  * Mathf.PI;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - Mathf.Clamp01(radius / MaxRadius));

        if (radius > MaxRadius)
        {
            Destroy(gameObject);
            return;
        }

        /*GameObject group = GameObject.Find("Objects");
        for(int i=0; i < group.transform.childCount; i++)
        {
            group.transform.GetChild(i);
            if()
        }*/

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance - 0.1 < radius && !DamagedObjects.Contains(enemy.gameObject))
            {
                enemy.ApplyDamage(Damage);
                DamagedObjects.Add(enemy.gameObject);
            }
        }

        Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            float distance = Vector3.Distance(projectile.transform.position, transform.position);
            if (distance - 0.1 < radius && !DamagedObjects.Contains(projectile.gameObject))
            {
                Destroy(projectile.gameObject);
                DamagedObjects.Add(projectile.gameObject);
            }
        }
	}

    //=======================================================================================================================================================/
    void OnDrawGizmos()
    {
        float time = Time.timeSinceLevelLoad - StartTime;
        float radius = time * Speed;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
