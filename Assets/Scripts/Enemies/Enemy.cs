using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public int health;
	public int points;
	public int moveSpeed;
	public Transform destination;
	protected Rigidbody r;

	private void Awake()
	{
		r = GetComponent<Rigidbody>();
		Spawn(destination.position);
	}

	public void Spawn(Vector3 destination)
	{
		r.MoveRotation(Quaternion.LookRotation(destination - transform.position));
	}

	protected virtual void FixedUpdate()
	{
		r.MoveRotation(Quaternion.LookRotation(destination.position - transform.position));
		r.velocity = transform.forward * moveSpeed;
	}

	public void DestroyEnemy()
	{

	}
}
