using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeMaker : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    [SerializeField] List<Material> materialsList = new List<Material>();
    [Range(0, 9), SerializeField] int materialUsed;

    [Space]

    [SerializeField] bool enable = false;

    private void Start()
    {
        Cube();
    }

    private void Update()
    {
        Enable();
    }

    private void Enable()
    {
        if (enable)
        {
            Cube();
            enable = false;
        }
    }

    public void Cube()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshBuilder meshBuilder = new MeshBuilder(materialsList.Count);

        Vector3 t0 = new Vector3(size.x, size.y, -size.z); 
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z); 
        Vector3 t2 = new Vector3(-size.x, size.y, size.z);
        Vector3 t3 = new Vector3(size.x, size.y, size.z);

        Vector3 b0 = new Vector3(size.x, -size.y, -size.z); 
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z);
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z); 
        Vector3 b3 = new Vector3(size.x, -size.y, size.z);

        meshBuilder.BuildTriangle(t0, t1, t2, materialUsed);
        meshBuilder.BuildTriangle(t0, t2, t3, materialUsed);

        meshBuilder.BuildTriangle(b2, b1, b0, materialUsed);
        meshBuilder.BuildTriangle(b3, b2, b0, materialUsed);

        meshBuilder.BuildTriangle(b0, t1, t0, materialUsed);
        meshBuilder.BuildTriangle(b0, b1, t1, materialUsed);

        meshBuilder.BuildTriangle(b1, t2, t1, materialUsed);
        meshBuilder.BuildTriangle(b1, b2, t2, materialUsed);

        meshBuilder.BuildTriangle(b2, t3, t2, materialUsed);
        meshBuilder.BuildTriangle(b2, b3, t3, materialUsed);

        meshBuilder.BuildTriangle(b3, t0, t3, materialUsed);
        meshBuilder.BuildTriangle(b3, b0, t0, materialUsed);

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }

    public void AddMaterial(Material material)
    {
        materialsList.Add(material);
    }
}
