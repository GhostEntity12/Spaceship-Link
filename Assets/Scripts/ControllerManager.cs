using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
	Gamepad player1Gamepad, player2Gamepad;

	private void Update()
	{
		if (player1Gamepad == null || player2Gamepad == null)
		{
			foreach (Gamepad gamepad in Gamepad.all)
			{
				if (gamepad.aButton.wasPressedThisFrame)
				{
					if (player1Gamepad == null)
					{
						player1Gamepad = gamepad;
					}
					else
					{
						player2Gamepad = gamepad;
					}
				}
			}
		}
	}
}
