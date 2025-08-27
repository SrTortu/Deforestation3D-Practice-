using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation.Machine;
using Deforestation.UI;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player" || (other.tag == "Machine" && MachineController.Instance.IsMoving()))
      {
         StartCoroutine(UIGameController.Instance.ShowEnd());

      }
   }
}
