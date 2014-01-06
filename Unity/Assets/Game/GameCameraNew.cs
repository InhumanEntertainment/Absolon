using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameCameraNew : MonoBehaviour 
{
    public Vector2 TargetResolution = new Vector2(180, 240);
    public Vector2 ScreenResolution = new Vector2(0, 0);
    public Vector2 CameraResolution = new Vector2(0, 0); 
    
    public float Aspect;
    public float TargetAspect;

    public float Multiplier;
    public int Closest;

    public float ClosestWidth;
    public float ClosestHeight;
    public float ExtraPixels;

    public float PixelScale = 100;
    public float CameraOrthographic;
    public float CameraPixelOffset;

    public float TargetPixel;

    //============================================================================================================================================================================================//
    void Update()
    {
        Aspect = (float)Screen.width / Screen.height;
        TargetAspect = TargetResolution.x / TargetResolution.y;

        float AspectRatio = Aspect;

        if (Aspect < TargetAspect)
        {
            AspectRatio = 1 / Aspect;
            Camera.main.rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            AspectRatio = 1 / TargetAspect;
            Camera.main.rect = new Rect((1 - (TargetAspect / Aspect)) / 2, 0, TargetAspect / Aspect, 1);
        }

        ScreenResolution.x = Mathf.RoundToInt(Camera.main.pixelWidth);
        ScreenResolution.y = Mathf.RoundToInt(Camera.main.pixelHeight);
        Aspect = ScreenResolution.x / ScreenResolution.y;

        float xx = (ScreenResolution.x / TargetResolution.x) % 1;
        float yy = (ScreenResolution.y / TargetResolution.y) % 1;
        Multiplier = xx < yy ? ScreenResolution.x / TargetResolution.x : ScreenResolution.y / TargetResolution.y;
        Closest = Mathf.RoundToInt(Multiplier);

        ClosestWidth = TargetResolution.x * Closest;
        ClosestHeight = TargetResolution.y * Closest;
        ExtraPixels = ScreenResolution.x - ClosestWidth;

        TargetPixel = Mathf.FloorToInt((float)ExtraPixels / Closest);
        CameraPixelOffset = ((float)ExtraPixels / Closest) % 1;
        CameraOrthographic = ((float)TargetResolution.x + TargetPixel + CameraPixelOffset) / PixelScale;
        Camera.main.orthographicSize = CameraOrthographic / Aspect / 2;

        CameraResolution.x = CameraOrthographic * 2 * PixelScale;
        CameraResolution.y = CameraOrthographic * 2 * PixelScale / Aspect;
	}
}
