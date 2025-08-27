using UnityEngine;
using System;
using Deforestation.Audio;
using Deforestation.Machine.Weapon;

namespace Deforestation.Machine
{
	[RequireComponent (typeof(HealthSystem))]
	public class MachineController : Singleton<MachineController>
	{
		#region Properties
		public HealthSystem HealthSystem => _health;
		public WeaponController WeaponController;
		public Action<bool> OnMachineDriveChange;
		public bool IsGrounded {get; set; }

		#endregion

		#region Fields
		
		[SerializeField] private float _gravityForce;
		
		private HealthSystem _health;
		private MachineMovement _movement;
		private Animator _anim;
		private Rigidbody _rigidbody;
		

		#endregion

		#region Unity Callbacks
		private void Awake()
		{
			_health = GetComponent<HealthSystem>();
			_movement = GetComponent<MachineMovement>();
			_anim = GetComponent<Animator>();
			_rigidbody = GetComponent<Rigidbody>();

		}
		// Start is called before the first frame update
		void Start()
		{
			_movement.enabled = false;
			_health.OnDeath += Die;
		}

		// Update is called once per frame
		void Update()
		{
			//TODO: Mover a Input System
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				StopDriving();
			}

			if (!IsGrounded)
			{
				Vector3 direction = -transform.up;
				_rigidbody.AddForce(direction * _gravityForce, ForceMode.Impulse);
			}
		}		

		#endregion

		#region Public Methods
		public void StopDriving()
		{
			OnMachineDriveChange?.Invoke(false);
			GameController.Instance.MachineMode(false);
			StopMoving();

		}

		public void StartDriving(bool machineMode)
		{
			
			enabled = machineMode;
			_movement.enabled = machineMode;
			_anim.SetTrigger("WakeUp");
			_anim.SetBool("Move", machineMode);
			OnMachineDriveChange?.Invoke(true);
		}

		public void StopMoving()
		{
			_movement.enabled = false;
			_movement.IsMoving = false;
			_anim.SetBool("Move", false);
		}
		public void GetDamage(float damage = 0)
		{
			_health.TakeDamage(damage);
			AudioController.Instance.MachineDamage();
		}

		public void Die()
		{
			AudioController.Instance.MachineDie();
		}

		public bool IsMoving()
		{
			return _movement.IsMoving;
		}

		public void JumpAnim()
		{
			_anim.SetTrigger("Jump");
		}
		#endregion

	}

}