using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    public static GemPool Instance { get; private set; }

    [System.Serializable]
    public class GemPoolEntry
    {
        public GameObject prefab;
        public int initialSize = 20;
    }

    [Header("Danh sách các loại gem")]
    public List<GemPoolEntry> gemTypes;

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new Dictionary<GameObject, Queue<GameObject>>();

    private Dictionary<GameObject, GameObject> instanceToPrefab = new Dictionary<GameObject, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var entry in gemTypes)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject gem = CreateNewGem(entry.prefab);
                queue.Enqueue(gem);
            }
            poolDict[entry.prefab] = queue;
        }
    }

    GameObject CreateNewGem(GameObject prefab)
    {
        GameObject gem = Instantiate(prefab, transform);
        gem.SetActive(false);
        instanceToPrefab[gem] = prefab; // ghi nhớ gem này thuộc prefab nào
        return gem;
    }

    public GameObject GetGem(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> queue = poolDict[prefab];
        GameObject gem;

        if (queue.Count > 0)
        {
            gem = queue.Dequeue();
        }
        else
        {
            gem = CreateNewGem(prefab);
        }

        gem.transform.SetPositionAndRotation(position, rotation);
        gem.SetActive(true);
        return gem;
    }

    public void ReturnGem(GameObject gem)
    {
        gem.SetActive(false);
        gem.transform.SetParent(transform);

        if (instanceToPrefab.TryGetValue(gem, out GameObject prefab))
        {
            if (!poolDict.ContainsKey(prefab))
                poolDict[prefab] = new Queue<GameObject>();

            poolDict[prefab].Enqueue(gem);
        }
        else
        {
            Debug.LogWarning($"GemPool: Không tìm thấy prefab gốc của {gem.name}, không thể trả về pool.");
        }
    }
}
