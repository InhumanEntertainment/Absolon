using UnityEngine;
using System.Collections;

public class fly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    bool go = false;
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            Animator anim = GetComponent<Animator>();

            if(go)
            {
                anim.SetTrigger("Fly");
            }
            else
            {
                 anim.SetTrigger("Idle");
            }
            go = !go;
        }
	}
}
