using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkWindowView : MonoBehaviour
{
	[Header("Networking")]
	[SerializeField] private NetworkManager networkManager;

    [Header("UI Components")]
    [SerializeField] private TMP_Text ipText;
	[SerializeField] private TMP_InputField ipInput;

	private const string PLAYER_PREFS_IP = "hostIP";

	private void Start()
	{
		SetIpText();
		SetLastJoinedIP();
	}

	private void Update()
	{
		if (NetworkClient.isConnected || NetworkServer.active)
		{
			Destroy(gameObject);
		}
	}

	public void JoinGame()
	{
		PlayerPrefs.SetString(PLAYER_PREFS_IP, ipInput.text);
		Debug.Log("IP INPUT: " + ipInput.text);
		networkManager.networkAddress = ipInput.text;
		Debug.Log("NETWORK ADDRESS: " + ipInput.text);
		networkManager.StartClient();
	}

	public void HostGame()
	{
		networkManager.StartHost();
	}

	private void SetIpText()
	{
		//string[] ipSplit = IPManager.GetIP().Split('.');
		//ipText.text = "Room code: " + ipSplit[ipSplit.Length - 1];

		ipText.text = "IP: " + IPManager.GetIP();
	}

	private void SetLastJoinedIP()
	{
		if (PlayerPrefs.HasKey(PLAYER_PREFS_IP))
		{
			ipInput.text = PlayerPrefs.GetString(PLAYER_PREFS_IP);
		}
	}
}
