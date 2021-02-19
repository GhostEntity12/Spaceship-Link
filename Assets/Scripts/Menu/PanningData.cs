using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Unnamed Panning Data", menuName = "Panning Data")]
public class PanningData : ScriptableObject
{
	public AnimationCurve movementCurveX;
	public AnimationCurve movementCurveY;
	public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
	public float duration = 0.3f;
	[NonSerialized] public float progress;
}
