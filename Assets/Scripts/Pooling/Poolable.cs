using UnityEngine;

public class Poolable : MonoBehaviour
{
	[System.NonSerialized]
	public Pool sourcePool;

	public void ReturnToPool() => sourcePool.ReturnPooledObject(this);
}
