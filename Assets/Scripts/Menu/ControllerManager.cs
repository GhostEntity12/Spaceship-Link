using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PlayerMenuState { Null, Joined, Ready }

[System.Serializable]
public class PlayerInputDevice
{
	public InputDevice activeDevice = null;

	public Renderer shipRenderer;
	public int colorIndex;

	public TextMeshProUGUI controlsText;
	public TextMeshProUGUI colorChangeText;
	public TextMeshProUGUI readyText;

	public PlayerMenuState state;

	public Animatable join;
	public Animatable ship;
	public Animatable ready;

	[NonSerialized] public float timer;
	[NonSerialized] public bool canLeft = true;
	[NonSerialized] public bool canRight = true;

	public void SetDevice(InputDevice device)
	{
		if (activeDevice == null)
		{
			activeDevice = device;
			state = PlayerMenuState.Joined;
		}
	}
}

public class ControllerManager : MonoBehaviour
{
	List<InputDevice> activeInputDevices = new List<InputDevice>();

	public PlayerInputDevice player1Slot, player2Slot;

	public bool inJoinMenu;

	readonly string keyboardControls = "<b><smallcaps>Controls</smallcaps></b><size=30>\nMove <sprite=\"controls\" name=keyboard_wasd>\nLook <sprite=\"controls\" name=mouse>\nFire <sprite=\"controls\" name=mouse_lmb>";
	readonly string keyboardChangeColor = "<sprite=\"controls\" name=keyboard_a><sprite=\"controls\" name=keyboard_d>\nChange Color";
	readonly string keyboardReady = "<sprite=\"controls\" name=keyboard_space>\nReady";
	readonly string gamepadControls = "<b><smallcaps>Controls</smallcaps></b><size=30>\nMove <sprite=\"controls\" name=xbox_stick_l>\nLook <sprite=\"controls\" name=xbox_stick_r>\nFire <sprite=\"controls\" name=xbox_trigger_r>";
	readonly string gamepadChangeColor = "<sprite=\"controls\" name=arrow_l><sprite=\"controls\" name=xbox_stick_l><sprite=\"controls\" name=arrow_r>\nChange Color";
	readonly string gamepadReady = "<sprite=\"controls\" name=xbox_a>\nReady";

	MenuManager menuManager;

	public Color[] shipColors;

	PlayerSetup setup;

	private void Start()
	{
		setup = FindObjectOfType<PlayerSetup>();
		if (!setup)
		{
			setup = new GameObject("SetupData", typeof(PlayerSetup)).GetComponent<PlayerSetup>();
		}

		menuManager = FindObjectOfType<MenuManager>();
	}

	private void Update()
	{
		if (inJoinMenu)
		{
			// Special case: quitting menu
			if (player1Slot.state == PlayerMenuState.Null && player2Slot.state == PlayerMenuState.Null &&
				(Gamepad.all.Any(g => g.bButton.wasPressedThisFrame) || Keyboard.current.escapeKey.wasPressedThisFrame))
			{
				menuManager.LoadMenuScreen();
				return;
			}

			UpdateLoop(player1Slot);
			UpdateLoop(player2Slot);

			// Special case: both players ready
			if (player1Slot.state == PlayerMenuState.Ready && player2Slot.state == PlayerMenuState.Ready)
			{
				setup.p1Data = new PlayerSetupData(player1Slot.activeDevice, shipColors[player1Slot.colorIndex]);
				setup.p2Data = new PlayerSetupData(player2Slot.activeDevice, shipColors[player2Slot.colorIndex]);

				SceneManager.LoadScene(2);
			}

			// Animations
			if (player1Slot.join.animating) player1Slot.join.Animate();
			if (player1Slot.ship.animating) player1Slot.ship.Animate();
			if (player1Slot.ready.animating) player1Slot.ready.Animate();
			if (player2Slot.join.animating) player2Slot.join.Animate();
			if (player2Slot.ship.animating) player2Slot.ship.Animate();
			if (player2Slot.ready.animating) player2Slot.ready.Animate();
		}
	}

	void UpdateLoop(PlayerInputDevice player)
	{
		switch (player.state)
		{
			case PlayerMenuState.Null:
				if (PlayerJoined(player)) return;
				break;
			case PlayerMenuState.Joined:
				if (PlayerDisconnected(player)) return;
				if (PlayerReady(player)) return;
				if (PlayerChangeColor(player)) return;
				break;
			case PlayerMenuState.Ready:
				if (PlayerUnready(player)) return;
				break;
			default:
				break;
		}
	}

