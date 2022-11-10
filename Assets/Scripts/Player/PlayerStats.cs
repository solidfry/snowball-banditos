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
        public int hp = 3;

        [SerializeField]
        public int maxHealth = 3;

        [SerializeField]
        public int charges = 5;

        [SerializeField]
        public int maxCharges = 5;

        private void Start()
        {
            _tr = GetComponent<Transform>();
            GetStartingPosition();
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(gameObject.name + " has collided with " + other.gameObject.name);
            if (other.gameObject.CompareTag("Projectile") && IsLocalPlayer)
            {

                hp--;

                Debug.Log($"Current HP is {hp}");
            }
        }

        void IsPlayerDead()
        {
            if (hp > 0) return;

            Debug.Log("Player died");
        }

        void GetStartingPosition()
        {
            _spawnPos = NetworkSpawnManager.Instance.GetStartingPosition(this.OwnerClientId);
            _tr.position = _spawnPos;
        }

    }

}

// public struct PlayerData : INetworkSerializable
// {
//     public int HitPoints;
//     public int Charges;
//     public int MaxCharges;
//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         serializer.SerializeValue(ref HitPoints);
//         serializer.SerializeValue(ref Charges);
//         serializer.SerializeValue(ref MaxCharges);
//     }
// }