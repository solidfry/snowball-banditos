using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Projectile : NetworkBehaviour
{
    [SerializeField] GameObject destroyParticles;
    private NetworkObject _networkObject;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _networkObject = this.GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(DestroyProjectile());
    }
    

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(.01f);
        var particle = Instantiate(destroyParticles, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.01f);
        _networkObject.Despawn();
    }
}
