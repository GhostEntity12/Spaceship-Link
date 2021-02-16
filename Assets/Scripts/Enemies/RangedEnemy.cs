using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
	public int projectileDamage;
	public float fireSpeed;

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
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		print("other" + other.name + other.gameObject.layer);
		if (other.gameObject.layer == 8)
		{
			shootAtBase = true;
			destination = other.transform;
		}
	}
}
