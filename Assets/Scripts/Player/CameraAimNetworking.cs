using System;
using Unity.Netcode;
using UnityEngine;

public class CameraAimNetworking : NetworkBehaviour
{ 

    public NetworkVariable<CameraData> cameraVectorNetwork = new(
       new CameraData()
       {
           x = 0,
           y = 0,
           z = 0,
       }
       );
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 _forward;

    private void Awake()
    {          
        if (!IsOwner)
            return;
        
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner && IsClient)
        {
            cameraTransform = GameObject.Find("PlayerCamera").transform;

            cameraVectorNetwork.OnValueChanged += UpdateCamera;
        }
    }

   private void UpdateCamera(CameraData previousvalue, CameraData newvalue)
   {
       Debug.Log("Updating camera values");
   }

   private void Update()
   {
       if (!IsOwner)
           return;
       
       UpdateCameraValues();
   }

   void UpdateCameraValues()
   {
       var f = cameraTransform.forward;
       _forward = new(f.x, f.y, f.z); 
       cameraVectorNetwork.Value = new CameraData
       {
           x = _forward.x,
           y = _forward.y,
           z = _forward.z,
       };
   }
}

[Serializable]
public struct CameraData : INetworkSerializable
{
    public float x, y, z;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref y);
        serializer.SerializeValue(ref z);
    }
}
