using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
	public int baseHealth;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 7)
		{
			Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
			baseHealth -= collidedEnemy.damage;
			collidedEnemy.DestroyEnemy();

			GameManager.instance.score += collidedEnemy.points;
		}
	}
}
