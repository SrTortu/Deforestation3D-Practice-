using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Main Panel Menu")] [Space(20)] [SerializeField]
    private GameObject _mainPanel;

    [Header("Buttons")] [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;


    [Space(20)] [Header("Sound")] [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _buttonClickSound;
    [SerializeField] private AudioSource _buttonEnterSound;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _fxSlider;

    [Space(20)] [Header("Settings")] [SerializeField]
    private Button _settingsOkButton;

    [SerializeField] private GameObject _settingsPanel;


    private void Start()
    {
        _playButton.onClick.AddListener(PlayButtonOnClick);
        _settingsButton.onClick.AddListener(SettingsButtonOnClick);
        _exitButton.onClick.AddListener(ExitButtonOnClick);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _fxSlider.onValueChanged.AddListener(SetFxVolume);
        _settingsOkButton.onClick.AddListener(CloseSettingsPanel);
    }

    private void PlayButtonOnClick()
    {
        _buttonClickSound.Play();
        SceneManager.LoadScene(1);
    }

    private void SettingsButtonOnClick()
    {
        _buttonClickSound.Play();
        _settingsPanel.SetActive(true);
        _mainPanel.SetActive(false);
    }

    private void ExitButtonOnClick()
    {
        _buttonClickSound.Play();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetFxVolume(float volume)
    {
        _audioMixer.SetFloat("FxVolume", volume);
    }

    private void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    private void CloseSettingsPanel()
    {
        _buttonClickSound.Play();
        _settingsPanel.SetActive(false);
        _mainPanel.SetActive(true);
    }
}