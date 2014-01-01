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
    Mesh Mesh;

    //============================================================================================================================================//
    void Awake()
    {
        Mesh = new Mesh();
        //CreateSprites();
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
            //CreateSprites();
            CreateMesh();
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
    void CreateMesh()
    {
        List<Vector3> Vertices = new List<Vector3>();
        List<Vector2> Uvs = new List<Vector2>();
        List<int> Triangles = new List<int>();

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
            if (obj.GetType() == typeof(Sprite))
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

        // Create Characters //
        float offset = 0;
        int index = 0;
        for (int i = 0; i < Text.Length; i++)
        {
            string letter = Text.Substring(i, 1);

            if (sprites.ContainsKey(letter))
            {
                Sprite sprite = sprites[letter];

                // Vertices //
                Vertices.Add(new Vector3(offset, -sprite.textureRect.height, 0) / PixelUnits);
                Vertices.Add(new Vector3(offset + sprite.textureRect.width, -sprite.textureRect.height, 0) /PixelUnits);
                Vertices.Add(new Vector3(offset + sprite.textureRect.width, 0, 0) / PixelUnits);
                Vertices.Add(new Vector3(offset, 0, 0) / PixelUnits);

                offset += (sprite.textureRect.width + Spacing);

                // UVs //
                Uvs.Add(new Vector2(sprite.textureRect.x / (float)Texture.width, sprite.textureRect.y / (float)Texture.height));
                Uvs.Add(new Vector2(sprite.textureRect.xMax / (float)Texture.width, sprite.textureRect.y / (float)Texture.height));
                Uvs.Add(new Vector2(sprite.textureRect.xMax / (float)Texture.width, sprite.textureRect.yMax / (float)Texture.height));
                Uvs.Add(new Vector2(sprite.textureRect.x / (float)Texture.width, sprite.textureRect.yMax / (float)Texture.height));

                // Triangles //
                Triangles.Add(index);
                Triangles.Add(index + 2);
                Triangles.Add(index + 1);

                Triangles.Add(index);
                Triangles.Add(index + 3);
                Triangles.Add(index + 2);

                index += 4;
            }
        }
        
        // Draw Mesh //           
        Mesh.Clear();
        Mesh.vertices = Vertices.ToArray();
        Mesh.uv = Uvs.ToArray();
        Mesh.triangles = Triangles.ToArray();

        MeshFilter m = GetComponent<MeshFilter>();
        m.mesh = Mesh;
        //Graphics.DrawMesh(Mesh, Matrix4x4.identity);

        //print("Rebuild Mesh: " + Text.Length + " Characters");
    }
}
