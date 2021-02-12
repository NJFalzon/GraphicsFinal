using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoadMaker : MonoBehaviour
{
    [Header("Dimensions of Road")]

    [SerializeField] float radius = 30f;
    [SerializeField] float segments = 300f;
    [SerializeField] float lineWidth = 0.3f;
    [SerializeField] float roadWidth = 8f;
    [SerializeField] float edgeWidth = 1f;
    [SerializeField] float edgeHeight = 1f;

    [Header("Randomness of Road"),Space]

    [SerializeField] float wavyness = 5f;
    [SerializeField] float waveScale = 0.1f;
    [SerializeField] Vector2 waveOffset;
    [SerializeField] Vector2 waveStep = new Vector2(0.01f, 0.01f);

    [Header("Materials"),Space]

    [SerializeField] List<Material> materialsList = new List<Material>();
    [SerializeField] GameObject car;
    [SerializeField] GameObject checkPoint;
    [SerializeField] string nextScene;

    public int checkpoints = 0;
    private int randomStart;
    private int mark;

    List<Vector3> points;

    private void Start()
    {
        GenerateRoad();
        SpawnCar();
    }
    private void Update()
    {
        NextLevel();
    }

    void GenerateRoad()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();
        MeshBuilder meshBuilder = new MeshBuilder(materialsList.Count);

        float segmentDegrees = 360f / segments;
        points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
        }

        Vector2 wave = RandomWaves();

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * control;
        }

        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(meshBuilder, pPrev, pCurr, pNext);
        }

        meshFilter.mesh = meshBuilder.CreateMesh();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
        meshCollider.sharedMesh = meshFilter.mesh;
    }

    private void ExtrudeRoad(MeshBuilder meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 0);

        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 1);

        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 2);

        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 2);

        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 2);

    }

    private void MakeRoadQuad(MeshBuilder meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext, 
        Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshBuilder.BuildTriangle(topRight,bottomRight,bottomLeft, submesh);

        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshBuilder.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }

    Vector2 RandomWaves()
    {
        waveOffset.x = Random.Range(-100, 100);
        waveOffset.y = Random.Range(-100, 100);
        return waveOffset;
    }

    void SpawnCar()
    {
        GameObject tempCar = Instantiate(car);
        randomStart = Random.Range(0, points.Count);

        tempCar.transform.parent = transform;
        tempCar.transform.position = points[randomStart] + (Vector3.up * 2);

        if (randomStart == points.Count - 1)
        {
            tempCar.transform.LookAt(points[0] + (Vector3.up * 2));
        }
        else
        {
            tempCar.transform.LookAt(points[randomStart + 1] + (Vector3.up * 2));
        }
        SpawnCheckPoint();
    }

    public void SpawnCheckPoint()
    {
        mark = randomStart + (80 * (checkpoints + 1));
        GameObject check = Instantiate(checkPoint);

        if(mark > points.Count - 1)
        {
            mark -= points.Count;
        }

        print("Random point: " + randomStart + " Location: " + points[randomStart] + "\nMark: " + mark + " Location: " + points[mark]);

        check.transform.position = points[mark] + (Vector3.up * 5);
        check.transform.LookAt(points[mark + 1] + (Vector3.up * 5));
        check.transform.parent = transform;

    }

    void NextLevel()
    {
        if (checkpoints > 3)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
