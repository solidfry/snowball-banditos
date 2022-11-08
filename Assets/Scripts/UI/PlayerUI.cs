using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : NetworkBehaviour
    {
        [SerializeField] private int charges, maxCharges;
        [SerializeField] private int health, maxHealth;
        [SerializeField] private GameObject ammoParent, healthParent;
        [SerializeField] private GameObject ammoPrefab, healthPrefab;
        [SerializeField] private PlayerStats stats = null;
        [SerializeField] private Attack attack;

        [SerializeField] private List<Image> ammoImages = new();
        [SerializeField] private List<Image> healthImages = new();
        
        public override void OnNetworkSpawn()
        {
            Debug.Log("PlayerUI spawn");
            stats = transform.parent.GetComponentInChildren<PlayerStats>();
            if (stats != null)
            {
                if (IsServer)
                {
                    SetPlayerUIValuesServerRpc(stats.data.Value);
                }
            
                charges = attack.charges;
                maxCharges = attack.maxCharges;
       
            }
        }

        private void Update()
        {
            charges = attack.charges;
            
            if(charges < maxCharges)
                ChangeUI(charges, maxCharges, ammoImages);
            
            health = stats.hp;
            
            if(health < maxHealth)
                ChangeUI(health, maxHealth, healthImages);
        }

        [ServerRpc]
        private void SetPlayerUIValuesServerRpc(PlayerData value)
        {
            Debug.Log("UI set up ran");
            maxCharges = value.MaxCharges;
            charges = value.Charges;
            health = value.HitPoints;
            maxHealth = value.HitPoints;
            
            InstantiateUI(healthPrefab, healthParent.transform, maxHealth, healthImages);
            InstantiateUI(ammoPrefab, ammoParent.transform, maxCharges, ammoImages);
        }

        void InstantiateUI(GameObject go, Transform tr, int value, List<Image> images)
        {
            for (int i = 0; i < value; i++)
            {
                images.Add(Instantiate(go, tr).GetComponent<Image>());
            }
        }

        void ChangeUI(int count, int maxCount, List<Image> images)
        {
            if(count < maxCount)
                for (int i = 0; i < images.Count; i++)
                {
                    var toggle = images[i].GetComponent<ToggleImage>();
                    toggle.IsFull = i < maxCount;
                }
        }

    }
}
