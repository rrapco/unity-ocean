using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainChunk : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;
    List<Vector3> vertices;
    List<int> triangles;
    Color[] colors;

    [Header("Chunk Size")]
    public int xSize = 100;
    public int zSize = 100;
    public float vertexScale = 1f;

    [Header("Terrain")]
    public float noiseScale = 0.05f;
    public float heightMultiplier = 8f;
    public float noiseOffsetX = 1000f;
    public float noiseOffsetZ = 1000f;

    [Header("Visual")]
    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    [Header("Color Range")]
    public float minHeightForColor = 0f;
    public float maxHeightForColor = 8f; // == heightMultiplier

    public float ChunkWorldSizeX => xSize * vertexScale;
    public float ChunkWorldSizeZ => zSize * vertexScale;

    void Awake()
    {
        mesh = new Mesh();

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // void Start()
    // {
    //     GenerateChunk();
    // }

    public void GenerateChunk()
    {
        xSize = Mathf.Clamp(xSize, 1, 500);
        zSize = Mathf.Clamp(zSize, 1, 500);
        vertexScale = Mathf.Max(0.01f, vertexScale);

        //CreateShape();
        GenerateVertices();
        GenerateTriangles();
        GenerateColors();
        AssignMesh();
    }

    void GenerateVertices()
    {
        minTerrainHeight = float.MaxValue;
        maxTerrainHeight = float.MinValue;

        vertices = new List<Vector3>((xSize + 1) * (zSize + 1));
        triangles = new List<int>(xSize * zSize * 6);

        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float localX = x * vertexScale; 
                float localZ = z * vertexScale;

                float worldX = transform.position.x + localX;
                float worldZ = transform.position.z + localZ;

                float sampleX = (worldX + noiseOffsetX) * noiseScale;
                float sampleZ = (worldZ + noiseOffsetZ) * noiseScale;

                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                float y = noise * heightMultiplier;

                Vector3 vertex = new Vector3(localX - ChunkWorldSizeX/2f, y, localZ - ChunkWorldSizeZ/2f);
                vertices.Add(vertex);

                if (y < minTerrainHeight) minTerrainHeight = y;
                if (y > maxTerrainHeight) maxTerrainHeight = y;
            }
        }
    }

    void GenerateTriangles()
    {
        for (int row = 0; row < zSize; row++)
        {
            for (int column = 0; column < xSize; column++)
            {
                int i = row * (xSize + 1) + column;

                triangles.Add(i);
                triangles.Add(i + xSize + 1);
                triangles.Add(i + xSize + 2);

                triangles.Add(i);
                triangles.Add(i + xSize + 2);
                triangles.Add(i + 1);
            }
        }
    }

    void GenerateColors()
    {
        colors = new Color[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            float height = Mathf.InverseLerp(minHeightForColor, maxHeightForColor, vertices[i].y);

            if (gradient != null)
                colors[i] = gradient.Evaluate(height);
            else
                colors[i] = Color.white;
        }
    }


    void AssignMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}