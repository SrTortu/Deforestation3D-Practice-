using System.Collections;
using System.Collections.Generic;
using Deforestation;
using UnityEngine;

public class TargetController : Singleton<TargetController>
{
    [SerializeField] private Transform _machineTarget;
    [SerializeField] private Transform _playerTarget;

    public Transform PlayerTarget()
    {
        return _playerTarget;
    }

    public Transform MachineTarget()
    {
        return _machineTarget;
    }
}