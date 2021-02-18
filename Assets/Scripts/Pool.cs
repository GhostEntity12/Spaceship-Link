using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool : MonoBehaviour
{
    public Poolable sourceObject;
    Queue<Poolable> itemPool = new Queue<Poolable>();

    public virtual Poolable GetPooledObject()
    {
        Poolable newObject;
        if (itemPool.Count == 0)
        {
            Debug.LogWarning("Ran out of items in the pool, instantiating a new instance");
            newObject = Instantiate(sourceObject.gameObject).GetComponent<Poolable>();
            newObject.sourcePool = this;
            newObject.gameObject.name = sourceObject.name + " (Pooled)";
        }
        else
        {
            newObject = itemPool.Dequeue();
        }

        newObject.gameObject.SetActive(true);
        return newObject;
    }

    public virtual void ReturnPooledObject(Poolable returningObject)
    {
        itemPool.Enqueue(returningObject);
        returningObject.gameObject.SetActive(false);
    }
}
