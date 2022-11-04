using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    bool isAttacking;
    [SerializeField] float forceFactor;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Transform origin;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnAttack()
    {
        var throwObject = Instantiate(attackPrefab, origin.position, Quaternion.identity);
        throwObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * forceFactor);
        // isAttacking = true;
        Debug.Log("Attacking");
    }


}
