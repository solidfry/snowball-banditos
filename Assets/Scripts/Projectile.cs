using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject destroyParticles;
    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(.01f);
        var particle = Instantiate(destroyParticles, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.01f);
    }
}
