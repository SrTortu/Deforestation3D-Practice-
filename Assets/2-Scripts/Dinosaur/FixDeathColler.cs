using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using UnityEngine;


public class FixDeathColler : MonoBehaviour
{
   private HealthSystem _healthSystem;
   private void Start()
   {
      _healthSystem = GetComponent<HealthSystem>();
      _healthSystem.OnDeath += MadeTriggerCollider;
   }

   private void MadeTriggerCollider()
   {
      BoxCollider collider = GetComponent<BoxCollider>();
      collider.isTrigger = true;
      
   }
}
