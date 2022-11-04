using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HideCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        this.GetComponent<Camera>().enabled = false;
    }
}
