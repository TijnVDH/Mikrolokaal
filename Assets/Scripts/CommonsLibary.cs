using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CommonsLibary
{
    public string GetIPv4()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    public long IPv4EncodeToPass(string[] _ipv1x4)
    {
        List<long> _hexxes = new List<long>();
        foreach(string ipv1 in _ipv1x4)
        {
            _hexxes.Add(long.Parse(ipv1));
        }

        for (int i = 0; i < _hexxes.Count; i++)
        {
            Debug.Log(_hexxes[i].ToString());
            _hexxes[i] = _hexxes[i] << (24 - (i * 8));
            Debug.Log(_hexxes[i].ToString());
        }

        long comb = _hexxes[0] | _hexxes[1] | _hexxes[2] | _hexxes[3];
        Debug.Log(comb.ToString());
        Debug.Log(comb.ToString("X"));
        
        
        return comb;
    }

    public string  PassDecodeToIPv4(string _pass)
    {
        byte[] hexBytes = StringToByteArray(_pass);
        List<int> values = new List<int>();
        values.Add((int)hexBytes[0]);
        values.Add((int)hexBytes[1]);
        values.Add((int)hexBytes[2]);
        values.Add((int)hexBytes[3]);
        string ipv4 = values[0].ToString() + '.' + values[1].ToString() + '.' + values[2].ToString() + '.' + values[3].ToString();
        Debug.Log(ipv4);
        return ipv4;
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length-1)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }
}
