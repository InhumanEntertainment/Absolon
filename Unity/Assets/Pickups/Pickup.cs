using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public Weapon Weapon;
    float StartTime;
    public SpriteRenderer FlashObject;

    //=======================================================================================================================================================/
    void Start()
    {
        StartTime = Random.value * Mathf.PI + Time.timeSinceLevelLoad;
    }

    //=======================================================================================================================================================/
    void Update()
    {
        if (FlashObject != null)
        {
            float t = Time.timeSinceLevelLoad - StartTime;

            float val = Mathf.Lerp(0.1f, 1f, Mathf.Cos(t * 12) * 0.5f + 0.5f);
            FlashObject.color = new Color(val, val, val);
        } 
    }

    //=======================================================================================================================================================/
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();

            // Destroy Weapon //
            Destroy(player.Weapon.gameObject);
            Destroy(gameObject);

            player.Weapon = (Weapon)Instantiate(Weapon);
        }
    }
}