using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Color Effect")]
public class ColorEffect : ImageEffectBase 
{
    public Color Color;
    public float Multiplier;
    public float Amount;
    public float ColorAmount;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) 
    {
        material.SetFloat("_Amount", Amount); 
        material.SetFloat("_Multiplier", Multiplier);
        material.SetColor("_Color", Color);
        material.SetFloat("_ColorAmount", ColorAmount);

		Graphics.Blit (source, destination, material);
	}
}