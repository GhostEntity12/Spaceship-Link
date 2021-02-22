using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[Header("Scoring")]
	private int score;
	public TextMeshProUGUI scoreText;

	[Header("Players")]
	public Player player1;
	public Player player2;

	[Header("Line Rendering")]
	public AnimationCurve colorRemap;
	public Color midpointColor;
	private LineRenderer line;
	Gradient g = new Gradient();

	[Header("Damage Scaling")]
	public AnimationCurve damageRemap;
	[System.NonSerialized]
	public float damageMultiplier;

	[Header("Audio")]
	public AudioSource audioSource;

	[Header("Particles")]
	public ParticlePool bigExplosionPool;
	public ParticlePool explosionPool;
	public ParticlePool projectileExplosionPool;

	[Header("Other")] 
	public Transform deathBoxPoint;
	public GameOver gameOverScript;
	int baseCount;
	public Transform[] spawnPoints;

	void Awake()
	{
		instance = this;
		line = GetComponent<LineRenderer>();
		Spawning.OnDestroyBaseEvent += BaseDestroyed;
	}

	private void Start()
	{
		baseCount = FindObjectsOfType<Base>().Length;
		PlayerSetup setup = FindObjectOfType<PlayerSetup>();

		if (setup)
		{
			SetupPlayer(player1, setup.p1Data);
			SetupPlayer(player2, setup.p2Data);

			Destroy(setup.gameObject);

			print("Loaded setup data");
		}
		else
		{
			player1.playerInput.SwitchCurrentControlScheme("KBM", Keyboard.current, Mouse.current);
			player1.playerMovement.isKeyboard = true;
			player2.playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);

			Renderer renderer = player1.GetComponent<Renderer>();
			renderer.materials[1].color = player1.color;
			renderer.materials[3].color = player1.color;
			renderer = player2.GetComponent<Renderer>();
			renderer.materials[1].color = player2.color;
			renderer.materials[3].color = player2.color;
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (player1.isAlive && player2.isAlive)
		{
			var distance = Vector3.Distance(player1.transform.position, player2.transform.position);

			damageMultiplier = damageRemap.Evaluate(distance);

			float normalised = (2 * damageMultiplier - 1) / 3;

			g.SetKeys(
				new GradientColorKey[] { new GradientColorKey(player1.color, 0), new GradientColorKey(midpointColor, 0.5f), new GradientColorKey(player2.color, 1) },
				new GradientAlphaKey[] { new GradientAlphaKey(colorRemap.Evaluate(normalised), 0), new GradientAlphaKey(colorRemap.Evaluate(normalised), 1) }
				);
			line.colorGradient = g;

			line.widthMultiplier = Mathf.Max(normalised / 3, 0.1f);

			line.SetPosition(0, player1.transform.position);
			line.SetPosition(1, (player1.transform.position + player2.transform.position) / 2);
			line.SetPosition(2, player2.transform.position);
		}
		else
		{
			line.widthMultiplier = 0;
			damageMultiplier = damageRemap.Evaluate(0);
		}
	}

	void SetupPlayer(Player player, PlayerSetupData data)
	{
		switch (data.device)
		{
			case Gamepad _:
				player.playerInput.SwitchCurrentControlScheme("Gamepad", data.device);
				player.playerMovement.isKeyboard = false;
				break;
			case Keyboard _:
				player.playerInput.SwitchCurrentControlScheme("KBM", data.device, Mouse.current);
				player.playerMovement.isKeyboard = true;
				break;
		}
		player.color = data.color;

		Renderer renderer = player.GetComponent<Renderer>();
		renderer.materials[1].color = player.color;
		renderer.materials[3].color = player.color;
	}

	void BaseDestroyed(Base b)
	{
		baseCount--;
		if (baseCount == 0)
		{
			gameOverScript.TriggerGameOver(score);
		}
	}

	public void UpdateScore(int increase)
	{
		score += increase;
		scoreText.text = score.ToString();
	}
}
