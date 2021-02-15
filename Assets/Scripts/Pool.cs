using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool : MonoBehaviour
{
    public GameObject prefab;
    Queue<GameObject> itemPool = new Queue<GameObject>();

    public virtual GameObject GetPooledObject()
    {
        GameObject newObject;
        if (itemPool.Count == 0)
        {
            Debug.LogWarning("Ran out of items in the pool, instantiating a new instance");
            newObject = Instantiate(prefab);
            newObject.name = prefab.name + " (Pooled)";
        }
        else
        {
            newObject = itemPool.Dequeue();
        }
        return newObject;
    }

    public virtual void ReturnPooledObject(GameObject @object)
    {
        itemPool.Enqueue(@object);
        @object.SetActive(false);
    }
}