	bool PlayerJoined(PlayerInputDevice player)
	{
		foreach (Gamepad gamepad in Gamepad.all)
		{
			if (activeInputDevices.Contains(gamepad)) continue;

			if (gamepad.aButton.wasPressedThisFrame)
			{
				player.SetDevice(gamepad);
				activeInputDevices.Add(gamepad);
				player.controlsText.text = gamepadControls;
				player.colorChangeText.text = gamepadChangeColor;
				player.readyText.text = gamepadReady;
				player.join.TriggerAnimation(player.join.offscreen);
				player.ship.TriggerAnimation(player.ship.onscreen);
				PlayerSetRandomColor(player);
				return true;
			}
		}

		if (activeInputDevices.Contains(Keyboard.current)) return false;

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			player.SetDevice(Keyboard.current);
			activeInputDevices.Add(Keyboard.current);
			player.controlsText.text = keyboardControls;
			player.colorChangeText.text = keyboardChangeColor;
			player.readyText.text = keyboardReady;
			player.join.TriggerAnimation(player.join.offscreen);
			player.ship.TriggerAnimation(player.ship.onscreen);
			PlayerSetRandomColor(player);
			return true;
		}

		return false;
	}

	bool PlayerDisconnected(PlayerInputDevice player)
	{
		if ((player.activeDevice is Keyboard k && k.escapeKey.wasPressedThisFrame) || (player.activeDevice is Gamepad g && g.bButton.wasPressedThisFrame))
		{
			activeInputDevices.Remove(player.activeDevice);
			player.activeDevice = null;
			player.state = PlayerMenuState.Null;
			player.join.TriggerAnimation(player.join.onscreen);
			player.ship.TriggerAnimation(player.ship.offscreen);
			return true;
		}
		return false;
	}

	bool PlayerReady(PlayerInputDevice player)
	{
		if ((player.activeDevice is Keyboard k && k.spaceKey.wasPressedThisFrame) || (player.activeDevice is Gamepad g && g.aButton.wasPressedThisFrame))
		{
			player.state = PlayerMenuState.Ready;
			player.ready.TriggerAnimation(player.ready.offscreen);
			return true;
		}
		return false;
	}

	bool PlayerUnready(PlayerInputDevice player)
	{
		if ((player.activeDevice is Keyboard k && k.escapeKey.wasPressedThisFrame) || (player.activeDevice is Gamepad g && g.bButton.wasPressedThisFrame))
		{
			player.state = PlayerMenuState.Joined;
			player.ready.TriggerAnimation(player.ready.onscreen);
			return true;
		}
		return false;
	}

	bool PlayerChangeColor(PlayerInputDevice player)
	{
		player.timer = Mathf.Max(0, player.timer - Time.deltaTime);
		if (player.timer == 0) player.canLeft = player.canRight = true;
		if ((player.activeDevice is Keyboard kl && kl.aKey.wasPressedThisFrame) || (player.activeDevice is Gamepad gl && (player.canLeft && gl.leftStick.ReadValue().x < -0.5)))
		{
			player.colorIndex = (player.colorIndex + shipColors.Length - 1) % shipColors.Length;
			player.shipRenderer.materials[1].color = shipColors[player.colorIndex];
			player.shipRenderer.materials[3].color = shipColors[player.colorIndex];
			player.canLeft = false;
			player.timer = 0.3f;
			return true;
		}
		else if ((player.activeDevice is Keyboard kr && kr.dKey.wasPressedThisFrame) || (player.activeDevice is Gamepad gr && (player.canRight && gr.leftStick.ReadValue().x > 0.5)))
		{
			player.colorIndex = (player.colorIndex + shipColors.Length + 1) % shipColors.Length;
			player.shipRenderer.materials[1].color = shipColors[player.colorIndex];
			player.shipRenderer.materials[3].color = shipColors[player.colorIndex];
			player.canRight = false;
			player.timer = 0.3f;
			return true;
		}
		return false;
	}

	void PlayerSetRandomColor(PlayerInputDevice player)
	{
		player.colorIndex = UnityEngine.Random.Range(0, shipColors.Count());
		player.shipRenderer.materials[1].color = shipColors[player.colorIndex];
		player.shipRenderer.materials[3].color = shipColors[player.colorIndex];
		player.canRight = false;
		player.timer = 0.3f;
	}
}
