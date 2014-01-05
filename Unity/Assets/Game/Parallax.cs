using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour 
{
    public float Multiplier = 1;

    //=======================================================================================================================================================/
    void Start()
    {
	
	}

    //=======================================================================================================================================================/
    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        transform.position = new Vector3(0, mouse.y * Multiplier, 0);
	
	}
}
