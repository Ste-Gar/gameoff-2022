using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string objectType;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.objectType, objectPool);
        }
    }

    public GameObject SpawnFromPool(string objectType, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(objectType))
        {
            Debug.LogWarning("Object type not found: " + objectType);
            return null;
        }

        GameObject obj = poolDictionary[objectType].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[objectType].Enqueue(obj);

        return obj;
    }
}
