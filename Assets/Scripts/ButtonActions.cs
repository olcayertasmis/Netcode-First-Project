using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
    }

    public void StartHost()
    {
        _networkManager.StartHost();
    }

    public void StartClient()
    {
        _networkManager.StartClient();
    }
}