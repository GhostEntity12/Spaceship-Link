using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : Pool
{
	public Projectile GetPooledProjectile()
	{
		Projectile p = base.GetPooledObject().GetComponent<Projectile>();
		p.sourcePool = this;
		p.gameObject.SetActive(true);
		return p;
	}

	public override void ReturnPooledObject(GameObject @object)
	{
		base.ReturnPooledObject(@object);
		@object.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
}
