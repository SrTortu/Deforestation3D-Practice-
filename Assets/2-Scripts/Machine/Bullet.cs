using UnityEngine;
using System;

namespace Deforestation.Machine.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        #region Properties
        #endregion

        #region Fields
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private float _force = 100;
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private LayerMask _enemyLayer;
        private Rigidbody _rb;
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.AddForce(transform.forward * _force, ForceMode.Impulse);
        }
        private void OnTriggerEnter(Collider other)
        {
            Explode();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        #endregion
        

        #region Private Methods
        private void Explode()
        {
            Debug.unityLogger.Log("Explode");
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (Collider hit in colliders)
            {
                Debug.unityLogger.Log("Hit");
                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null && !hit.CompareTag("Machine"))
                {
                    health.TakeDamage(_damage);
                    Debug.Log(_damage+"daño");
                }
            }
        }
        #endregion
    }
}