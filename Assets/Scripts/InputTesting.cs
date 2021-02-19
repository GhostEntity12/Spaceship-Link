using UnityEngine;
using UnityEngine.InputSystem;

public class InputTesting : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		print(Gamepad.all.Count);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
