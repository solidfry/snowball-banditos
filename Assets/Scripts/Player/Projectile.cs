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
            _networkObject = GetComponent<NetworkObject>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsHost)
            {
                StartCoroutine(DestroyProjectile());
            }
        }


        private void Update()
        {
            if (this.transform.position.y < yDestroy && IsHost)
                StartCoroutine(DestroyProjectile());
        }

        private IEnumerator DestroyProjectile()
        {

            var particle = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            particle.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSeconds(.5f);
            particle.GetComponent<NetworkObject>().Despawn();
            this.NetworkObject.Despawn();

            yield return new WaitForSeconds(.2f);
            Destroy(particle.gameObject);
            Destroy(this.gameObject);

        }
    }
}
