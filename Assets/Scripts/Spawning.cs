using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemySpawningClass
{
	public Enemy enemyPrefab;
	public int cost;
	public bool spawnable = true;
	public int requiredWaveThreshold;

	[System.NonSerialized]
	public EnemyPool enemyPool;

	public EnemySpawningClass(int cost)
	{
		this.cost = cost;
	}

	public void SetupPool(GameObject poolObject, ProjectilePool projectilePool)
	{
		enemyPool = poolObject.AddComponent(typeof(RangedEnemyPool)) as RangedEnemyPool;
		(enemyPool as RangedEnemyPool).projectilePool = projectilePool;
		enemyPool.sourceObject = enemyPrefab;
	}

	public void SetupPool(GameObject poolObject)
	{
		enemyPool = poolObject.AddComponent(typeof(EnemyPool)) as EnemyPool;
		enemyPool.sourceObject = enemyPrefab;
	}
}

public class Spawning : MonoBehaviour
{
	public static Spawning instance;
	Camera c;
	readonly float[] offscreenPos = new float[2] { -0.1f, 1.1f };
	public Transform defaultTarget;


	[Header("Enemy Types")]
	public EnemySpawningClass meleeBasic = new EnemySpawningClass(1);
	public EnemySpawningClass meleeFast = new EnemySpawningClass(3);
	public EnemySpawningClass meleeStrong = new EnemySpawningClass(5);
	public EnemySpawningClass meleeTank = new EnemySpawningClass(5);
	public EnemySpawningClass meleeBoss = new EnemySpawningClass(10);
	public EnemySpawningClass rangedBasic = new EnemySpawningClass(3);
	public EnemySpawningClass rangedSniper = new EnemySpawningClass(7);
	List<EnemySpawningClass> allEnemies = new List<EnemySpawningClass>();

	public ProjectilePool enemyProjectilePool;

	[Header("Waves")]
	public float roundEndDelay = 5;
	float timer;
	float roundEndTimer;

	public AnimationCurve difficultyCurve;
	int wave;
	int pointPool;
	public int activeEnemyCount;

	List<Base> bases = new List<Base>();

	public delegate void OnDestroyBase(Base b);
	public static event OnDestroyBase OnDestroyBaseEvent;

	void Start()
	{
		c = Camera.main;

		instance = this;

		meleeBasic.SetupPool(gameObject);
		meleeFast.SetupPool(gameObject);
		meleeStrong.SetupPool(gameObject);
		meleeTank.SetupPool(gameObject);
		meleeBoss.SetupPool(gameObject);
		rangedBasic.SetupPool(gameObject, enemyProjectilePool);
		rangedSniper.SetupPool(gameObject, enemyProjectilePool);

		allEnemies.Add(meleeBasic);
		allEnemies.Add(meleeFast);
		allEnemies.Add(meleeStrong);
		allEnemies.Add(meleeTank);
		allEnemies.Add(meleeBoss);
		allEnemies.Add(rangedBasic);
		allEnemies.Add(rangedSniper);

		bases = FindObjectsOfType<Base>().ToList();

		meleeBasic.spawnable = true;
	}

	private void Update()
	{
		if (roundEndTimer == 0)
		{
			if (timer == 0)
			{
				if (pointPool > 0)
				{
					var validEnemies = allEnemies.Where(e => e.cost <= pointPool && e.spawnable).ToList();
					var chosenEnemyType = validEnemies[Random.Range(0, validEnemies.Count())];
					Spawn(chosenEnemyType.enemyPool.GetPooledEnemy());
					pointPool -= chosenEnemyType.cost;
					timer = chosenEnemyType.cost / 1.5f;
				}
				else if (activeEnemyCount == 0)
				{
					wave++;
					pointPool = Mathf.CeilToInt(difficultyCurve.Evaluate(wave));
					allEnemies.Where(e => e.requiredWaveThreshold == wave).ToList().ForEach(e => e.spawnable = true);
					roundEndTimer = roundEndDelay;
				}
			}

			timer = Mathf.Max(0, timer - Time.deltaTime);
		}
		roundEndTimer = Mathf.Max(0, roundEndTimer - Time.deltaTime);
	}

	void Spawn(Enemy enemy)
	{
		activeEnemyCount++;
		float sidePos = Random.Range(-0.1f, 1.1f);
		Vector3 position = c.ViewportToWorldPoint(Random.value > 0.5 ? new Vector3(offscreenPos[Random.Range(0, 2)], sidePos, c.transform.position.y) : new Vector3(sidePos, offscreenPos[Random.Range(0, 2)], c.transform.position.y));
		enemy.transform.position = position;
		enemy.Activate(bases[Random.Range(0, bases.Count)]);
	}

	public void BaseDestroyed(Base b)
	{
		bases.Remove(b);
		OnDestroyBaseEvent?.Invoke(b);
	}

	public Transform GetNewBase()
	{
		if (bases.Count > 0)
		{
			return bases[Random.Range(0, bases.Count)].transform;
		}
		else return defaultTarget;
	}
}