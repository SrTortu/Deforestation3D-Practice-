using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(IATextAnimator))]
public class IATextController : Singleton<IATextController>
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _panel;


    private IATextAnimator _animator;

    private void Awake()
    {
        _animator = GetComponent<IATextAnimator>();
    }

   public IEnumerator StartDialogue(GameText gameText)
    {
        while (_animator.CorrutineIsOn)
        {
            yield return null;
        } 
        StartCoroutine(_animator.ControlTextAppear(_textBox, _titleText, gameText, _panel));
    }
}