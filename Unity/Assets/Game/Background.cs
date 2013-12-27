using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    float Offset = 0;
    public float ScrollSpeed = 0.1f;

    //=======================================================================================================================================================/
    void Update()
    {
        Offset += Time.deltaTime * ScrollSpeed;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(0, Offset));
	}
}
