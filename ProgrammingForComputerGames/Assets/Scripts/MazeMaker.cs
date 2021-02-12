using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MazeMaker : MonoBehaviour
{
    [SerializeField] List<Material> materialList;
    [SerializeField] Vector3[] start;
    [SerializeField] Vector3[] end;

    GameObject plane;
    GameObject wall;

    void Start()
    {
        Platform();
        StartEnd();
        Walls();
    }

    void Platform()
    {
        plane = new GameObject("Plane");
        plane.transform.position = Vector3.zero;
        plane.AddComponent<PlaneMaker>();
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[1]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.AddComponent<BoxCollider>();
        plane.AddComponent<Rigidbody>().isKinematic = true;
        plane.transform.parent = transform;
    }

    void StartEnd()
    {
        int startTemp = Random.Range(0, start.Length);
        int endTemp;

        if(startTemp == 0 || startTemp == 1)
        {
            endTemp = 0;
        }
        else
        {
            endTemp = 1;
        }

        plane = new GameObject("Plane");
        plane.transform.position = start[startTemp];
        plane.AddComponent<PlaneMaker>().Size(5, 5);
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[2]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.transform.parent = transform;

        plane = new GameObject("Plane");
        plane.transform.position = end[endTemp];
        plane.AddComponent<PlaneMaker>().Size(5, 5);
        plane.AddComponent<BoxCollider>().center = new Vector3(2f, 0, 2f);
        plane.GetComponent<BoxCollider>().size = new Vector3(4, 0.01f, 4);
        plane.AddComponent<Rigidbody>().isKinematic = true;
        plane.AddComponent<FinishLevel>();
        plane.GetComponent<PlaneMaker>().AddMaterial(materialList[3]);
        plane.GetComponent<PlaneMaker>().Plane();
        plane.transform.parent = transform;

        Player(start[startTemp]);
    }

    void Walls()
    {
        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(0, 1, 12f);
        wall.AddComponent<CubeMaker>().size = new Vector3(1, 1, 13f);
        wall.AddComponent<BoxCollider>().size = new Vector3(1, 1, 13f) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(12, 1, 0);
        wall.AddComponent<CubeMaker>().size = new Vector3(13, 1, 1);
        wall.AddComponent<BoxCollider>().size = new Vector3(13, 1, 1) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(24, 1, 12);
        wall.AddComponent<CubeMaker>().size = new Vector3(1, 1, 13);
        wall.AddComponent<BoxCollider>().size = new Vector3(1, 1, 13) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(12, 1, 24);
        wall.AddComponent<CubeMaker>().size = new Vector3(13, 1, 1);
        wall.AddComponent<BoxCollider>().size = new Vector3(13, 1, 1) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(8, 1, 18);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.AddComponent<BoxCollider>().size = new Vector3(8, 1, 1) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(16, 1, 12);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.AddComponent<BoxCollider>().size = new Vector3(8, 1, 1) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;

        wall = new GameObject("Wall");
        wall.transform.position = new Vector3(8, 1, 6);
        wall.AddComponent<CubeMaker>().size = new Vector3(8, 1, 1);
        wall.AddComponent<BoxCollider>().size = new Vector3(8, 1, 1) * 2;
        wall.AddComponent<Rigidbody>().isKinematic = true;
        wall.GetComponent<CubeMaker>().AddMaterial(materialList[0]);
        wall.GetComponent<CubeMaker>().Cube();
        wall.transform.parent = transform;
    }

    void Player(Vector3 position)
    {
        position += Vector3.one*2; 
        Camera.main.transform.position = position;
    }
}
