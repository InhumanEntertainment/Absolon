using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public Weapon Weapon;

    //=======================================================================================================================================================/
    void Update()
    {
	
	}

    //============================================================================================================================================//
    void OnTriggerEnter(Collider collider)
    {
        // Finish Level //
        if (collider.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();

            // Destroy Weapon //
            Destroy(player.Weapon.gameObject);

            player.Weapon = (Weapon)Instantiate(Weapon);
        }
    }

}
