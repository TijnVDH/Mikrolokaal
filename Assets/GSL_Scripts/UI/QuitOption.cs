using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class QuitOption : NetworkBehaviour
{
	public Button quitBtn;

	public GameObject soup;

	void Start()
	{
		Button quit = quitBtn.GetComponent<Button>();
		quit.onClick.AddListener(soup.GetComponent<QuitScript>().CmdQuit);
	}
}
