using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        //public string objectType;
        public GameObject prefab;
        public int amount;
    }

    [SerializeField] List<Pool> pools;
    //public Dictionary<string, GameObject[]> poolDictionary;
    private GameObject[] objects = new GameObject[0];

    private void Start()
    {
        //poolDictionary = new Dictionary<string, GameObject[]>();
        
        foreach (Pool pool in pools)
        {
            int startingIndex = objects.Length;
            Array.Resize(ref objects, objects.Length + pool.amount);
            //GameObject[] objectPool = new GameObject[pool.amount];

            for (int i = startingIndex; i < objects.Length; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objects[i] = obj;
            }
        }
    }

    public void SpawnRandomFromPool(Vector3 position, Quaternion rotation)
    {
        int objectIndex = UnityEngine.Random.Range(0, objects.Length);
        
        if (objects[objectIndex].activeInHierarchy)
        {
            SpawnRandomFromPool(position, rotation);
        }
        else
        {
            objects[objectIndex].transform.position = position;
            objects[objectIndex].transform.rotation = rotation;
            objects[objectIndex].SetActive(true);
        }
    }
}
