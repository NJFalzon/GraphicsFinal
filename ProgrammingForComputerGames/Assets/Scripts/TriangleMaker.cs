using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TriangleMaker : MonoBehaviour
{

    [SerializeField] Vector3 size = Vector3.one;
    [SerializeField] List<Material> materialList;
    [Range (0, 9), SerializeField] int materialUsed;

    [Space]

    [SerializeField] bool enable = false;

    private void Start()
    {
        Triangle();
    }

    private void Update()
    {
        Enable();
    }

    private void Enable()
    {
        if (enable)
        {
            Triangle();
            enable = false;
        }
    }

    public void Triangle()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshBuilder meshBuilder = new MeshBuilder(materialList.Count);

        Vector3 p0 = new Vector3(size.x,  size.y, -size.z);
        Vector3 p1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 p2 = new Vector3(-size.x, size.y,  size.z);

        meshBuilder.BuildTriangle(p0, p1, p2, materialUsed);
        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialList.ToArray();
    }
}
