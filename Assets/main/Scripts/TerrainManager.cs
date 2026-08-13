using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform player;
    public float updateStep = 5f;

    TerrainChunk chunk;
    Vector2Int lastGridPos;

    void Start()
    {
        chunk = GetComponent<TerrainChunk>();

        if (player != null && chunk != null)
        {
            UpdateTerrain(forceUpdate: true);
        }
    }

    void Update()
    {
        if (player == null || chunk == null) return;

        UpdateTerrain(forceUpdate: false);
    }

    void UpdateTerrain(bool forceUpdate)
    {
        int gridX = Mathf.FloorToInt(player.position.x / updateStep);
        int gridZ = Mathf.FloorToInt(player.position.z / updateStep);

        Vector2Int currentGridPos = new Vector2Int(gridX, gridZ);

        if (forceUpdate || currentGridPos != lastGridPos)
        {
            lastGridPos = currentGridPos;

            float snappedX = gridX * updateStep;
            float snappedZ = gridZ * updateStep;

            transform.position = new Vector3(snappedX, 0f, snappedZ);
            chunk.GenerateChunk();
        }
    }
}