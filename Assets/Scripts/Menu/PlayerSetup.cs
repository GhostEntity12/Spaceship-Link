using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetupData
{
	public InputDevice device;
	public Color color;

	public PlayerSetupData(InputDevice device, Color color)
	{
		this.device = device;
		this.color = color;
	}
}

public class PlayerSetup : MonoBehaviour
{
	public PlayerSetupData p1Data;
	public PlayerSetupData p2Data;
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
