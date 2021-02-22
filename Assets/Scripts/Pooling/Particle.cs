using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : Poolable
{
	ParticleSystem particles;
	public AudioClip explosionSound;

	private void Awake()
	{
		particles = GetComponent<ParticleSystem>();
	}

	private void OnEnable()
	{
		active = true;
		particles.Play();
		if (explosionSound)
			GameManager.instance.audioSource.PlayOneShot(explosionSound, 0.5f);
		Invoke("RePool", 3f);
	}

	public void RePool()
	{	
		active = false;
		ReturnToPool();
	}
}
