using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        [Header("Attack Information")]
        [SerializeField] bool _isAttacking;
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
            if(charges > 0 && !_isAttacking)
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
            _isAttacking = true;
            yield return new WaitForSeconds(cooldownTime);
            _isAttacking = false;
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
