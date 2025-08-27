using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointRotationFix : MonoBehaviour
{
   private void Update()
   {
      transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
   }
}
