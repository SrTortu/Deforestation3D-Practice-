using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deforestation
{
    [RequireComponent(typeof(HealthSystem))]
    public class PlayerController : MonoBehaviour
    {
        public HealthSystem HealthSystem => _healthSystem;

        [SerializeField] private HealthSystem _healthSystem;

        void Start()
        {
            GameController.Instance.OnPlayerDamage += GetDamage;
            _healthSystem.OnDeath += PlayerDeath;
        }

        private void GetDamage(float damage = 0)
        {
            _healthSystem.TakeDamage(damage);
        }

        private void PlayerDeath()
        {
            
        }
    }
}