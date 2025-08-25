using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
  [SerializeField] private float _gravity;
  private Rigidbody _rB;

  private void Awake()
  {
    _rB = GetComponent<Rigidbody>();
  }
  
  
}
