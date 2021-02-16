using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public AnimationCurve difficultyCurve;
    public GameObject cube;
    Camera c;
    readonly float[] offscreenPos = new float[2] { -0.1f, 1.1f };

    // Start is called before the first frame update
    void Start()
    {
        c = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float sidePos = Random.Range(-0.1f, 1.1f);
        Instantiate(cube, c.ViewportToWorldPoint(Random.value > 0.5 ? new Vector3(offscreenPos[Random.Range(0, 2)], sidePos, c.transform.position.y) : new Vector3(sidePos, offscreenPos[Random.Range(0, 2)], c.transform.position.y)), Quaternion.identity);
    }
}
