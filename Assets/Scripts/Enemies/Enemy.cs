using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : Poolable
{
	public int health;
	public int points;
	public int moveSpeed;
	public Transform destination;
	protected Rigidbody r;

	List<TrailRenderer> trails = new List<TrailRenderer>();

	private void Awake()
	{
		r = GetComponent<Rigidbody>();
		trails = GetComponentsInChildren<TrailRenderer>().ToList();
		Spawning.OnDestroyBaseEvent += CheckNewBaseRequired;
	}

	public void Activate(Base b)
	{
		destination = b.transform;
		active = true;
		r.MoveRotation(Quaternion.LookRotation(destination.position - transform.position));
		trails.ForEach(t => t.enabled = true);
	}

	protected virtual void FixedUpdate()
	{
		if (active)
		{
			r.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination.position - transform.position), 15));
			r.velocity = transform.forward * moveSpeed;
		}
	}

	public virtual void DestroyEnemy(bool addPoints)
	{
		if (active)
		{
			if (addPoints)
			{
				GameManager.instance.UpdateScore(points);
			}
			var explosion = GameManager.instance.explosionPool.GetPooledParticle();
			explosion.transform.position = transform.position;
			trails.ForEach(t => t.enabled = false);
			active = false;
			Spawning.instance.activeEnemyCount--;
			r.velocity = Vector3.zero;
			r.rotation = Quaternion.identity;
			r.position = Vector3.zero;
			ReturnToPool();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Player player = collision.gameObject.GetComponent<Player>();
		if (player)
		{
			player.DestroyPlayer();
			DestroyEnemy(false);
			return;
		}

		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile && collision.gameObject.layer == 9)
		{
			health -= projectile.damage;
			if (health <= 0)
			{
				DestroyEnemy(true);
			}
			return;
		}
	}

	void CheckNewBaseRequired(Base b)
	{
		if (active && destination == b.transform)
		{
			destination = Spawning.instance.GetNewBase();
			if (this is RangedEnemy r)
			{
				r.shootAtBase = false;
			}
			Debug.Log($"Gave new distination to ship {gameObject.name}", this);
		}
	}
}