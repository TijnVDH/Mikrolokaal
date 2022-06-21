using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class QRCodeGenerator : NetworkBehaviour
{
    [SerializeField]
    private RawImage _rawImageReceiver;

    [SyncVar]
    public string new_ip;

    private const string PLAYER_PREFS_IP = "hostIP";

    private CommonsLibary CL = new CommonsLibary();

    private Texture2D _storeEncodedTexture;

    [SerializeField] private TMP_Text ipText;

    // Start is called before the first frame update
    void Start()
    {
        new_ip = CL.GetIPv4();
        string[] _ipv1x4 = new_ip.Split('.');
        new_ip = CL.IPv4EncodeToPass(_ipv1x4).ToString("X");
        _storeEncodedTexture = new Texture2D(256, 256);
        ipText.text = new_ip;
        EncodeTextToQRCode();
    }

    private Color32 [] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(textForEncoding);
    }

    public void OnClickEncode()
    {
        EncodeTextToQRCode();
    }

    private void EncodeTextToQRCode()
    {
        string textWrite = new_ip;
        Color32[] _convertPixelToTexture = Encode(textWrite, _storeEncodedTexture.width, _storeEncodedTexture.height);
        _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
        _storeEncodedTexture.Apply();

        _rawImageReceiver.texture = _storeEncodedTexture;
    }

    public override void OnStartClient()
    {
        get_ip();
        Debug.Log("qr client connected");
    }

    [Command(requiresAuthority = false)]
    public void get_ip()
    {
        new_ip = CL.GetIPv4();
        string[] _ipv1x4 = new_ip.Split('.');
        new_ip = CL.IPv4EncodeToPass(_ipv1x4).ToString("X");
        RpcSend_ip(new_ip);
        Debug.Log(new_ip);
    }

    [ClientRpc]
    public void RpcSend_ip(string _result)
    {
        new_ip = _result;
        Debug.Log(_result);
        ipText.text = new_ip;
        _storeEncodedTexture = new Texture2D(256, 256);
        EncodeTextToQRCode();
    }
}
