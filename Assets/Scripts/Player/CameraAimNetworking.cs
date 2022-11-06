using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraAimNetworking : NetworkBehaviour
{ 

    [SerializeField] private Vector3 _forward;
    
    public NetworkVariable<CameraForward> cameraVectorNetwork;
    
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {          
        if (!IsOwner)
            return;
        
        cameraTransform = GameObject.Find("PlayerCamera").transform;

        var permission = IsOwner ? NetworkVariableWritePermission.Owner : NetworkVariableWritePermission.Server;
        cameraVectorNetwork = new NetworkVariable<CameraForward>(writePerm: permission);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner && IsClient)
        {
            print("OwnerClientID is " + OwnerClientId);
            cameraTransform = GameObject.Find("PlayerCamera").transform;

            // cameraVectorNetwork.OnValueChanged += UpdateCamera;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        // cameraVectorNetwork.OnValueChanged -= UpdateCamera;
    }

    private void UpdateCamera(Vector3 previousvalue, Vector3 newvalue)
   {
       Debug.Log("Updating camera values");
   }

   private void Update() => UpdateCameraValues();
   

   void UpdateCameraValues()
   {
       _forward = cameraTransform.forward;

       if (IsOwner)
       {
           SetState();
       }
       else
       {
           _forward = cameraVectorNetwork.Value.forward;
       }
   }

   void SetState()
   {
       var state = new CameraForward
       {
           forward = _forward,
       };

       if (IsServer)
       {
           cameraVectorNetwork.Value = state;
       }
       else
       {
           TransmitStateServerRpc(state);
       }
   }

   [ServerRpc]
   void TransmitStateServerRpc(CameraForward state)
   {
       cameraVectorNetwork.Value = state;
   }
}

[Serializable]
public struct CameraForward : INetworkSerializable
{
    public Vector3 forward;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref forward);
    }
}

