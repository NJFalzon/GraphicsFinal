using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<int> indices = new List<int>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<int>[] submeshIndices = new List<int>[] { };

    public MeshBuilder(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];

        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        for(int i = 0; i < 3; i++)
        {
            indices.Add(vertices.Count + i);
            submeshIndices[subMesh].Add(vertices.Count + i);
        }

        vertices.Add(p0); 
        vertices.Add(p1); 
        vertices.Add(p2);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if(submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        mesh.RecalculateNormals();
        return mesh;
    }

}
