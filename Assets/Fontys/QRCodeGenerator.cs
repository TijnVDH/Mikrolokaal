using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageReceiver;

    private string _ip;

    private CommonsLibary CL = new CommonsLibary();

    private Texture2D _storeEncodedTexture;

    // Start is called before the first frame update
    void Start()
    {
        _ip = CL.GetIPv4();
        string[] _ipv1x4 = _ip.Split('.');
        _ip = CL.IPv4EncodeToPass(_ipv1x4).ToString("X");
        _storeEncodedTexture = new Texture2D(256, 256);

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
        string textWrite = _ip;
        Color32[] _convertPixelToTexture = Encode(textWrite, _storeEncodedTexture.width, _storeEncodedTexture.height);
        _storeEncodedTexture.SetPixels32(_convertPixelToTexture);
        _storeEncodedTexture.Apply();

        _rawImageReceiver.texture = _storeEncodedTexture;
    }
}
