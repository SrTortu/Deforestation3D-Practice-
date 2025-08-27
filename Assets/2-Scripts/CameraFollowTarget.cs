using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using Deforestation.Machine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || (other.tag == "Machine") && MachineController.Instance.IsMoving()))
        {
            StartCoroutine(GameController.Instance.CameraLookAt(_target));
        }
    }
}