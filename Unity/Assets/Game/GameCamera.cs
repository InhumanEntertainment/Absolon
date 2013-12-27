using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResolutionOverride
{
    public string Name;
    public int ScreenHeight;
    public int TargetHeight;
}

public class GameCamera : MonoBehaviour 
{
    public bool PixelPerfect = true;  

    /*// Pixel Perfect Resolution //
    public float TargetScale = 0;
    public int TargetHeight = 160;
    public int ScreenHeight = 720;
    public int ScreenWidth = 1280;
    public Vector2 Resolution;*/

    // Screen Resolution Overrides
    public float UnitScale = 10;
    public int DefaultTargetHeight = 160;
    public ResolutionOverride[] Resolutions;
	
    //============================================================================================================================================================================================//
    void Awake()
    {
        // Singleton: Destroy all others //
        Object[] cameras = FindObjectsOfType(typeof(GameCamera));
        if (cameras.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }


        float targetHeight = DefaultTargetHeight;

        foreach (ResolutionOverride resolution in Resolutions)
        {
            if (Screen.height == resolution.ScreenHeight)
            {
                //print("Found Override: " + resolution.ScreenHeight + " : " + resolution.TargetHeight);
                targetHeight = resolution.TargetHeight;
            }
        }

        camera.orthographicSize = targetHeight / UnitScale / 2;
        print("Resolution: " + Screen.height + " -> " + targetHeight + " - Scale: " + Screen.height / targetHeight);
    }

	//============================================================================================================================================================================================//
	public float CamExact;
	public int CamClosest;
	public float CamAspect;

	void Update()
	{
		CamExact = Screen.height / (float)DefaultTargetHeight;
		CamClosest = Mathf.RoundToInt(CamExact);
		CamAspect = Screen.width / (float)Screen.height;

		camera.orthographicSize = (DefaultTargetHeight / UnitScale) * CamExact / CamClosest / 2;
		//print ((DefaultTargetHeight / UnitScale) * CamExact / CamClosest / 2 * DefaultTargetHeight);
	}
	
	//============================================================================================================================================================================================//
    float pixify(float number)
    {
        float result = ((int)(number * 10)) / 10f;

        return result;
    }

    //============================================================================================================================================================================================//
    /*void FindPerfectResolution()
    {
        if (ScreenHeight > TargetHeight)
        {
            float remainder = 0;
            int testHeight = TargetHeight;

            do
            {
                remainder = ScreenHeight % testHeight;
                if (remainder == 0)
                {
                    TargetScale = ScreenHeight / testHeight;
                    Resolution = new Vector2(ScreenWidth / TargetScale, testHeight);
                }
                testHeight++;
            }
            while (remainder > 0);
        }
    }*/
}
