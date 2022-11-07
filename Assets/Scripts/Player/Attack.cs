using System.Collections;
using StarterAssets;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
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
        [SerializeField] Camera cam;
        [SerializeField] private NetworkThirdPersonController playerController;
        
        private PlayerStats stats;
        private PlayerData playerData;
        
        [SerializeField] private CameraAimNetworking cameraAimNetworking;
        [SerializeField] private Vector3 cameraForward;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsOwner && IsClient)
            {
                if(playerController == null)
                {
                    playerController = GetComponent<NetworkThirdPersonController>();
                }
                cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
                Debug.Log(cam);
                stats = GetComponent<PlayerStats>();
                playerData = stats.data.Value;
                
                if(cameraAimNetworking == null)
                    cameraAimNetworking = GetComponent<CameraAimNetworking>();
                
                cameraForward = cameraAimNetworking.cameraVectorNetwork.Value.forward;
                cameraAimNetworking.cameraVectorNetwork.OnValueChanged += UpdatePlayerAim;
            }
        }

        private void UpdatePlayerAim(CameraForward previousvalue, CameraForward newvalue)
        {
            print(cameraForward);
            cameraForward = cameraAimNetworking.cameraVectorNetwork.Value.forward;
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
                if (attackPrefab == null || isAttacking)
                    return;
                
                if (playerData.Charges > 0 && !isAttacking)
                {
                    // isAttacking = true;
                    StartCoroutine(CoolDown(1f));
                    playerData.Charges--;

                    FireProjectileOnServerRpc();

                    // Debug.Log("Attacking");
                    // print("Charges are" + playerData.Charges);
                }
            }
        }

        void InstantiateProjectile()
        {
            if(IsServer)
            {
                var projectile = Instantiate(attackPrefab, origin.position, Quaternion.identity);

                projectile.GetComponent<NetworkObject>().Spawn(true);

                Collider objCollider = projectile.GetComponent<Collider>();

                projectile.GetComponent<Rigidbody>()
                    .AddForce(cameraForward * forceFactor);

                StartCoroutine(ToggleCollider(objCollider));
            }
        }

        IEnumerator CoolDown(float time)
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
            Instantiate(rechargePrefab, transform.position, Quaternion.identity);
        }

        // Todo: is this really the right code?
        [ServerRpc]
        private void FireProjectileOnServerRpc()
        {
            InstantiateProjectile();
        }

        
    }
}
