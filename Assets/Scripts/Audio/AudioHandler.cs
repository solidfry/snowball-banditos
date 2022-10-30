using System.Collections;
using System.Collections.Generic;
using Abstracts;
using Events;
using ScriptableObjects;
using UnityEngine;

namespace Audio
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private ConstructionMaterialType constructionMaterial;
        List<AudioClip> _clips;
        [SerializeField] private float clipPlayWaitTime = 0.5f;
        private bool _isAvailable = true;

        private void Start()
        {
            AddConstructionType();
            _clips = this.constructionMaterial.audioClips;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_isAvailable == false)
                return;
            
            StartCoroutine(StartCooldown());
            AudioClip randomClip = GetRandomClip(_clips);
            GameEvents.OnAudioCollisionEvent?.Invoke(randomClip);
        }

        AudioClip GetRandomClip(List<AudioClip> listOfClips)
        {
            AudioClip clip = listOfClips[Random.Range(0, listOfClips.Count)];
//            Debug.Log("We got a random clip to play " + clip);
            return clip;
        }

        void AddConstructionType()
        {
            if (constructionMaterial == null && GetComponent<Receptacle>())
            {
                constructionMaterial = GetComponent<Receptacle>().ConstructionMaterial;
            }
        }

        IEnumerator StartCooldown()
        {
            _isAvailable = false;
            yield return new WaitForSeconds(clipPlayWaitTime);
            _isAvailable = true;
        }
    }
}