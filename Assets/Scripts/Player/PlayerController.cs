using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deforestation
{
    [RequireComponent(typeof(HealthSystem))]
    public class PlayerController : MonoBehaviour
    {
        public HealthSystem HealthSystem => _healthSystem;
        public CharacterController PlayerMoveController => _player;
        
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private CharacterController _player;

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