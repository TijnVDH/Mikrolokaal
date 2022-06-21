using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : NetworkBehaviour
{
    [Range(0, .2f)] public float ProgressRate;
    [Range(0, .2f)] public float DeclineRate;
    [Range(0,  1f)] public float StartingPoint;

    public Soup Soup;
	public Slider ProgressSlider;

	private SoupState soupState;

	public static ProgressBarAction OnFullBar;
	public static ProgressBarAction OnEmptyBar;
	public delegate void ProgressBarAction();

	public GameObject EnableOnWin;
	public GameObject EnableOnLose;

	private void Start()
	{
		ProgressSlider.value = StartingPoint;
		Soup.OnStateChange += (newState) => soupState = newState;

		ProgressSlider.interactable = isServer;
	}

	[ClientRpc] public void RpcSetProgress(float progress)
	{
		ProgressSlider.SetValueWithoutNotify(progress);
	}

	public void OnSliderValueChanged() {
		RpcSetProgress(ProgressSlider.value);
	}

	private void Update()
	{
		if (ProgressSlider.value >= ProgressSlider.maxValue)
		{
			OnFullBar?.Invoke();
			EnableOnWin.SetActive(true);
			Time.timeScale = 0;
		}
		else if (ProgressSlider.value <= ProgressSlider.minValue)
		{
			OnEmptyBar?.Invoke();
			EnableOnLose.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
