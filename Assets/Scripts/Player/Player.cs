using System;
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

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		playerMovement = GetComponent<PlayerMovement>();
		playerFiring = GetComponent<PlayerFiring>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
