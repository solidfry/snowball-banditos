using System.Collections.Generic;
using Events;
using Player;
using StarterAssets;
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
        
        
        private List<Image> images = new();
        
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
                ChangeUI(charges, maxCharges);
            
            health = stats.hp;
            
            if(health < maxHealth)
                ChangeUI(health, maxHealth);
        }

        [ServerRpc]
        private void SetPlayerUIValuesServerRpc(PlayerData value)
        {
            Debug.Log("UI set up ran");
            maxCharges = value.MaxCharges;
            charges = value.Charges;
            health = value.HitPoints;
            maxHealth = value.HitPoints;
            
            InstantiateUI(healthPrefab, healthParent.transform, maxHealth);
            InstantiateUI(ammoPrefab, ammoParent.transform, maxCharges);
        }

        void InstantiateUI(GameObject go, Transform tr, int value)
        {
            for (int i = 0; i < value; i++)
            {
                images.Add(Instantiate(go, tr).GetComponent<Image>());
            }
        }

        void ChangeUI(int count, int maxCount)
        {
            if(count < maxCount)
                for (int i = 0; i < images.Count; i++)
                {
                    var toggle = images[i].GetComponent<ToggleImage>();
                    if (i < maxCount)
                        toggle.IsFull = true;
                    else 
                        toggle.IsFull = false;
                }
        }

    }
}
