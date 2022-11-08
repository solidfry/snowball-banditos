using Events;
using Unity.Netcode;
using UnityEngine;
using NetworkSpawnManager = Networking.NetworkSpawnManager;
using Vector3 = UnityEngine.Vector3;

namespace Player
{
    public class PlayerStats : NetworkBehaviour
    { 
        private Transform _tr;
        private Vector3 _spawnPos = new();
        
        [SerializeField] 
        public NetworkVariable<PlayerData> data = new(
           new PlayerData()
           {
            HitPoints = 3,
            Charges = 5,
            MaxCharges = 5,
           }
        );
       
       [SerializeField]
       public int hp;
       [SerializeField]
       public int charges;
       [SerializeField]
       public int maxCharges;
       
       public override void OnNetworkSpawn()
       {
           // var playerData = data.Value;
           hp = data.Value.HitPoints;
           charges = data.Value.Charges;
           maxCharges = data.Value.MaxCharges;
            
           data.OnValueChanged += IsPlayerDead;
           data.OnValueChanged += PlayerHealthChanged;
           data.OnValueChanged += PlayerAmmoChanged;

           _tr = GetComponent<Transform>();
           _spawnPos = NetworkSpawnManager.Instance.Spawns[OwnerClientId].position;
           _tr.position = _spawnPos;
           
           if(IsLocalPlayer)
                GameEvents.OnInitaliseUIEvent?.Invoke(data.Value);
       }

       public override void OnNetworkDespawn()
       {
           data.OnValueChanged -= IsPlayerDead;
           data.OnValueChanged -= PlayerHealthChanged;
           data.OnValueChanged -= PlayerAmmoChanged;
       }

       private void PlayerAmmoChanged(PlayerData previousvalue, PlayerData newvalue)
       {
           if (previousvalue.Charges == newvalue.Charges)
               return;

           Debug.Log("Player Ammo Changed");
           data.Value = new PlayerData { HitPoints = hp, Charges = charges, MaxCharges = 5};
           // GameEvents.OnChangeAmmoEvent?.Invoke(newvalue);
       }

       private void PlayerHealthChanged(PlayerData previousvalue, PlayerData newvalue)
       {
           if (previousvalue.HitPoints == newvalue.HitPoints)
               return;
           
           Debug.Log("Player Health Changed");
           data.Value = new PlayerData { HitPoints = hp, Charges = charges, MaxCharges = 5 };
           // GameEvents.OnChangeAmmoEvent?.Invoke(newvalue);
       }

       private void OnCollisionEnter(Collision collision)
       {
           if (collision.gameObject.CompareTag("Projectile"))
           {
               hp--;
               
               GameEvents.OnChangeHealthEvent?.Invoke(data.Value);
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