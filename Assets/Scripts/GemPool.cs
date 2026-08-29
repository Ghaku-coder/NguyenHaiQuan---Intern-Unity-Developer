using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    public static GemPool Instance {get; private set;}

    [Header("prefab va so luong khoi tao")]
    public GameObject gemPrefab;
    public int poolSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for(int i = 0; i < poolSize; i++)
        {
            GameObject gem = CreateNewGem();
            pool.Enqueue(gem);
        }
    }

    GameObject CreateNewGem()
    {
        GameObject gem = Instantiate(gemPrefab, transform);
        gem.SetActive(false);
        return gem;
    }

    public GameObject GetGem(Vector3 position, Quaternion rotation)
    {
        GameObject gem;

        if(pool.Count > 0)
        {
            gem = pool.Dequeue();
        }
        else
        {
            gem = CreateNewGem();
        }

        gem.transform.SetPositionAndRotation(position, rotation);
        gem.SetActive(true);
        return gem;
    }

    public void ReturnGem(GameObject gem)
    {
        gem.SetActive(false);
        gem.transform.SetParent(transform);
        pool.Enqueue(gem);
    }
}
