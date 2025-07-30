using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
   private void OnTriggerStay(Collider other)
   {
      if (other.tag == "Water")
      {
         Debug.Log("Water");
      }
   }
}
