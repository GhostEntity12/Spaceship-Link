using UnityEngine;

public class MenuPlayerBob : MonoBehaviour
{
	public float bobSpeed;
	public float bobHeight;
	Vector3 cachedPos;

	private void Start()
	{
		cachedPos = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.5f * bobHeight, transform.localPosition.z);
	}

	void Update()
	{
		transform.localPosition = Vector3.up * (((Mathf.Cos(Time.time * bobSpeed) + 1.0f) / 2) * bobHeight) + cachedPos;
	}
}
