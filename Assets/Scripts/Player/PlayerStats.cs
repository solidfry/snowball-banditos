using System;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerStats : NetworkBehaviour
    {
       [SerializeField] public NetworkVariable<PlayerData> data = new(
           new PlayerData()
           {
            HitPoints = 3,
            Charges = 5,
            MaxCharges = 5,
           }
       );
       
       [SerializeField]
       public int hp;
       
       public override void OnNetworkSpawn()
       {
           base.OnNetworkSpawn();

           var playerData = data.Value;
           hp = playerData.HitPoints;

           data.OnValueChanged += IsPlayerDead;
       }
       
       private void OnCollisionEnter(Collision collision)
       {
           if (collision.gameObject.CompareTag("Projectile"))
           {
               hp--;
               data.Value = new PlayerData { HitPoints = hp };
               Debug.Log($"Current HP is {hp}");
           }
       }

       void IsPlayerDead(PlayerData value, PlayerData newValue)
       {
           if (newValue.HitPoints <= 0)
           { 
               Debug.Log("Player died");
           }
       }
       
    }
}

public struct PlayerData : INetworkSerializable
{
    public int HitPoints;
    public int Charges;
    public int MaxCharges;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref HitPoints);
        serializer.SerializeValue(ref Charges);
        serializer.SerializeValue(ref MaxCharges);
    }
}