using System;
using Deforestation.Dinosaurus;
using Deforestation.Recolectables;
using UnityEngine;

namespace Deforestation.Machine
{
    public class MachineMovement : MonoBehaviour
    {
        public bool IsMoving => _isMoving;
        public bool IsJumping => _isJumping;

        #region Fields

        [SerializeField] private float _speedForce = 50;
        [SerializeField] private float _speedRotation = 15;
        [SerializeField] private float _jumpForce = 15;
        [SerializeField] private Animator _machineAnimator;
        private Rigidbody _rb;
        private MachineGravity _machineGravity;
        private Vector3 _movementDirection;
        private bool _isMoving;
        private bool _isGrounded = false;
        private bool _isJumping = false;
        private Inventory _inventory => GameController.Instance.Inventory;

        [Header("Energy")] [SerializeField] private float energyDecayRate = 20f;
        private float energyTimer = 0f;

        #endregion


        #region Unity Callbacks

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _machineGravity = GetComponent<MachineGravity>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space) && _isMoving && _machineGravity.IsGrounded)
            {
                
                _isJumping = true;
              
            }
            if (_inventory.HasResource(RecolectableType.HyperCrystal))
            {
                //Movement
                _movementDirection = new Vector3(Input.GetAxis("Vertical"), 0, 0);
                transform.Rotate(Vector3.up * _speedRotation * Time.deltaTime * Input.GetAxis("Horizontal"));
                Debug.DrawRay(transform.position,
                    transform.InverseTransformDirection(_movementDirection.normalized) * _speedForce);
                _isMoving = true;

                //Energy
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    energyTimer += Time.deltaTime;
                    if (energyTimer >= energyDecayRate)
                        _inventory.UseResource(RecolectableType.HyperCrystal);
                }
            }
            else
            {
                GameController.Instance.MachineController.StopMoving();
                _isMoving = false;
            }
            
        }

        private void FixedUpdate()
        {
            _rb.AddRelativeForce(_movementDirection.normalized * _speedForce, ForceMode.Impulse);
            if (_isJumping)
            {
                Jump();
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tree")
            {
                int index = other.GetComponent<Tree>().Index;
                GameController.Instance.TerrainController.DestroyTree(index, other.transform.position);
            }
        }
        

        private void Jump()
        {
            _machineAnimator.SetTrigger("Jump");
            _rb.AddRelativeForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isJumping = false;
        }

        #endregion


        private void OnDrawGizmos()
        {
        }
    }
}