using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Poolable
{
	Rigidbody r;
	public float projectileSpeed;
	[System.NonSerialized] public int damage;
	public AudioClip shootSound;

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

	public void Fire(int _damage, Vector3 position, Quaternion rotation)
	{
		active = true;
		GameManager.instance.audioSource.PlayOneShot(shootSound, 0.2f);
		transform.position = position;
		transform.rotation = rotation;
		damage = _damage;
		Invoke("RePoolTimeout", 3f);
	}

	public void RePool()
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

	void RePoolTimeout()
	{
		Debug.Log($"Returning {gameObject.name} to pool: Timed out");
		RePool();
	}

	private void OnCollisionEnter(Collision collision)
	{
		var explosion = GameManager.instance.projectileExplosionPool.GetPooledParticle();
		explosion.transform.position = collision.GetContact(0).point;
		Debug.Log($"Returning {gameObject.name} to pool: hit {collision.gameObject.name}");
		RePool();
	}
}
