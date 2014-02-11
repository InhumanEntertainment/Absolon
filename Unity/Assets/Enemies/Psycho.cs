using UnityEngine;
using System.Collections;

public class Psycho : Enemy 
{
    // State //
    public enum PsychoState {Idle, Firing, Moving};
    public PsychoState State = PsychoState.Idle;

    // Weapon //
    public GameObject Laser;
    Vector3 LaserOffset;
    public float FireDuration = 4;
    public float FireTimeMin = 1;
    public float FireTimeMax = 2;
    float NextFireTime = 0;
    float LastFireTime = 0;
	
    //=======================================================================================================================================================/
    void Start()
    {
        Laser.SetActive(false);
        LaserOffset = Laser.transform.localPosition;
        NextFireTime = Time.timeSinceLevelLoad + Mathf.Lerp(FireTimeMin, FireTimeMax, Random.value);
    }

	//=======================================================================================================================================================/
    void Update()
    {
        Laser.transform.position = transform.position + LaserOffset;

        // Get Laser //
        switch (State)
	    {
            case PsychoState.Idle:
                if(Time.timeSinceLevelLoad > NextFireTime)
                {
                    Laser.SetActive(true);
                    State = PsychoState.Firing;
                    LastFireTime = Time.timeSinceLevelLoad;
                    NextFireTime = Time.timeSinceLevelLoad + FireDuration + Mathf.Lerp(FireTimeMin, FireTimeMax, Random.value); 
                }
                break;

            case PsychoState.Firing:
                if(Time.timeSinceLevelLoad > LastFireTime + FireDuration)
                {
                    Laser.SetActive(false);
                    State = PsychoState.Idle;
                }

                break;

            case PsychoState.Moving:
                break;
	    }
    }
}

