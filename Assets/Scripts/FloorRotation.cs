using UnityEngine;

public class FloorRotation : MonoBehaviour
{
	public float rotationSpeed;

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotationSpeed * Time.deltaTime);
	}
}
