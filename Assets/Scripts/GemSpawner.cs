using UnityEngine;
using System.Collections.Generic;

public class GemSpawner : MonoBehaviour
{
    [Header("khu vuc spawn")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(50f, 0f, 168f);

    [Header("raycast do be mat")]
    public float RaycastHeightAbove = 50f;
    public float RaycastMaxDistance = 100f;
    public LayerMask groundMask;
    public int maxAttempsPerSpawn = 30;

    [Header("kiem tra vat can tai diem spawn")]
    public LayerMask obstacleMask;
    public float obstacleCheckRadius = 0.3f;
    public float spawnHeightOffset = 0.3f;

    [Header("gioi han do doc be mat")]
    public float maxSurfaceAngle = 60f;
    public float minSurfaceAngle = -60f;

    [Header("thoi gian")]
    public float spawnInterval = 1f;

    [Header("gioi han so gem ton tai cung luc")]
    public int maxGemAlive = 15;

    [Header("Cac loai gem co the spawn")]
    public GameObject[] gemPrefabs;
    private List<GameObject> activeGems = new List<GameObject>();
    private float timer;

    void Update()
    {
        activeGems.RemoveAll(g => g == null || !g.activeInHierarchy);

        timer += Time.deltaTime;

        if(timer >= spawnInterval && activeGems.Count < maxGemAlive){
            timer = 0f;
            TrySpawnGem();
        }
    }

    void TrySpawnGem()
    {
        if (TryGetRandomValidPosition(out Vector3 spawnPos))
        {
            GameObject chosenPrefab = gemPrefabs[Random.Range(0, gemPrefabs.Length)];
            GameObject gem = GemPool.Instance.GetGem(chosenPrefab, spawnPos, Quaternion.identity);
            activeGems.Add(gem);
        }
        else
        {
            Debug.LogWarning("GemSpawner: Không tìm được vị trí spawn hợp lệ sau nhiều lần thử.");
        }
    }

    bool TryGetRandomValidPosition(out Vector3 result)
    {
        for (int i = 0; i < maxAttempsPerSpawn; i++)
        {
            float x = areaCenter.x + Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
            float z = areaCenter.z + Random.Range(-areaSize.z / 2f, areaSize.z / 2f);
            Vector3 rayStart = new Vector3(x, areaCenter.y + RaycastHeightAbove, z);

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, RaycastMaxDistance, groundMask))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                if (angle > maxSurfaceAngle || angle < minSurfaceAngle)
                    continue;

                Vector3 checkPos = hit.point + Vector3.up * spawnHeightOffset;

                if (Physics.CheckSphere(checkPos, obstacleCheckRadius, obstacleMask))
                    continue;

                result = checkPos;
                return true;
            }
        }
        result = areaCenter;
        return false;
    }

}
