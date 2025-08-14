using System.Collections;
using System.Collections.Generic;
using Deforestation.Audio;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private TextMeshProUGUI _continueText;
    
    [SerializeField] private GameText _gameText;
    [SerializeField] private float _timeAppear;
    
    private bool _continue = false;

    void Start()
    {
        StartCoroutine(ControlTextAppear());
    }

    void Update()
    {
        if (_continue && Input.anyKeyDown)
        {
            _continue = true;
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator ControlTextAppear()
    {
        _screenText.text = " ";
        foreach (char character in _gameText.Text)
        {
            _screenText.text += character;
            yield return new WaitForSeconds(_timeAppear);
        }
        AudioController.Instance.TextFx.Stop();
        StartCoroutine(ContinueTextAppear());
        _continue = true;
    }

    IEnumerator ContinueTextAppear()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f); 
            _continueText.DOFade(1, 1.5f);
            yield return new WaitForSeconds(1.5f);
            _continueText.DOFade(0, 1.5f);
            
        }
    }
}