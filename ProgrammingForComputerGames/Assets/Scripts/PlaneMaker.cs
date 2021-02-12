using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PlaneMaker : MonoBehaviour
{

    [SerializeField] float cellSize = 1f;
    [SerializeField] int width = 24;
    [SerializeField] int height = 24;
    [SerializeField] List<Material> materialsList = new List<Material>();
    [Range(0, 9), SerializeField] int materialUsed;

    [Space]

    [SerializeField] bool enable = false;

    private void Start()
    {
        //Plane();
    }

    private void Update()
    {
        Enable();
    }

    private void Enable()
    {
        if (enable)
        {
            Plane();
            enable = false;
        }
    }

    public void Plane()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshBuilder meshBuilder = new MeshBuilder(materialsList.Count);

        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(cellSize * x, 0,cellSize * y);
            }
        }

        for(int x = 0; x < width - 1; x++)
        {
            for(int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x ,    y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x ,    y + 1];
                Vector3 tl = points[x + 1, y + 1];

                meshBuilder.BuildTriangle(bl,tr,tl, materialUsed);
                meshBuilder.BuildTriangle(bl,br,tr, materialUsed);
            }
        }

        meshFilter.mesh = meshBuilder.CreateMesh();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }

    public void AddMaterial(Material material)
    {
        materialsList.Add(material);
    }

    public void Size(int x, int y)
    {
        width = x;
        height = y;
    }
}
