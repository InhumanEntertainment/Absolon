using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour 
{
    public Vector3 StartPosition;
    Vector3 EndPosition;
    public float Value = 0;
    public float Duration = 1;
    public float Delay = 0;

    float LastTime = 0F;
    float CurrentTime = 0F;
    float DeltaTime = 0F;
    float StartTime;

    public enum TweenMode {Linear, Smooth, FastOut, SlowOut, Berp};
    public TweenMode Mode = TweenMode.Linear;
    bool isPlaying = false;
    public bool AutoPlay = false;

    //=======================================================================================================================================================/
    void Start()
    {
        StartTime = Time.timeSinceLevelLoad;
        EndPosition = transform.localPosition;

        if (AutoPlay)
            Play();
	}

    //=======================================================================================================================================================/
    public void Play()
    {
        isPlaying = true;
        StartTime = Time.timeSinceLevelLoad;
    }

    //=======================================================================================================================================================/
    void Update()
    {
        CurrentTime = Time.realtimeSinceStartup;
        DeltaTime = CurrentTime - LastTime;
        LastTime = CurrentTime;

        //Value = (Time.realtimeSinceStartup / 1 % 3) - 1;
        //Value = (Time.realtimeSinceStartup - Delay) / Duration % 1;

        if (isPlaying)
        {
            Value = (Time.timeSinceLevelLoad - Delay - StartTime) / Duration;
            Value = Mathf.Clamp(Value, 0, 1);
            float ease = Ease(Value);
            Vector3 newPosition = StartPosition * (1 - ease) + EndPosition * ease;
            transform.localPosition = newPosition;

            if(Time.timeSinceLevelLoad - StartTime > Delay + Duration)
            {
                isPlaying = false;
                transform.localPosition = EndPosition;
            }
        }
	}

	//=======================================================================================================================================================/
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.localPosition, .1f);
        Gizmos.DrawWireSphere(StartPosition, 0.1f);
        Gizmos.DrawLine(transform.localPosition, StartPosition);

        /*Vector3 lastpos = new Vector3(0, Ease(0) * 2);
        for(float i=0; i<=1.02; i+=0.02f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.localPosition, StartPosition, Ease(i));
            Gizmos.DrawWireSphere(newPosition, 0.02f);

            Vector3 newpos = new Vector3(i * 2, Ease(i) * 2);
            Gizmos.DrawLine(newpos, lastpos);
            lastpos = newpos;
        }*/

    }

    //=======================================================================================================================================================/
    float Ease(float value)
    {
        float blend;
        if (Mode == TweenMode.Smooth)
            blend = Smooth(value);
        else if (Mode == TweenMode.FastOut)
            blend = FastOut(value);
        else if (Mode == TweenMode.SlowOut)
            blend = SlowOut(value);
        else if (Mode == TweenMode.Berp)
        {
            value = Mathf.Clamp01(value);
            //blend = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            //blend = (Mathf.Sin(value * Mathf.PI) * 1f) + value;
            
            //blend = (Mathf.Cos((value * 360 * 4 + 180) * Mathf.Deg2Rad) * 0.1f) * 0 + Mathf.Pow(value, 0.5f);// * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            blend = Mathfx.Berp(0, 1, value);
        }       
        else
            blend = value;

        return blend;
    }

    //=======================================================================================================================================================/
    float Smooth(float value)
    {
        return Mathf.SmoothStep(0, 1, value);
    }

    //=======================================================================================================================================================/
    float FastOut(float value)
    {
        return Mathf.Pow(value, 0.5f);
    }

    //=======================================================================================================================================================/
    float SlowOut(float value)
    {
        return Mathf.Pow(value, 2f);
    }
}
