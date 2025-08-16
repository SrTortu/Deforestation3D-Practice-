using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using UnityEngine;
using Tree = UnityEngine.Tree;

public class MachineGravity : MonoBehaviour
{
    [SerializeField] private Animator _machineAnimator;
    [SerializeField] private Rigidbody _machineRigidbody;
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _fallDistance;

    private bool _isGrounded = true;

    private void Update()
    {
        CheckGround();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            _machineAnimator.applyRootMotion = _isGrounded;
            ;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            _machineAnimator.applyRootMotion = _isGrounded;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            _machineAnimator.applyRootMotion = _isGrounded;
        }
    }


    private void CheckGround()
    {
        RaycastHit hit;
        Vector3 direction = -transform.up;

        // Dibuja el rayo en el editor
        Debug.DrawRay(transform.position, direction * _fallDistance, Color.red);

        // Calcula la m�scara de la capa correctamente
        int layerMask = 1 << LayerMask.NameToLayer("Terrain");

        // Lanza un rayo hacia abajo desde la posici�n del objeto
        if (!Physics.Raycast(transform.position, direction, out hit, _fallDistance, layerMask))
        {
            _machineRigidbody.AddForce(direction * _gravityForce, ForceMode.Impulse);
        }
    }
}