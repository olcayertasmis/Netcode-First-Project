using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestServerRPC : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            TestServerRpc(0);
        }
    }

    [ClientRpc]
    private void TestClientRpc(int value)
    {
        if (IsClient)
        {
            Debug.Log("Client Received the RPC# " + value);
            TestServerRpc(value);
        }
    }

    [ServerRpc]
    private void TestServerRpc(int value)
    {
        Debug.Log("Server Received the RPC# " + value);
        TestClientRpc(value);
    }
}