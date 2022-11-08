using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class CameraAimNetworking : NetworkBehaviour
    { 

        [SerializeField] private Vector3 _forward;
    
        public NetworkVariable<CameraForward> cameraVectorNetwork;
    
        [SerializeField] private Transform cameraTransform;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            cameraVectorNetwork = new NetworkVariable<CameraForward>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
            
            if(IsOwner && IsClient)
            {
                print("OwnerClientID is " + OwnerClientId);
                
                if(cameraTransform == null)
                    cameraTransform = GameObject.Find("PlayerCamera").transform;

                // cameraVectorNetwork.OnValueChanged += UpdateCamera;
            }
        }

        private void Update()
        {
            UpdateCameraValues();
        }

        void UpdateCameraValues()
        {
            if(IsClient && IsOwner)
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
}