using UnityEngine;
using System.Collections;

public class PsychoLaser : MonoBehaviour 
{
    //======================================================================================================================================//
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player.Instance.Kill();
        }
    }
}
