using UnityEngine;
using System.Collections;

public class Reticule : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    // Find closest enemy //
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortest = 2000;
        Vector3 closestEnemy = player.transform.localPosition;
        foreach(GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.transform.localPosition, enemy.transform.localPosition);
            if (distance < shortest)
            {
                shortest = distance;
                closestEnemy = enemy.transform.localPosition;
            }
        }
        transform.localPosition = closestEnemy;
        Debug.DrawLine(player.transform.localPosition, closestEnemy, Color.cyan);

	}
}
