using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class NetworkAddressAssigner : MonoBehaviour
{
	[SerializeField] private ADDRESSFAM ipType = ADDRESSFAM.IPv4;
	[SerializeField] private bool assignOnAwake = true;
	[SerializeField] private bool debugLog = false;

	private NetworkManager mirrorNetworkManager;

	private void Awake()
	{
		mirrorNetworkManager = GetComponent<NetworkManager>();
		if (assignOnAwake) Assign(ipType);
	}

	public void Assign(ADDRESSFAM ipType = ADDRESSFAM.IPv4)
	{
		string ip = IPManager.GetIP(ipType);
		if (debugLog) Debug.LogFormat("Setting network address to '{0}'", ip);
		mirrorNetworkManager.networkAddress = IPManager.GetIP(ipType);
	}
}
