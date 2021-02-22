using UnityEngine;

public class RangedEnemy : Enemy
{
	public int projectileDamage;

	public ProjectilePool projectilePool;

	public float fireSpeed = 0.1f;
	float fireTimer = 0;

	[System.NonSerialized] public bool shootAtBase = false;

	public Transform[] firingPoints;

	protected override void FixedUpdate()
	{
		if (!shootAtBase)
		{
			base.FixedUpdate();
		}
		else
		{
			r.velocity = Vector3.zero;
			r.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination.position - transform.position), 15));

			if (fireTimer == 0)
			{
				fireTimer = fireSpeed;
				foreach (Transform t in firingPoints)
				{
					Projectile projectile = projectilePool.GetPooledProjectile();
					projectile.Fire(Mathf.CeilToInt(projectileDamage * GameManager.instance.damageMultiplier / firingPoints.Length), t.position, t.rotation);
				}
			}
			fireTimer = Mathf.Max(0, fireTimer - Time.fixedDeltaTime);
		}
	}

	public override void DestroyEnemy(bool addPoints)
	{
		shootAtBase = false;
		base.DestroyEnemy(addPoints);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 8)
		{
			shootAtBase = true;
			destination = other.transform;
		}
	}
}
