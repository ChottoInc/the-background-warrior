using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public string poolName;
    public GameObject prefab;
    public int preloadAmount = 10;

    private Queue<GameObject> objects;
    private Transform parent;

    public void Initialize(Transform parentContainer)
    {
        parent = parentContainer;
        objects = new Queue<GameObject>(preloadAmount);

        for (int i = 0; i < preloadAmount; i++)
            CreateNewObject();
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        objects.Enqueue(obj);
        return obj;
    }

    public GameObject Pull()
    {
        if (objects.Count == 0)
        {
            Debug.Log("Exceeded objects");
            CreateNewObject();
        }

        GameObject obj = objects.Dequeue();
        obj.SetActive(true);

        IPoolObject poolObj = obj.GetComponent<IPoolObject>();
        poolObj?.OnSpawn();

        return obj;
    }

    public void Return(GameObject obj)
    {
        IPoolObject poolObj = obj.GetComponent<IPoolObject>();
        poolObj?.OnDespawn();

        obj.SetActive(false);
        objects.Enqueue(obj);
    }
}
