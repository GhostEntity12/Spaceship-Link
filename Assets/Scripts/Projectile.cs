using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Poolable
{
	Rigidbody r;
	public float projectileSpeed;
	[System.NonSerialized] public float damage;
	bool active;

	private void Awake()
	{
		r = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (active)
		{
			r.velocity = transform.forward * projectileSpeed;
		}
	}

	public void Fire(float _damage, Vector3 position, Quaternion rotation)
	{
		active = true;
		transform.position = position;
		transform.rotation = rotation;
		damage = _damage;
		Invoke("RePool", 3f);
	}

	void RePool()
	{
		if (active)
		{
			CancelInvoke();
			active = false;
			r.velocity = Vector3.zero;
			r.rotation = Quaternion.identity;
			r.position = Vector3.zero;
			ReturnToPool();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		RePool();
	}
}
