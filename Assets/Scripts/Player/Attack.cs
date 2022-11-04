using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        [FormerlySerializedAs("_isAttacking")]
        [Header("Attack Information")]
        [SerializeField] bool isAttacking;
        [SerializeField] float forceFactor;
        [SerializeField] GameObject attackPrefab;
        [SerializeField] Transform origin;
        [SerializeField] private float cooldownTime = 0.5f;
        
        [Header("Projectile Charges")]
        [SerializeField] int charges = 5, maxCharges = 5;
        [SerializeField] private string rechargeTargetTag = "RechargeZone";
        [SerializeField] GameObject rechargePrefab;
        Camera _cam;

        void Start()
        {
            _cam = Camera.main;
        }

        void OnAttack()
        {
            if(charges > 0 && !isAttacking)
            {
                charges--;
                StartCoroutine(CoolDown());
                var throwObject = Instantiate(attackPrefab, origin.position, Quaternion.identity);
                throwObject.GetComponent<Rigidbody>().AddForce(_cam.transform.forward * forceFactor);
                // isAttacking = true;
                Debug.Log("Attacking");
            }
        }

        IEnumerator CoolDown()
        {
            isAttacking = true;
            yield return new WaitForSeconds(cooldownTime);
            isAttacking = false;
        }

        void ResetCharges()
        {
            charges = maxCharges;
            Instantiate(rechargePrefab, transform.position, quaternion.identity);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(rechargeTargetTag))
            {
                ResetCharges();
            }
        }
    }
}
