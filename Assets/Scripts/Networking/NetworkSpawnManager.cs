using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    public class NetworkSpawnManager : NetworkBehaviour
    {

        public static NetworkSpawnManager Instance = null;

        private Dictionary<ulong, Transform> spawns = new();

        public Dictionary<ulong, Transform> Spawns
        {
            get => spawns;
            set => spawns = value;
        }

        [SerializeField] private List<Transform> transforms = new();
        [SerializeField] private List<ulong> id = new();

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }

            CreateAllSpawnPoints();
        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.OnClientConnectedCallback += SetLocation;
        }

        private void SetLocation(ulong id)
        {
            NetworkManager.SpawnManager
            .GetPlayerNetworkObject(id)
            .GetComponentInChildren<Player.PlayerStats>()
            .transform
            .position = Spawns[id].position;

            Debug.Log("Set location ran");
        }

        void CreateAllSpawnPoints()
        {
            var tempSpawns = GetComponentsInChildren<Transform>().ToList();
            for (int i = 0; i < tempSpawns.Count; i++)
            {
                if (i == 0) tempSpawns.RemoveAt(i);
            }
            ulong count = 0;
            Spawns = tempSpawns.ToDictionary(v => count++, k => k.GetComponent<Transform>());
            tempSpawns.Clear();

            foreach (var spawn in Spawns)
            {
                transforms.Add(spawn.Value);

                id.Add(spawn.Key);
            }
        }

        public Vector3 GetStartingPosition(ulong id) => Spawns[id].position;

    }
}
