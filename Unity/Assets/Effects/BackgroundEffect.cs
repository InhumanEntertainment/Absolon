using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Inhuman/Background Effect")]
public class BackgroundEffect : ImageEffectBase 
{
    public Color Color;
    public float Multiplier;
    public float Amount;
    public float ColorAmount;
    //public Texture ;

    //============================================================================================================================================//
    void OnRenderImage(RenderTexture source, RenderTexture destination) 
    {
        //material.SetTexture("_RampTex", textureRamp);
        material.SetFloat("_Amount", Amount); 
        material.SetFloat("_Multiplier", Multiplier);
        material.SetColor("_Color", Color);
        material.SetFloat("_ColorAmount", ColorAmount);

		Graphics.Blit (source, destination, material);
	}
}