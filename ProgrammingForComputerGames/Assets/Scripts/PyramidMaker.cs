using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidMaker : MonoBehaviour
{
    [SerializeField]
    private float pyramidSize = 5f;

    [SerializeField] List<Material> materialsList = new List<Material>();
    [Range(0, 9), SerializeField] int materialUsed;

    [Space]

    [SerializeField] bool enable = false;

    private void Start()
    {
        Pyramid();
    }

    private void Update()
    {
        Enable();
    }

    private void Enable()
    {
        if (enable)
        {
            Pyramid();
            enable = false;
        }
    }

    void Pyramid()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshBuilder meshBuilder = new MeshBuilder(materialsList.Count);

        Vector3 top = new Vector3(0, pyramidSize, 0);
        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * pyramidSize;

        meshBuilder.BuildTriangle(base0, base1, base2, materialUsed);
        meshBuilder.BuildTriangle(base1, base0, top, materialUsed);
        meshBuilder.BuildTriangle(base2, top, base0, materialUsed);
        meshBuilder.BuildTriangle(top, base2, base1, materialUsed);

        meshFilter.mesh = meshBuilder.CreateMesh();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }
}
