using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public Transform player;
    public float updateStep = 5f;

    WaterGenerator water;
    Vector2Int lastGridPos;

    void Start()
    {
        water = GetComponent<WaterGenerator>();

        if (player != null && water != null)
        {
            water.GenerateWater();
            UpdateWaterPosition(forceUpdate: true);
        }
    }

    void Update()
    {
        if (player == null || water == null) return;

        UpdateWaterPosition(forceUpdate: false);
        water.AnimateWater(Time.timeSinceLevelLoad);
    }

    void UpdateWaterPosition(bool forceUpdate)
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
        }
    }
}