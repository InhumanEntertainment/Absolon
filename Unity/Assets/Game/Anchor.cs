using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Anchor : MonoBehaviour 
{
	public enum AnchorPoint
	{
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public AnchorPoint Location = AnchorPoint.MiddleCenter;
	Vector3[] Vectors = 
	{
		new Vector3(0, 1, 0),
		new Vector3(0.5f, 1, 0),
		new Vector3(1, 1, 0),
		new Vector3(0, 0.5f, 0),
		new Vector3(0.5f, 0.5f, 0),
		new Vector3(1, 0.5f, 0),
		new Vector3(0, 0, 0),
		new Vector3(0.5f, 0, 0),
		new Vector3(1, 0, 0),
	};
	
	//=======================================================================================================================================================/
	void Update () 
	{
		// Positioning //
		Vector3 offset = Vector3.Scale(Vectors[(int)Location], new Vector3(Screen.width, Screen.height, 0));
		Vector3 wh = Camera.main.ScreenToWorldPoint(offset);
		wh.z = 0;
		transform.position = wh;
	}
}
