using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Code.Components
{
    public class ExplosionComponent : MonoBehaviour
    {
        private float startTime;
        private float journeyLength;
        [SerializeField] private float speed;
        [SerializeField] private float timeToLive;
        [SerializeField] private double explosionDamage;
        
        private void Start()
        {
            startTime = Time.time;

            journeyLength = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(4f, 4f, 4f));
        }

        private void Update()
        {
            Explooooosiiiooon();
            StartCoroutine(DestroyExplosion());
        }

        private void Explooooosiiiooon()
        {
            float distanceCovered = (Time.time - startTime) * speed;

            float fraction = distanceCovered / journeyLength;

            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(4f, 4f, 4f), fraction);
        }

        IEnumerator DestroyExplosion()
        {
            yield return new WaitForSeconds(timeToLive);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var distanceFromExplosionCenter = Vector3.Distance(transform.position, other.transform.parent.position);
            
            if (other.transform.parent.TryGetComponent(out EnemyComponent enemyComponent))
            {
                enemyComponent.InflictDamage(explosionDamage * (3f - distanceFromExplosionCenter));
            }
        }
    }
}
