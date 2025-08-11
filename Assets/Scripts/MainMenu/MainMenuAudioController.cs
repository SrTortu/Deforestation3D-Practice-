using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _buttonOnEnterMouseSound;

    public void playButtonOnEnterMouseSound()
    {
        _buttonOnEnterMouseSound.Play();
    }
}
