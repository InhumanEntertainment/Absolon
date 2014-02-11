using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Effect")]

public class ColorEffect : ImageEffectBase
{
    public Color Color;
    public Color DeathColor;
    public float Multiplier;
    public float Amount;
    public float ColorAmount;

    public bool isFading = false;
    public float FadeTimer = 0;
    public float FadeDuration = 0.3f;
    float StartTime = 10;

    //============================================================================================================================================//
    void Awake()
    {
        StartTime = Time.realtimeSinceStartup - 10;
    }

    //============================================================================================================================================//
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Amount", Amount);
        material.SetFloat("_Multiplier", Multiplier);
        if (isFading)
            material.SetColor("_Color", DeathColor);
        else
            material.SetColor("_Color", Color);
        material.SetFloat("_ColorAmount", ColorAmount);

        Graphics.Blit(source, destination, material);
    }

    //============================================================================================================================================//
    public void StartFade()
    {
        isFading = true;
        StartTime = Time.realtimeSinceStartup;
    }

    //============================================================================================================================================//
    void Update()
    {
        if (isFading)
        {
            FadeTimer = Time.realtimeSinceStartup - StartTime;
            float delta = Mathf.Clamp01(FadeTimer / FadeDuration);
            Amount = 1 - delta;
            if (delta == 1)
                isFading = false;
        }
        else
            Amount = 0;
    }
}