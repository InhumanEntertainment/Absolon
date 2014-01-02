using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode]
public class GameText : MonoBehaviour 
{
    public GameFont Font;
    public string Text = "Default";              

    GameFont PreviousFont;
    string PreviousText = "Default";
    string PreviousCharacters;
    float PreviousSpacing = 1;
    Color PreviousTopColor;
    Color PreviousBottomColor;

    [HideInInspector]
    public float PixelUnits = 0;
    [HideInInspector]
    public List<Object> SavedSprites;
    Mesh Mesh;

    //============================================================================================================================================//
    void Awake()
    {
        Mesh = new Mesh();
	}

    //============================================================================================================================================//
    void Update()
    {
        if (Font != null)
        {
            if (PreviousBottomColor != Font.BottomColor || PreviousTopColor != Font.TopColor || Text != PreviousText || Font.Spacing != PreviousSpacing || Font != PreviousFont || PreviousCharacters != Font.Characters)
                CreateMesh();

            PreviousText = Text;
            PreviousSpacing = Font.Spacing;
            PreviousFont = Font;
            PreviousCharacters = Font.Characters;
            PreviousTopColor = Font.TopColor;
            PreviousBottomColor = Font.BottomColor;

            //Graphics.DrawMesh(Mesh, transform.localToWorldMatrix, Font.Material, 0);
        }
	}

    //============================================================================================================================================//
    void CreateMesh()
    {
        List<Vector3> Vertices = new List<Vector3>();
        List<Vector2> Uvs = new List<Vector2>();
        List<int> Triangles = new List<int>();
        List<Color> Colors = new List<Color>();

        // Create sprite dictionary //      
        Object[] Sprites;
#if UNITY_EDITOR
        string texturePath = AssetDatabase.GetAssetPath(Font.Material.mainTexture);
        Sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);
        SavedSprites = new List<Object>();
        TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
        PixelUnits = importer.spritePixelsToUnits;
#else
        Sprites = SavedSprites.ToArray();
#endif

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        int index = 0;
        foreach (Object obj in Sprites)
        {
            if (obj.GetType() == typeof(Sprite))
            {
                if ((obj as Sprite).texture == Font.Material.mainTexture)
                {
                    Sprite sprite = (obj as Sprite);

                    if (index < Font.Characters.Length)
                    {
                        string name = Font.Characters.Substring(index, 1);
                        sprites.Add(name, sprite);
                    }

#if UNITY_EDITOR
                    SavedSprites.Add(sprite);
#endif
                    index++;
                }
            }
        }

        // Create Characters //
        float offset = 0;
        index = 0;
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

                offset += (sprite.textureRect.width + Font.Spacing);

                // UVs //
                Uvs.Add(new Vector2(sprite.textureRect.x / (float)Font.Material.mainTexture.width, sprite.textureRect.y / (float)Font.Material.mainTexture.height));
                Uvs.Add(new Vector2(sprite.textureRect.xMax / (float)Font.Material.mainTexture.width, sprite.textureRect.y / (float)Font.Material.mainTexture.height));
                Uvs.Add(new Vector2(sprite.textureRect.xMax / (float)Font.Material.mainTexture.width, sprite.textureRect.yMax / (float)Font.Material.mainTexture.height));
                Uvs.Add(new Vector2(sprite.textureRect.x / (float)Font.Material.mainTexture.width, sprite.textureRect.yMax / (float)Font.Material.mainTexture.height));

                // Colors //
                Colors.Add(Font.BottomColor);
                Colors.Add(Font.BottomColor);
                Colors.Add(Font.TopColor);
                Colors.Add(Font.TopColor);            

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
        Mesh.colors = Colors.ToArray();

        MeshFilter m = GetComponent<MeshFilter>();
        if (m == null)
            m = gameObject.AddComponent<MeshFilter>();

        MeshRenderer r = GetComponent<MeshRenderer>();
        if (r == null)
            r = gameObject.AddComponent<MeshRenderer>();

        m.mesh = Mesh;
        renderer.material = Font.Material;      

        //print("Rebuild Mesh: " + Text.Length + " Characters");
    }
}
