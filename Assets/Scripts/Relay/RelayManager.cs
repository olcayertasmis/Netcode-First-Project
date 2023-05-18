using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Relay
{
    public class RelayManager : MonoBehaviour
    {
        //[Header("Other Compenents")]

        [Header("Player Variables")]
        [SerializeField] private TextMeshProUGUI playerIdText;
        private string _playerId;

        [Header("Room Settings && Variables")]
        [SerializeField] private TMP_Dropdown playerCount;
        private int _maxPlayerCount;
        [SerializeField] private TMP_InputField joinInput;
        [SerializeField] private TextMeshProUGUI joinCodeText;

        [Header("Data Structs")]
        private RelayHostData _relayHostData;
        private RelayJoinData _relayJoinData;

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
            playerIdText.text = "Player ID : " + _playerId;
        }

        public async void OnHostClick()
        {
            _maxPlayerCount = Convert.ToInt32(playerCount.options[playerCount.value].text);

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(_maxPlayerCount);

            _relayHostData = new RelayHostData()
            {
                IpV4Adress = allocation.RelayServer.IpV4,
                Port = (ushort)allocation.RelayServer.Port,

                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                Key = allocation.Key
            };
            _relayHostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(_relayHostData.AllocationID);

            Debug.Log("Allocate Completed : " + _relayHostData.AllocationID);

            joinCodeText.text = _relayHostData.JoinCode;
        }

        public async void OnJoinClick()
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinInput.text);

            _relayJoinData = new RelayJoinData()
            {
                IpV4Adress = joinAllocation.RelayServer.IpV4,
                Port = (ushort)joinAllocation.RelayServer.Port,

                AllocationID = joinAllocation.AllocationId,
                AllocationIDBytes = joinAllocation.AllocationIdBytes,
                ConnectionData = joinAllocation.ConnectionData,
                HostConnectionData = joinAllocation.HostConnectionData,
                Key = joinAllocation.Key
            };
        }

        private struct RelayHostData
        {
            public string JoinCode;
            public string IpV4Adress;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] Key;
        }

        private struct RelayJoinData
        {
            public string IpV4Adress;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] HostConnectionData;
            public byte[] Key;
        }
    }
}