using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class WaterDetection : MonoBehaviour
{
    [SerializeField] private float _damageInterval = 1f;
    private float _lastTimeDamage = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Water")
        {
            return;
        }

        if (Time.time - _lastTimeDamage < _damageInterval)
        {
            return;
        }

        _lastTimeDamage = Time.time;
        
        if (this.gameObject.CompareTag("Player"))
        {
            GameController.Instance.PlayerDamage(10);
            return;
        }

        if (this.gameObject.CompareTag("Machine"))
        {
            GameController.Instance.MachineController.HealthSystem.TakeDamage(10);
            return;
        }
        
        
    }
}
