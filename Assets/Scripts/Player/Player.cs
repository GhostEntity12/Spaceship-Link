using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public Color color;

	[NonSerialized]
	public PlayerInput playerInput;
	[NonSerialized]
	public PlayerMovement playerMovement;
	[NonSerialized]
	public PlayerFiring playerFiring;
	[NonSerialized]
	public bool isAlive = true;

	List<TrailRenderer> trails = new List<TrailRenderer>();
	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		playerMovement = GetComponent<PlayerMovement>();
		playerFiring = GetComponent<PlayerFiring>();
		trails = GetComponentsInChildren<TrailRenderer>().ToList();
	}

	public void DestroyPlayer()
	{
		var explosion = GameManager.instance.explosionPool.GetPooledParticle();
		explosion.transform.position = transform.position;
		isAlive = false;
		transform.position = GameManager.instance.deathBoxPoint.position;
		trails.ForEach(t => t.enabled = false);
		playerFiring.enabled = false;
		Invoke("RespawnPlayer", 5f);
	}

	void RespawnPlayer()
	{
		isAlive = true;
		foreach (Transform spawnPoint in GameManager.instance.spawnPoints)
		{
			if (Physics.OverlapSphere(spawnPoint.position, 1).Length == 0)
			{
				transform.position = spawnPoint.position;
				break;
			}
		}
		trails.ForEach(t => t.enabled = true);
		playerFiring.enabled = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 7)
		{
			DestroyPlayer();
		}
	}
}
