using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Animatable
{
	public RectTransform transform;
	public PanningData offscreen;
	public PanningData onscreen;
	[NonSerialized] public PanningData activeAnimation = null;
	[NonSerialized] public bool animating = false;

	public void TriggerAnimation(PanningData animation)
	{
		animating = true;
		activeAnimation = animation;
	}

	public void Animate()
	{
		// If it's not meant to be moving, just quit here
		if (!animating) return;

		// Calculate how far into the curve we are
		activeAnimation.progress = Mathf.Clamp(activeAnimation.progress + (Time.unscaledDeltaTime / activeAnimation.duration), 0, 1);

		transform.anchoredPosition = new Vector3(activeAnimation.movementCurveX.Evaluate(activeAnimation.progress), activeAnimation.movementCurveY.Evaluate(activeAnimation.progress));
		transform.localScale = Vector3.one * activeAnimation.scaleCurve.Evaluate(activeAnimation.progress);

		// Stop if at completion
		if (activeAnimation.progress == 1)
		{
			activeAnimation.progress = 0;
			animating = false;
		}
	}
}

public class MenuManager : MonoBehaviour
{
	[Header("Title Screen")]
	public Animatable menuPanel;
	public Animatable creditsPanel;
	public Animatable p1Panel;
	public Animatable p2Panel;

	ControllerManager controllerManager;

	bool transitionToJoin, transitionToMenu;

	private void Start()
	{
		controllerManager = FindObjectOfType<ControllerManager>();
	}

	public void LoadJoinScreen()
	{
		if (transitionToJoin || transitionToMenu) return;

		menuPanel.TriggerAnimation(menuPanel.offscreen);
		creditsPanel.TriggerAnimation(creditsPanel.offscreen);
		p1Panel.TriggerAnimation(p1Panel.onscreen);
		p2Panel.TriggerAnimation(p2Panel.onscreen);
		transitionToJoin = true;
		EventSystem.current.SetSelectedGameObject(null);
		controllerManager.inJoinMenu = true;
	}

	public void LoadMenuScreen()
	{
		if (transitionToJoin || transitionToMenu) return;

		menuPanel.TriggerAnimation(menuPanel.onscreen);
		creditsPanel.TriggerAnimation(creditsPanel.onscreen);
		p1Panel.TriggerAnimation(p1Panel.offscreen);
		p2Panel.TriggerAnimation(p2Panel.offscreen);
		transitionToMenu = true;
		EventSystem.current.SetSelectedGameObject(null);
		controllerManager.inJoinMenu = false;
	}

	public void Quit() => Application.Quit();

	private void Update()
	{
		if (menuPanel.animating) menuPanel.Animate();
		if (creditsPanel.animating) creditsPanel.Animate();
		if (p1Panel.animating) p1Panel.Animate();
		if (p2Panel.animating) p2Panel.Animate();

		if (transitionToJoin && (!menuPanel.animating && !creditsPanel.animating && !p1Panel.animating && !p2Panel.animating))
			transitionToJoin = false;
		if (transitionToMenu && (!menuPanel.animating && !creditsPanel.animating && !p1Panel.animating && !p2Panel.animating))
			transitionToMenu = false;
	}
}
