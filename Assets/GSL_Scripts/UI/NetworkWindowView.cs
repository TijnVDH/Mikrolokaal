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

	private CommonsLibary commonsLibary = new CommonsLibary();

	[SerializeField] private GameObject QRScanner;

	private void Start()
	{
		//SetIpText();
		SetGamePassText();
		SetLastJoinedIP();
	}

	private void Update()
	{
		if (NetworkClient.isConnected || NetworkServer.active)
		{
			Destroy(gameObject);
		}
	}
	/// <summary>
	/// normal version
	/// </summary>
	//public void JoinGame()
	//{
	//	PlayerPrefs.SetString(PLAYER_PREFS_IP, ipInput.text);
	//	Debug.Log("IP INPUT: " + ipInput.text);
	//	networkManager.networkAddress = ipInput.text;
	//	Debug.Log("NETWORK ADDRESS: " + ipInput.text);
	//	networkManager.StartClient();
	//}

	/// <summary>
	/// Game pass version
	/// </summary>
	public void JoinGame()
	{
		string _ipv4 = commonsLibary.PassDecodeToIPv4(ipInput.text);

		PlayerPrefs.SetString(PLAYER_PREFS_IP, ipInput.text);
		Debug.Log("IP INPUT: " + _ipv4);
		networkManager.networkAddress = _ipv4;
		Debug.Log("NETWORK ADDRESS: " + _ipv4);
		networkManager.StartClient();
	}

	/// <summary>
	/// QR code version
	/// </summary>
	/// <param name="_ipv4"></param>
	public void JoinGame(string _ipv4)
	{
		Debug.Log("IP INPUT: " + _ipv4);
		networkManager.networkAddress = _ipv4;
		Debug.Log("NETWORK ADDRESS: " + _ipv4);
		networkManager.StartClient();
	}

	public void HostGame()
	{
		networkManager.StartHost();
	}

	public void ScanQR()
    {
		QRScanner.SetActive(true);
    }

	private void SetIpText()
	{
		//string[] ipSplit = IPManager.GetIP().Split('.');
		//ipText.text = "Room code: " + ipSplit[ipSplit.Length - 1];

		ipText.text = "IP: " + IPManager.GetIP();
	}

	private void SetGamePassText()
    {
		string _ipv4 = commonsLibary.GetIPv4();
		string[] _ipv1x4 = _ipv4.Split('.');
		ipText.text = "Pass: " + commonsLibary.IPv4EncodeToPass(_ipv1x4).ToString("X");
    }

	private void SetLastJoinedIP()
	{
		if (PlayerPrefs.HasKey(PLAYER_PREFS_IP))
		{
			ipInput.text = PlayerPrefs.GetString(PLAYER_PREFS_IP);
		}
	}
}
