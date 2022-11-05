using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraAimNetworking : NetworkBehaviour
{
   [SerializeField] private Vector3 cameraForward = new();
   public NetworkVariable<CameraData> cameraVectorNetwork = new();
   private Transform cameraTransform;

   private void Awake()
   {
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
       previousvalue.x = cameraForward.x;
       previousvalue.y = cameraForward.y;
       previousvalue.z = cameraForward.z;
   }

   private void Update()
   {
       UpdateCameraValues();
   }

   void UpdateCameraValues()
   {
       cameraForward = cameraTransform.forward;
   }
}

[Serializable]
public struct CameraData
{
    public float x, y, z;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref y);
        serializer.SerializeValue(ref z);
    }
}
