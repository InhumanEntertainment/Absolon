using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;
    public Weapon Weapon;
    public Vector3 TouchOffset;

    float DeathTimeout = 2;
    float DeathStart;
    public bool isAlive = true;

    //======================================================================================================================================//
    void Awake()
    {
        Instance = this;
	}

    //======================================================================================================================================//
    void Update()
    {
        Weapon.transform.position = transform.position + Weapon.Offset;

        // Mouse Down //
        if (Input.GetMouseButton(0))
        {
            Vector3 Mouse = GetMousePosition();

            transform.position = Mouse + TouchOffset;
            Weapon.transform.position = transform.position + Weapon.Offset;

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
                (renderer as SpriteRenderer).color = Color.red;
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

            isAlive = false;
            DeathStart = Time.timeSinceLevelLoad;
            Game.Instance.Death();
        }
    }
}
