using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Relay
{
    public class RelayManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI idText;

        [SerializeField] private TMP_Dropdown playerCount;

        private string _playerId;

        private RelayHostData _relayHostData;

        private int _maxPlayerCount;

        private async void Start()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Service Init");
            SignIn();
        }

        private async void SignIn()
        {
            Debug.Log("Sign in");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            _playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in : " + _playerId);
            idText.text = _playerId;
        }

        public async void OnHostClick()
        {
            _maxPlayerCount = Convert.ToInt32(playerCount.options[playerCount.value].text);

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(_maxPlayerCount);

            _relayHostData = new RelayHostData()
            {
                IPv4Adress = allocation.RelayServer.IpV4,
                Port = (ushort)allocation.RelayServer.Port,

                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                Key = allocation.Key
            };

            Debug.Log("Allocate Completed : " + _relayHostData.AllocationID);
        }

        public struct RelayHostData
        {
            public string JoinCode;
            public string IPv4Adress;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] Key;
        }
    }
}