using UnityEngine;
using TMPro;
using Deforestation.Recolectables;
using System;
using Deforestation.Audio;
using Deforestation.Interaction;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Deforestation.UI
{
    public class UIGameController : Singleton<UIGameController>
    {
        #region Properties

        #endregion

        #region Fields

        private Inventory _inventory => GameController.Instance.Inventory;
        private InteractionSystem _interactionSystem => GameController.Instance.InteractionSystem;

        [Header("Settings")] [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _fxSlider;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _confirmSettingsButton;
        [SerializeField] private AudioSource _buttonClickSound;
        [SerializeField] private AudioSource _buttonEnterSound;

        [Header("Inventory")] [SerializeField] private TextMeshProUGUI _crystal1Text;
        [SerializeField] private TextMeshProUGUI _crystal2Text;

        [Header("Interacytion")] [SerializeField]
        private InteractionPanel _interactionPanel;

        [Header("Live")] [SerializeField] private Slider _machineSlider;
        [SerializeField] private Slider _playerSlider;

        [Header("Reset")] [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _exitResetButton;

        private bool _settingsOn = false;
        private float _mouseLockTimer = 0f;

        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            SetMixerVolume();
            _settingsPanel.SetActive(false);

            //My Events
            _inventory.OnInventoryUpdated += UpdateUIInventory;
            _interactionSystem.OnShowInteraction += ShowInteraction;
            _interactionSystem.OnHideInteraction += HideInteraction;
            GameController.Instance.OnPlayerDeath += ShowGameOverPanel;
            //Settings events
            _settingsButton.onClick.AddListener(SwitchSettings);
            _musicSlider.onValueChanged.AddListener(MusicVolumeChange);
            _fxSlider.onValueChanged.AddListener(FXVolumeChange);
            _exitGameButton.onClick.AddListener(ExitButtonOnClick);
            _confirmSettingsButton.onClick.AddListener(ConfirmSettingsButtonOnClick);
            _exitResetButton.onClick.AddListener(ExitButtonOnClick);
            _mainMenuButton.onClick.AddListener(GoToMainMenu);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (_mouseLockTimer > 3f && Cursor.lockState == CursorLockMode.None && !_settingsOn && !_gameOverPanel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _mouseLockTimer = 0f;
            }

            _mouseLockTimer += Time.deltaTime;
        }

        private void ShowGameOverPanel()
        {
            _gameOverPanel.SetActive(true);
        }

        private void SwitchSettings()
        {
            AudioController.Instance.ButtonClickFX();
            _settingsOn = !_settingsOn;
            _settingsPanel.SetActive(_settingsOn);
        }

        internal void UpdateMachineHealth(float value)
        {
            _machineSlider.value = value;
        }

        internal void UpdatePlayerHealth(float value)
        {
            _playerSlider.value = value;
        }

        #endregion

        #region Public Methods

        public void PlayButtonEnterSound()
        {
            AudioController.Instance.ButtonEnterFX();
        }

        public void ShowInteraction(string message)
        {
            _interactionPanel.Show(message);
        }

        public void HideInteraction()
        {
            _interactionPanel.Hide();
        }

        #endregion

        #region Private Methods

        private void UpdateUIInventory()
        {
            if (_inventory.InventoryStack.ContainsKey(RecolectableType.SuperCrystal))
                _crystal1Text.text = _inventory.InventoryStack[RecolectableType.SuperCrystal].ToString();
            else
                _crystal1Text.text = "0";
            if (_inventory.InventoryStack.ContainsKey(RecolectableType.HyperCrystal))
                _crystal2Text.text = _inventory.InventoryStack[RecolectableType.HyperCrystal].ToString();
            else
                _crystal2Text.text = "0";
        }

        private void FXVolumeChange(float value)
        {
            _mixer.SetFloat("FXVolume", value);
        }

        private void MusicVolumeChange(float value)
        {
            _mixer.SetFloat("MusicVolume", value);
        }

        private void SetMixerVolume()
        {
            float musicValue;
            _mixer.GetFloat("MusicVolume", out musicValue);
            _musicSlider.value = musicValue;
            float fxValue;
            _mixer.GetFloat("FXVolume", out fxValue);
            _fxSlider.value = fxValue;
        }

        private void ConfirmSettingsButtonOnClick()
        {
            AudioController.Instance.ButtonClickFX();
            _settingsOn = !_settingsOn;
            _settingsPanel.SetActive(_settingsOn);
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _mouseLockTimer = 0f;
            }
        }

        private void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        private void ExitButtonOnClick()
        {
            AudioController.Instance.ButtonClickFX();
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        #endregion
    }
}