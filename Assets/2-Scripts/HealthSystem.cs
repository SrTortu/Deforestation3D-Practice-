using System;
using UnityEngine;

namespace Deforestation
{

	public class HealthSystem : MonoBehaviour
	{
		public event Action<float> OnHealthChanged;
		public bool IsDead => _isDead;
		public event Action OnDeath;

		[SerializeField]
		private float _maxHealth = 100f;
		private float _currentHealth;
		private bool _isDead = false;

		private void Awake()
		{
			_currentHealth = _maxHealth;
		}

		private void Update()
		{
			if (_isDead)
			{
				return;
			}
			if (_currentHealth <= 0)
			{
				Die();
				_isDead = true;
			}
		}

		public void TakeDamage(float damage)
		{
			_currentHealth -= damage;
			OnHealthChanged?.Invoke(_currentHealth);

			
		}

		public void Heal(float amount)
		{
			_currentHealth += amount;
			_currentHealth = Mathf.Min(_currentHealth, _maxHealth);
			OnHealthChanged?.Invoke(_currentHealth);
		}

		public void SetHealth(float value)
		{
			_currentHealth = value;
			_currentHealth = Mathf.Min(_currentHealth, _maxHealth);
			OnHealthChanged?.Invoke(_currentHealth);
		}

		private void Die()
		{
			OnDeath?.Invoke();
		}
	}

}