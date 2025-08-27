using System;
using System.Collections;
using System.Collections.Generic;
using Deforestation;
using UnityEngine;

public class LoadStartScene : MonoBehaviour
{
    private TextAnimator _textAnimator;

    private void Awake()
    {
        _textAnimator = GetComponent<TextAnimator>();
    }

    private void Start()
    {
        _textAnimator.onKeyDown += () => GameController.Instance.LoadStartScene();
    }
}
