using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailOptimized : MonoBehaviour
{
    Mesh mesh;
    MeshFilter Filter;
    public Vector3 Target = Vector3.up;
    public int TailMax = 50;
    public float TailWidthStart = 1f;
    public float TailWidthEnd = 1f;
    
    Vector3[] Points;
    int PointCount = 0;
    int PointOffset = 0;
    int PointStart = 0;
    int PointPrev = 0;

    Vector3[] Vertices;
    Vector2[] Uvs;
    int[] Triangles;

    public Material Material;

    //============================================================================================================================================//
    void Awake()
    {
        mesh = new Mesh();
        Filter = GetComponent<MeshFilter>();
        Filter.mesh = mesh;

        Points = new Vector3[TailMax];
        Vertices = new Vector3[TailMax * 2];
        Uvs = new Vector2[TailMax * 2];
        Triangles = new int[TailMax * 6];
        AddPoint(transform.position);
    }

    //============================================================================================================================================//
    void AddPoint(Vector3 point)
    {   
        // Point //
        if (PointCount == TailMax)
        {
            PointStart++;
            if (PointStart > TailMax - 1)
                PointStart = 0;
        }
        else
            PointCount++;

        int current_point = PointStart + PointCount - 1;
        if (current_point > TailMax - 1)
		    current_point = current_point - TailMax;

        Points[current_point] = point;
        
        // Vertices //
        Vector3 vector;
        if(PointCount > 1)
            vector = Points[current_point] - Points[PointPrev];
        else
            vector = Vector3.up;

        vector.Normalize();

        Vector3 left = new Vector3(vector.y * -1, vector.x, 0);
        Vector3 right = new Vector3(vector.y, vector.x * -1, 0);

        Vertices[current_point * 2] = Points[current_point] + left * TailWidthStart;
        Vertices[current_point * 2 + 1] = Points[current_point] + right * TailWidthStart;

        // Triangles //     
        int t1 = PointPrev * 2;
        int t2 = PointPrev * 2 + 1;
        int t3 = current_point * 2;
        int t4 = current_point * 2 + 1;

        Triangles[current_point * 6] = t1;
        Triangles[current_point * 6 + 1] = t2;
        Triangles[current_point * 6 + 2] = t3;

        Triangles[current_point * 6 + 3] = t3;
        Triangles[current_point * 6 + 4] = t2;
        Triangles[current_point * 6 + 5] = t4;

        PointPrev = current_point;
    }

    //============================================================================================================================================//
    void Update()
    {
        if (Time.timeScale == 1)
        {
            Target = transform.position;
            AddPoint(Target);
        }
           
        BuildMesh();
    }

    //============================================================================================================================================//
    void BuildMesh()
    {
        if (PointCount > 1)
        {
            int[] ShortTriangles = new int[(PointCount - 1) * 6];

            // Generate Vertices //
            for (int i = 0; i < PointCount - 2; i++)
            {
                int current_point = PointStart + i + 1;
                if (current_point > TailMax - 1)
                    current_point = current_point - TailMax;

                ShortTriangles[i * 6] = Triangles[current_point * 6];
                ShortTriangles[i * 6 + 1] = Triangles[current_point * 6 + 1];
                ShortTriangles[i * 6 + 2] = Triangles[current_point * 6 + 2];

                ShortTriangles[i * 6 + 3] = Triangles[current_point * 6 + 3];
                ShortTriangles[i * 6 + 4] = Triangles[current_point * 6 + 4];
                ShortTriangles[i * 6 + 5] = Triangles[current_point * 6 + 5];

                // UVs //
                float v = 1 - ((float)i / (PointCount - 1));
                Uvs[current_point * 2] = new Vector2(0, v);
                Uvs[current_point * 2 + 1] = new Vector2(1, v);
            }       

            //mesh.Clear();
            mesh.vertices = Vertices;
            mesh.uv = Uvs;
            mesh.triangles = ShortTriangles;
            Graphics.DrawMesh(mesh, Matrix4x4.identity, Material, 0);
        }
    }
}
