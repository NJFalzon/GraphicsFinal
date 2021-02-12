using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadHeightmapImage : MonoBehaviour
{
    [Header("Terrain")]
    private TerrainData terrainData;
    [SerializeField] List<Texture2D> heightMapImage; 
    [SerializeField] List<TerrainTextureData> terrainTextureDataList;
    [SerializeField] float terrainTextureBlendOffset = 0.01f;
    private int randomHeightMap;
    [Space]

    [Header("Water")]
    [SerializeField] GameObject water;
    [SerializeField] float waterHeight;

    [Space]

    [Header("Clouds")]
    [SerializeField] Transform cloudParent;
    [SerializeField] List<GameObject> cloud;
    [SerializeField] Material rainMat;
    [SerializeField, Range(0, 100)] int cloudCoverage;

    [Space]

    [Header("Trees")]
    [SerializeField] List<TreeData> treeDataList;
    [SerializeField] int maxTrees = 2000;
    [SerializeField] int treeSpacing = 10;
    [SerializeField] float randomXRange = 0.0f;
    [SerializeField] float randomZRange = 0.0f;
    [SerializeField] int terrainLayerIndex = 8;

    [Space]

    [Header("Scale")]
    [SerializeField] Vector3 heightMapScale = new Vector3(1, 1, 1);
    [SerializeField] bool loadHeightMap = true;

    void Start()
    {
        randomHeightMap = UnityEngine.Random.Range(0, heightMapImage.Count);
        terrainData = Terrain.activeTerrain.terrainData;
        UpdateHeightmap();
        TerrainTexture();
        AddWater();
        AddClouds();
        AddTrees(maxTrees);
    }

    void UpdateHeightmap()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for(int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for(int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (loadHeightMap)
                {

                    heightMap[width, height] = heightMapImage[randomHeightMap].GetPixel((int)(width * heightMapScale.x),
                                                                       (int)(height * heightMapScale.z)).grayscale
                                                                       * heightMapScale.y;
                }
            }
        }

        terrainData.SetHeights(0 , 0, heightMap);
    }

    void AddWater()
    {
        GameObject waterGameObject = GameObject.Find("Water");
        waterHeight = UnityEngine.Random.Range(0.3f, 0.4f);

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "Water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    void AddClouds()
    {
        for(int i = 0; i < cloudCoverage; i++)
        {
            float randomX = UnityEngine.Random.Range(transform.position.x, terrainData.size.x);
            float y = 1 * terrainData.size.y;
            float randomZ = UnityEngine.Random.Range(transform.position.x, terrainData.size.z);
            GameObject tempCloud = Instantiate(cloud[UnityEngine.Random.Range(0, cloud.Count-1)], new Vector3(randomX, y, randomZ), Quaternion.identity);
            tempCloud.transform.parent = cloudParent;
            AddRain(tempCloud.transform);
        }

        if (cloudParent.childCount > cloudCoverage)
        {
            for(int i = cloudCoverage; i < cloudParent.childCount; i++)
            {
                Destroy(cloudParent.GetChild(i).gameObject);
            }
        }
    }

    void AddTrees(int max)
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                {
                    if (treeInstanceList.Count < max)
                    {
                        float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                        if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                           currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                        {
                            float randomX = (x + UnityEngine.Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + UnityEngine.Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                treeInstance.position.y * terrainData.size.y,
                                                                treeInstance.position.z * terrainData.size.z) + this.transform.position;



                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                treeInstance.rotation = UnityEngine.Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 0.95f;
                                treeInstance.widthScale = 0.95f;

                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
        }
        

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    void AddRain(Transform cloud)
    {
        for (int i = 0; i < cloud.transform.childCount; i++)
        {
            ParticleSystem rain = cloud.GetChild(i).gameObject.AddComponent<ParticleSystem>();
            var main = rain.main;
            main.maxParticles = 10000;

            var emission = rain.emission;
            emission.enabled = true;
            emission.rateOverTime = UnityEngine.Random.Range(50, 100);

            var shape = rain.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.MeshRenderer;
            shape.meshRenderer = cloud.GetChild(i).GetComponent<MeshRenderer>();

            var force = rain.forceOverLifetime;
            force.enabled = true;
            force.y = -50;

            var color = rain.colorOverLifetime;
            color.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            color.color = grad;

            var collide = rain.collision;
            collide.enabled = true;
            collide.type = ParticleSystemCollisionType.World;
            collide.bounce = 0;

            var render = rain.GetComponent<ParticleSystemRenderer>();
            render.renderMode = ParticleSystemRenderMode.Stretch;
            render.material = rainMat;
        }
    }

    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            terrainLayers[i] = new TerrainLayer();
            terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
            terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    void NormaliseSplatMap(float[] splatmap)
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
    }
}
