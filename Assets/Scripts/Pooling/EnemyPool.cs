using UnityEngine;

public class EnemyPool : Pool
{
	private void Start()
	{
		if (!(sourceObject is Enemy))
		{
			Debug.LogError($"EnemyPool {gameObject.name} has a prefab that is not a enemy", this);
			enabled = false;
		}
	}

	public Enemy GetPooledEnemy() => base.GetPooledObject() as Enemy;
}
