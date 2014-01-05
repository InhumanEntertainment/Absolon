using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public Weapon Weapon;

    //=======================================================================================================================================================/
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();

            // Destroy Weapon //
            Destroy(player.Weapon.gameObject);
            // Destroy(gameObject)

            player.Weapon = (Weapon)Instantiate(Weapon);
        }
    }
}