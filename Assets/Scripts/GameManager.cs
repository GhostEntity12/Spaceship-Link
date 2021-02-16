using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scoring")]
    public int score;

    [Header("Players")]
    public Player player1;
    public Player player2;

    [Header("Line Rendering")]
    public AnimationCurve colorRemap;
    public Color midpointColor;
    private LineRenderer line;
    Gradient g = new Gradient();

    [Header("Damage Scaling")]
    public float minDistance = 5;
    public float maxDistance = 35;
    public AnimationCurve damageRemap;
    [System.NonSerialized]
    public float damageMultiplier;
    
    void Awake()
    {
        instance = this;
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        player1.playerInput.SwitchCurrentControlScheme("KBM", Keyboard.current, Mouse.current);
        player2.playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
    }

    // Update is called once per frame
    void Update()
    {
        var usefulDistance =  1 - ((Mathf.Clamp(Vector3.Distance(player1.transform.position, player2.transform.position), minDistance, maxDistance) - minDistance)/ maxDistance);

        damageMultiplier = damageRemap.Evaluate(usefulDistance);

        g.SetKeys(
            new GradientColorKey[] { new GradientColorKey(player1.color, 0), new GradientColorKey(midpointColor, 0.5f), new GradientColorKey(player2.color, 1) },
            new GradientAlphaKey[] { new GradientAlphaKey(colorRemap.Evaluate(usefulDistance), 0), new GradientAlphaKey(colorRemap.Evaluate(usefulDistance), 1) }
            );
        line.colorGradient = g;

        line.widthMultiplier = usefulDistance / 3;

        line.SetPosition(0, player1.transform.position);
        line.SetPosition(1, (player1.transform.position + player2.transform.position) / 2);
        line.SetPosition(2, player2.transform.position);
    }
}
