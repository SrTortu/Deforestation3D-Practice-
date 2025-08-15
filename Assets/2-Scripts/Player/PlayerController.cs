using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        }

        private void GetDamage(float damage = 0)
        {
            _healthSystem.TakeDamage(damage);
        }

        
    }
}