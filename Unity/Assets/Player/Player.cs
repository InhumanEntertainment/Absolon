using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;
    public Weapon Weapon;

	public GameObject Chasers;
	public GameObject Chargers;
	bool mode = false;

    public Vector3 TouchOffset;

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

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Chasers.SetActive(mode);
			Chargers.SetActive(!mode);

			mode = !mode;
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
}
