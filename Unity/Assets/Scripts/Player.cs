using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;
    public Weapon Weapon;

	public GameObject Chasers;
	public GameObject Chargers;
	bool mode = false;

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

            transform.position = Mouse;
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

        // Get Game Camera //
        Camera cam = Camera.main;
        var w = cam.pixelWidth;
        var h = cam.pixelHeight;

        //if (Input.GetMouseButton(0))
        //{
        vec = new Vector3((Input.mousePosition.x / w - 0.5f) * cam.orthographicSize * cam.aspect * 2, (Input.mousePosition.y / h - 0.5f) * cam.orthographicSize * 2, 0);
        //}

        return vec;
    }
}
