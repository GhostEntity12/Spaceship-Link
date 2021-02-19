using UnityEngine;

public class RangedEnemy : Enemy
{
	public int projectileDamage;

	public ProjectilePool projectilePool;

	public float fireSpeed = 0.1f;
	float fireTimer = 0;

	bool shootAtBase;

	protected override void FixedUpdate()
	{
		if (!shootAtBase)
		{
			base.FixedUpdate();
		}
		else
		{
			r.velocity = Vector3.zero;
			r.MoveRotation(Quaternion.LookRotation(destination.position - transform.position));

			if (fireTimer == 0)
			{
				fireTimer = fireSpeed;
				Projectile projectile = projectilePool.GetPooledProjectile();
				projectile.transform.position = this.transform.position;
				projectile.transform.rotation = this.transform.rotation;
				projectile.Fire(projectileDamage);
			}
			fireTimer = Mathf.Max(0, fireTimer - Time.fixedDeltaTime);
		}
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
