using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;
    public Weapon Weapon;
    public Bomb Bomb;
    public Vector3 TouchOffset;
    public ParticleSystem DeathEffect;

    float DeathTimeout = 2;
    float DeathStart;
    public bool isAlive = true;

    public float TapTime;
    public int TapCount = 0;
    float TapDuration = 0.2f;

    //======================================================================================================================================//
    void Awake()
    {
        Instance = this;
	}

    //======================================================================================================================================//
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position - TouchOffset, transform.position);
        Gizmos.DrawWireSphere(transform.position - TouchOffset, 0.05f);
    }

    //======================================================================================================================================//
    void Update()
    {
        // Mouse Down //
        if (Input.GetMouseButton(0))
        {
            Vector3 Mouse = GetMousePosition();
            Mouse.z = 0;

            if (Input.GetMouseButtonDown(0))
            {
                TouchOffset = transform.position - Mouse;

                if (Time.realtimeSinceStartup - TapTime < TapDuration)
                {
                    TapCount++;
                    if(TapCount == 1)
                    {
                        Game.Spawn(Bomb, transform.position);
                        TapCount = 0;
                        TapTime = 0;
                    }
                }

                TapTime = Time.realtimeSinceStartup;
                
            }

            transform.position = Mouse + TouchOffset;
            Weapon.transform.position = transform.position + Weapon.Offset;
            
            if(isAlive)
                Weapon.Fire();
        }

        if(isAlive)
        {
            
        }
        else
        {
            float amount = Time.timeSinceLevelLoad - DeathStart;
            if (Input.GetMouseButtonDown(0))
                amount = DeathTimeout;

            if (amount < DeathTimeout)
            {
                bool value = (amount * 4 % 1) > 0.5f;
                (renderer as SpriteRenderer).color = value ? Color.red : new Color(1, 0, 0, 0.1f);
            }
            else
            {
                isAlive = true;
                (renderer as SpriteRenderer).color = Color.white;
            }       
        }
	}

    //======================================================================================================================================//
    Vector3 GetMousePosition()
    {
        Vector3 vec = Vector3.zero;
        vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;

        return vec;
    }

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy" && isAlive)
        {
            //Destroy(this.gameObject);
            // Play Effect //
            if (DeathEffect)
                Game.Spawn(DeathEffect, transform.position);

            isAlive = false;
            DeathStart = Time.timeSinceLevelLoad;

            // Reset Weapon //
            Destroy(Weapon.gameObject);
            Weapon = (Weapon)Instantiate(Game.Instance.Weapons[0]);
            
            Game.Instance.Death();
        }
    }
}
