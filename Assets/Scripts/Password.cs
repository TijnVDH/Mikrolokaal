using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Password : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _expected_Result_T;
    [SerializeField]
    private TextMeshProUGUI _passcode_T;
    [SerializeField]
    private TextMeshProUGUI _passcode_IF;
    [SerializeField]
    private TextMeshProUGUI _answer_T;

    private CommonsLibary _cl = new CommonsLibary();

    private string _ipv4;
    private string[] _ipv1x4;
    private long _ipComb;

    private void Start()
    {
        _ipv4 = _cl.GetIPv4();
        _ipv1x4 = _ipv4.Split('.');
        _ipComb = _cl.IPv4EncodeToPass(_ipv1x4);

        _expected_Result_T.text = _ipv4;
        _passcode_T.text = _ipComb.ToString("X");
    }

    public void Decode()
    {
        string _pass = _passcode_IF.text;
        string _result = _cl.PassDecodeToIPv4(_pass);
        _answer_T.text = _result;
    }
}
