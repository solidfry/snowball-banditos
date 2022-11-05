using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] GameObject destroyParticles;
        private NetworkObject _networkObject;
        [SerializeField] private float yDestroy = -65f;
    
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _networkObject = this.GetComponent<NetworkObject>();
        }

        private void OnCollisionEnter(Collision other)
        {
            StartCoroutine(DestroyProjectile());
        }


        private void Update()
        {
            if (this.transform.position.y < yDestroy)
                StartCoroutine(DestroyProjectile());
        }

        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(.01f);
            var particle = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            _networkObject.Despawn();
            yield return new WaitForEndOfFrame();
            Destroy(this.gameObject);
        }
    }
}
