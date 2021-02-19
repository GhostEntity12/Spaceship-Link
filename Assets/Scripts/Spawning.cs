using UnityEngine;

[System.Serializable]
public class EnemySpawningClass
{
	public Enemy enemyPrefab;
	public int cost;

	[System.NonSerialized]
	public EnemyPool enemyPool;

	public EnemySpawningClass(int cost)
	{
		this.cost = cost;
	}

	public void SetupPool(GameObject poolObject)
	{
		if (enemyPrefab is RangedEnemy)
		{
			enemyPool = poolObject.AddComponent(typeof(RangedEnemyPool)) as RangedEnemyPool;
		}
		else
		{
			enemyPool = poolObject.AddComponent(typeof(EnemyPool)) as EnemyPool;
		}
		enemyPool.sourceObject = enemyPrefab;
	}
}

public class Spawning : MonoBehaviour
{
	public AnimationCurve difficultyCurve;

	Camera c;
	readonly float[] offscreenPos = new float[2] { -0.1f, 1.1f };

	public EnemySpawningClass meleeBasic = new EnemySpawningClass(1);
	public EnemySpawningClass meleeFast = new EnemySpawningClass(3);
	public EnemySpawningClass meleeStrong = new EnemySpawningClass(5);
	public EnemySpawningClass meleeTank = new EnemySpawningClass(5);
	public EnemySpawningClass meleeBoss = new EnemySpawningClass(10);
	public EnemySpawningClass rangedBasic = new EnemySpawningClass(3);
	public EnemySpawningClass rangedSniper = new EnemySpawningClass(7);

	// Start is called before the first frame update
	void Start()
	{
		c = Camera.main;

		meleeBasic.SetupPool(gameObject);
		meleeFast.SetupPool(gameObject);
		meleeStrong.SetupPool(gameObject);
		meleeTank.SetupPool(gameObject);
		meleeBoss.SetupPool(gameObject);
		rangedBasic.SetupPool(gameObject);
		rangedSniper.SetupPool(gameObject);
	}

	// Update is called once per frame
	public void Spawn(Enemy enemy)
	{
		float sidePos = Random.Range(-0.1f, 1.1f);
		Vector3 position = c.ViewportToWorldPoint(Random.value > 0.5 ? new Vector3(offscreenPos[Random.Range(0, 2)], sidePos, c.transform.position.y) : new Vector3(sidePos, offscreenPos[Random.Range(0, 2)], c.transform.position.y));
		enemy.transform.position = position;
	}
}
