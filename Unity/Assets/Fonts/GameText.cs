using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode]
public class GameText : MonoBehaviour 
{
    public Texture2D Texture;
    public float Spacing = 1;
    float PreviousSpacing = 1;
    public string Text = "Default";
    string PreviousText = "Default";

    float FPS = 60;
    public float PixelUnits = 0;
    public List<Object> SavedSprites;

    //============================================================================================================================================//
    void Awake()
    {
        CreateSprites();
	}

    //============================================================================================================================================//
    void Update()
    {
        FPS = FPS * 0.98f + (1 / Time.deltaTime) * 0.02f;
        if (FPS == Mathf.Infinity)
            FPS = 0;

        string fps = FPS.ToString("n2");
        Text = fps;

	    if(Text != PreviousText || Spacing != PreviousSpacing)
        {
            CreateSprites();
        }

        PreviousText = Text;
        PreviousSpacing = Spacing;
	}

    //============================================================================================================================================//
    void CreateSprites()
    {
        // Delete Old sprites //
        while(transform.childCount > 0)
        { 
            Transform child = transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }

        // Create sprite dictionary //      
        Object[] Sprites;
#if UNITY_EDITOR
        string texturePath = AssetDatabase.GetAssetPath(Texture);
        Sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);
        SavedSprites = new List<Object>();
        TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
        PixelUnits = importer.spritePixelsToUnits;
#else
        Sprites = SavedSprites.ToArray();
#endif

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        foreach (Object obj in Sprites)
        {
            if(obj.GetType() == typeof(Sprite))
            {
                if ((obj as Sprite).texture == Texture)
                {
                    Sprite sprite = (obj as Sprite);
                    sprites.Add(sprite.name, sprite);

#if UNITY_EDITOR
                    SavedSprites.Add(sprite);
#endif
                }
            }
        }

        // Create new Sprites //
        float offset = 0;
        for(int i=0; i < Text.Length; i++)
        {
            string letter = Text.Substring(i, 1);
            
            if (sprites.ContainsKey(letter))
            {
                var go = new GameObject(i.ToString() + "_" + letter);
                go.transform.parent = transform;
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = sprites[letter];
                go.transform.localPosition = new Vector3(offset, 0, 0);
                offset += (sprites[letter].textureRect.width + Spacing) / PixelUnits;   
            }
        }
    }

    //============================================================================================================================================//
    /*void CreateMesh()
    {
        float offset = 0;
        for (int i = 0; i < Text.Length; i++)
        {
            string letter = Text.Substring(i, 1);

            if (sprites.ContainsKey(letter))
            {
                var go = new GameObject(i.ToString() + "_" + letter);
                go.transform.parent = transform;
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = sprites[letter];
                go.transform.localPosition = new Vector3(offset, 0, 0);
                offset += (sprites[letter].textureRect.width + Spacing);
            }
        }

        // Vertices //

        // Triangles //




        // Then create triangles //
        Vector3[] vertices = new Vector3[Tail.Count * 2];
        Vector2[] uv = new Vector2[Tail.Count * 2];
        int[] triangles = new int[(Tail.Count - 1) * 6];

        // Generate Vertices //
        for (int i = 0; i < Tail.Count; i++)
        {
            // Generate the vertex positions //
            Vector3 vector;
            if (i == 0)
            {
                vector = Tail[i] - Tail[i + 1];
            }
            else if (i == Tail.Count - 1)
            {
                vector = Tail[i - 1] - Tail[i];
            }
            else
            {
                vector = Tail[i - 1] - Tail[i + 1];
            }

            vector.Normalize();

            Vector3 left = new Vector3(vector.y * -1, vector.x, 0);
            Vector3 right = new Vector3(vector.y, vector.x * -1, 0);

            // from 0 to 1 along the length of the tail //
            float v = 1 - ((float)i / (Tail.Count - 1));
            float tailwidth = Mathf.Lerp(TailWidthStart, TailWidthEnd, v);

            vertices[i * 2] = Tail[i] + left * tailwidth;
            vertices[i * 2 + 1] = Tail[i] + right * tailwidth;

            uv[i * 2] = new Vector2(0, v * 1);
            uv[i * 2 + 1] = new Vector2(1, v * 1);

            //Debug.DrawLine(Tail[i] + left, Tail[i] + right, Color.blue);
        }

        // Generate Triangles //
        for (int i = 0; i < Tail.Count - 1; i++)
        {
            int t1 = i * 2;
            int t2 = i * 2 + 1;
            int t3 = i * 2 + 2;
            int t4 = i * 2 + 3;

            triangles[i * 6] = t1;
            triangles[i * 6 + 1] = t2;
            triangles[i * 6 + 2] = t3;

            triangles[i * 6 + 3] = t3;
            triangles[i * 6 + 4] = t2;
            triangles[i * 6 + 5] = t4;

            // Draw Wireframe //
            if (DrawWireframe)
            {
                Debug.DrawLine(vertices[t1], vertices[t2], Color.black);
                Debug.DrawLine(vertices[t3], vertices[t4], Color.black);
                Debug.DrawLine(vertices[t1], vertices[t3], Color.black);
                Debug.DrawLine(vertices[t2], vertices[t4], Color.black);
            }
        }

        // Draw Tail Mesh //           
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        MeshFilter m = GetComponent<MeshFilter>();
        Graphics.DrawMesh(mesh, Matrix4x4.identity, renderer.material, 0);
    }*/
}

// Draw Text Boxes and Bounds //
// Get subsprites from atlas ??
