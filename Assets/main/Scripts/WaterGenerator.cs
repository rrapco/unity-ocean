using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;

    List<Vector3> vertices;
    List<Vector3> baseVertices;
    List<int> triangles;

    [Header("Chunk Size")]
    public int xSize = 100;
    public int zSize = 100;
    public float vertexScale = 1f;

    [Header("Water")]
    public float amplitude = 0.2f; // vertikalna velkost vln
    public float frequency = 1f;
    public float speed = 0.5f;
    public float waterLevel = 5f;

    public float ChunkWorldSizeX => xSize * vertexScale;
    public float ChunkWorldSizeZ => zSize * vertexScale;

    void Awake()
    {
        mesh = new Mesh();
        mesh.MarkDynamic();

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public void GenerateWater()
    {
        xSize = Mathf.Clamp(xSize, 1, 500);
        zSize = Mathf.Clamp(zSize, 1, 500);
        vertexScale = Mathf.Max(0.01f, vertexScale);

        //CreateShape();
        CreateVertices();
        CreateTriangles();
        AssignMesh();
    }

    public void AnimateWater(float time)
    {
        if (vertices == null || baseVertices == null || vertices.Count == 0)
            return;

        Waves(time);
        UpdateMesh();
    }

    void CreateVertices()
    {
        vertices = new List<Vector3>((xSize + 1) * (zSize + 1));
        baseVertices = new List<Vector3>((xSize + 1) * (zSize + 1));
        triangles = new List<int>(xSize * zSize * 6);

        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float localX = x * vertexScale;
                float localZ = z * vertexScale;

                Vector3 vertex = new Vector3(
                    localX - ChunkWorldSizeX / 2f,
                    waterLevel,
                    localZ - ChunkWorldSizeZ / 2f
                );

                vertices.Add(vertex);
                baseVertices.Add(vertex);
            }
        }
    }

    void CreateTriangles()
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

    void Waves(float time)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 baseVertex = baseVertices[i];
            Vector3 vertex = baseVertex;

            float worldX = transform.position.x + baseVertex.x;
            float worldZ = transform.position.z + baseVertex.z;

            //float wave1 = Mathf.Sin(worldX * frequency + time * speed);
            float wave1 = Mathf.Sin(worldX * frequency + time * speed);
            float wave2 = Mathf.Cos(worldZ * frequency * 0.8f + time * speed * 1.2f);
            float wave3 = Mathf.Sin((worldX + worldZ) * frequency * 0.5f + time * speed * 0.7f);

            vertex.y = baseVertex.y + (wave1 + wave2 + wave3) * amplitude;
            //vertex.y = baseVertex.y + wave1 * amplitude;

            vertices[i] = vertex;
        }
    }

    void AssignMesh()
    {
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void UpdateMesh()
    {
        mesh.SetVertices(vertices);
        //mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}