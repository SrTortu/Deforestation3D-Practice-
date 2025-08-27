using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation.Audio;
using Deforestation.Machine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(GameText))]
public class IADialogueTrigger : MonoBehaviour
{
    [SerializeField] private bool _shouldKeepActive;
    private GameText _text;


    private void Awake()
    {
        _text = GetComponent<GameText>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || (other.tag == "Machine" && MachineController.Instance.IsMoving()))
        {
            StartCoroutine(IATextController.Instance.StartDialogue(_text));
            AudioController.Instance.PlayIncomeCall();
            if (!_shouldKeepActive)
            {
                gameObject.SetActive(false);
            }
        }
    }
}