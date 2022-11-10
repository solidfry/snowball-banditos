using System.Collections.Generic;
using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : NetworkBehaviour
    {

        [SerializeField] private GameObject ammoParent, healthParent;
        [SerializeField] private GameObject ammoPrefab, healthPrefab;
        [SerializeField] private PlayerStats stats = null;
        [SerializeField] private Attack attack;

        [SerializeField] private List<Image> ammoImages = new();
        [SerializeField] private List<Image> healthImages = new();

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            Debug.Log("PlayerUI spawn");
            if (stats == null)
                stats = transform.parent.GetComponentInChildren<PlayerStats>();


            SetPlayerUIValues(stats.maxHealth, stats.maxCharges);
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            ChangeUI(stats.charges, stats.maxCharges, ammoImages);
            ChangeUI(stats.hp, stats.maxHealth, healthImages);
        }


        private void SetPlayerUIValues(int maxHitPoints, int ammoMaxCharges)
        {
            // Debug.Log("UI set up ran");

            InstantiateUI(healthPrefab, healthParent.transform, stats.maxHealth, healthImages);
            InstantiateUI(ammoPrefab, ammoParent.transform, stats.maxCharges, ammoImages);
        }

        void InstantiateUI(GameObject go, Transform tr, int value, List<Image> images)
        {
            for (int i = 0; i < value; i++)
            {
                var imageObj = Instantiate(go, tr).GetComponent<Image>();
                imageObj.GetComponent<ToggleImage>().IsFull = true;
                images.Add(imageObj);
            }
        }

        void ChangeUI(int count, int maxCount, List<Image> images)
        {
            for (int i = 0; i < images.Count; i++)
            {
                var toggle = images[i].GetComponent<ToggleImage>();
                // print("attempted to toggle UI");
                if (i < count)
                    toggle.IsFull = true;
                else
                    toggle.IsFull = false;
            }
        }

    }
}
