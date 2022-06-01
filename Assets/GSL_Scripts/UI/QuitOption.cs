using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitOption : MonoBehaviour
{
	public Button quitBtn;

	void Start()
	{
		Button quit = quitBtn.GetComponent<Button>();
		quit.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		Application.Quit();
		Debug.Log("You quit the game");
	}
}
