using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerFiring : MonoBehaviour
{
	bool isFiring;
	public int projectileDamage = 10;
	public float fireSpeed = 0.1f;
	float fireTimer = 0;

	public ProjectilePool projectilePool;

	public Transform[] firingPoints;

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isFiring)
		{
			fireTimer = 0;
		}
		else
		{
			if (fireTimer == 0)
			{

				fireTimer = fireSpeed;
				foreach (Transform t in firingPoints)
				{
					Projectile projectile = projectilePool.GetPooledProjectile();
					projectile.Fire(Mathf.CeilToInt(projectileDamage * GameManager.instance.damageMultiplier / firingPoints.Length), t.position, t.rotation);
					print(projectileDamage * GameManager.instance.damageMultiplier / firingPoints.Length);
				}
			}
			fireTimer = Mathf.Max(0, fireTimer - Time.fixedDeltaTime);
		}
	}

	public void OnFire(CallbackContext context)
	{
		isFiring = context.ReadValue<float>() > 0;
	}
}
