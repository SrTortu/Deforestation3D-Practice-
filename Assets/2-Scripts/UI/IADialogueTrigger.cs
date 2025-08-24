using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(GameText))]
public class IADialogueTrigger : MonoBehaviour
{
    
     private GameText _text;

     private void Awake()
     {
         _text = GetComponent<GameText>();
     }

     private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IATextController.Instance.StartDialogue(_text);
            gameObject.SetActive(false);
        }
    }
}