using System.Collections;
using StarterAssets;
using Unity.Mathematics;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(NetworkThirdPersonController))]
    public class Attack : NetworkBehaviour
    {
        [Header("Attack Information")]
        [SerializeField] bool isAttacking;
        [SerializeField] float forceFactor;
        [SerializeField] GameObject attackPrefab;
        [SerializeField] Transform origin;
        [SerializeField] float cooldownTime = 0.5f;

        [Header("Projectile Charges")] 
        [SerializeField] private int charges;
        [SerializeField] private int maxCharges;
        [SerializeField] string rechargeTargetTag = "RechargeZone";
        [SerializeField] GameObject rechargePrefab;
        [SerializeField] Camera _cam;
        [SerializeField] private NetworkThirdPersonController playerController;
        
        private PlayerStats stats;
        private PlayerData playerData;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsOwner && IsClient)
            {
                if(playerController == null)
                {
                    playerController = GetComponent<NetworkThirdPersonController>();
                }
                _cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();

                stats = GetComponent<PlayerStats>();
                playerData = stats.data.Value;
            }
        }

        private void Update()
        { 
            charges = playerData.Charges;
            maxCharges = playerData.MaxCharges;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(rechargeTargetTag))
            {
                ResetCharges();
            }
        }

        void OnAttack()
        {
            Debug.Log("Attacking");
            if(IsOwner && IsClient)
            {
                if (attackPrefab == null)
                    return;

                if (playerData.Charges > 0 && !isAttacking)
                {
                    playerData.Charges--;

                    FireProjectileOnClientRpc();

                    // Debug.Log("Attacking");
                    // print("Charges are" + playerData.Charges);
                }
            }
        }

        void InstantiateProjectile()
        {
            var projectile = Instantiate(attackPrefab, origin.position, Quaternion.identity);

            projectile.GetComponent<NetworkObject>().Spawn(true);
            Collider objCollider = projectile.GetComponent<Collider>();
            projectile.GetComponent<Rigidbody>().AddForce(_cam.transform.forward * forceFactor);
            StartCoroutine(ToggleCollider(objCollider));
        }

        IEnumerator CoolDown()
        {
            isAttacking = true;
            yield return new WaitForSeconds(cooldownTime);
            isAttacking = false;
        }

        IEnumerator ToggleCollider(Collider collider)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            collider.enabled = true;
        }

        void ResetCharges()
        {
            playerData.Charges = playerData.MaxCharges;
            Instantiate(rechargePrefab, transform.position, quaternion.identity);
        }

        [ClientRpc]
        private void FireProjectileOnClientRpc()
        {
            InstantiateProjectile();
        }

        
    }
}
