using UnityEngine;

public class Base : MonoBehaviour
{
	public int baseHealth;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 7)
		{
			Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
			if (collidedEnemy is MeleeEnemy meleeEnemy)
			{
				baseHealth -= meleeEnemy.collisionDamage;
				collidedEnemy.DestroyEnemy();

				GameManager.instance.score += collidedEnemy.points;
			}
		}
	}
}
