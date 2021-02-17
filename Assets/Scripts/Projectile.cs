using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Poolable
{
	Rigidbody r;
	public float projectileSpeed;
	//[System.NonSerialized]
	public float damage;

	private void Awake()
	{
		r = GetComponent<Rigidbody>();
	}

	public void Fire(float _damage)
	{
		r.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
		damage = _damage;
		Invoke("Remove", 3f);
		Debug.DrawRay(transform.position, transform.forward, Color.white, 0.3f);
	}

	void Remove()
	{
		r.velocity = Vector3.zero;
		r.rotation = Quaternion.identity;
		r.position = Vector3.zero;
		ReturnToPool();
	}
}
