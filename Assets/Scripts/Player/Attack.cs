using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        bool _isAttacking;
        [SerializeField] float forceFactor;
        [SerializeField] GameObject attackPrefab;
        [SerializeField] Transform origin;
        Camera _cam;

        void Start()
        {
            _cam = Camera.main;
        }

        void OnAttack()
        {
            var throwObject = Instantiate(attackPrefab, origin.position, Quaternion.identity);
            throwObject.GetComponent<Rigidbody>().AddForce(_cam.transform.forward * forceFactor);
            // isAttacking = true;
            Debug.Log("Attacking");
        }

    }
}
