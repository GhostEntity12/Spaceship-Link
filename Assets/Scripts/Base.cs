using System;
using UnityEngine;

public class Base : MonoBehaviour
{
	public int baseHealth = 100;

	[NonSerialized]
	new public Renderer renderer;

	private void Awake()
	{
		renderer = GetComponent<Renderer>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy)
		{
			if (enemy is MeleeEnemy meleeEnemy)
			{
				DamageBase(meleeEnemy.collisionDamage);
			}
			enemy.DestroyEnemy(false);
			return;
		}
		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile)
		{
			if (projectile.gameObject.layer == 10)
			{
				DamageBase(projectile.damage);
			}
			return;
		}
	}

	void DamageBase(int damage)
	{
		baseHealth -= damage;

		Color c = renderer.materials[1].color;
		c.a = Mathf.Max(baseHealth / 1000f - 0.01f, 0f);
		renderer.materials[1].color = c;

		if (baseHealth <= 0)
		{
			DestroyBase();
		}
	}

	public void DestroyBase()
	{
		var explosion = GameManager.instance.bigExplosionPool.GetPooledParticle();
		explosion.transform.position = transform.position;
		Spawning.instance.BaseDestroyed(this);
		gameObject.SetActive(false);
	}
}
