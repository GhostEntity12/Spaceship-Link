using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawningClass
{
    public EnemyPool enemyPool;
    public int cost;
}

public class Spawning : MonoBehaviour
{
    public AnimationCurve difficultyCurve;
    Camera c;
    readonly float[] offscreenPos = new float[2] { -0.1f, 1.1f };

    // Start is called before the first frame update
    void Start()
    {
        c = Camera.main;
    }

    // Update is called once per frame
    public void Spawn(Enemy enemy)
    {
        float sidePos = Random.Range(-0.1f, 1.1f);
        Vector3 position = c.ViewportToWorldPoint(Random.value > 0.5 ? new Vector3(offscreenPos[Random.Range(0, 2)], sidePos, c.transform.position.y) : new Vector3(sidePos, offscreenPos[Random.Range(0, 2)], c.transform.position.y));
        enemy.transform.position = position;
    }
}
