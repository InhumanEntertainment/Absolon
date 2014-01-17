using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trail : MonoBehaviour
{
    public List<Vector3> Tail = new List<Vector3>();
    Mesh mesh;
    MeshFilter Filter;
    public Vector3 Target = Vector3.up;
    public int TailMax = 50;
    public float TailWidthStart = 1f;
    public float TailWidthEnd = 1f;    

    //============================================================================================================================================//
    void Awake()
    {
        mesh = new Mesh();
        Tail.Add(transform.position);
        Filter = GetComponent<MeshFilter>();
        Filter.mesh = mesh;   
    }

    //============================================================================================================================================//
    void Update()
    {
        if (Time.timeScale == 1)
        {
            Target = transform.position;
            float distance = Vector3.Distance(Target, transform.position);
            Vector3 prevposition = Tail[Tail.Count - 1];

            Tail.Add(Target);
            if (Tail.Count > TailMax)
                Tail.RemoveRange(0, Tail.Count - TailMax);

        }
           
        BuildMesh();
    }

    //============================================================================================================================================//
    void BuildMesh()
    {
        if (Tail.Count > 1)
        {
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

                vertices[i * 2] = Tail[i] + left * tailwidth - transform.position;
                vertices[i * 2 + 1] = Tail[i] + right * tailwidth - transform.position;

                uv[i * 2] = new Vector2(0, v * 1);
                uv[i * 2 + 1] = new Vector2(1, v * 1);
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
            }
         
            //mesh.Clear();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
    }
}
