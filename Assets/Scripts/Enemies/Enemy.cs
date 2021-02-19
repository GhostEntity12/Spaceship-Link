using UnityEngine;

public abstract class Enemy : Poolable
{
	public int health;
	public int points;
	public int moveSpeed;
	public Transform destination;
	protected Rigidbody r;
	bool active;

	private void Awake()
	{
		r = GetComponent<Rigidbody>();
	}

	public void Activate(Base b)
	{
		destination = b.transform;
		active = true;
	}

	protected virtual void FixedUpdate()
	{
		if (active)
		{
			r.MoveRotation(Quaternion.LookRotation(destination.position - transform.position));
			r.velocity = transform.forward * moveSpeed;
		}
	}

	public void DestroyEnemy()
	{
		if (active)
		{
			active = false;
			Spawning.instance.activeEnemyCount--;
			r.velocity = Vector3.zero;
			r.rotation = Quaternion.identity;
			r.position = Vector3.zero;
			ReturnToPool();
		}
	}
}
