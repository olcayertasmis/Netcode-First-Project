using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if (IsOwner && IsServer)
        {
            transform.RotateAround(GetComponentInParent<Transform>().position, Vector3.up, 100f * Time.deltaTime);
        }
    }
}