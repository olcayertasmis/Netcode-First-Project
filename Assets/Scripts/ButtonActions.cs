using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager _networkManager;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void Awake()
    {
        _networkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartHost()
    {
        _networkManager.StartHost();
        InitMovementText();
    }

    public void StartClient()
    {
        _networkManager.StartClient();
        InitMovementText();
    }

    public void SubmitNewPosition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerMovement>();
        player.Move();
    }

    private void InitMovementText()
    {
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            buttonText.text = "MOVE";
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            buttonText.text = "Request Move";
        }
    }
}