using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class QuitOption : NetworkBehaviour
{
	public Button quitBtn;
	public bool isHost = false;

	void Start()
	{
		Button quit = quitBtn.GetComponent<Button>();
		quit.onClick.AddListener(TaskOnClick);
	}
	
	[Command]
	void TaskOnClick()
	{
		Application.Quit();
		Debug.Log("You quit the game");
	}
}
