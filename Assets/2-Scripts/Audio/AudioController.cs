using UnityEngine;
using DG.Tweening;
using System;

namespace Deforestation.Audio
{
    public class AudioController : Singleton<AudioController>
    {
        const float MAX_VOLUME = 0.1f;

        public AudioSource TextFx
        {
            get { return _textFX; }

            private set { _textFX = value; }
        }

        #region Fields

        [Header("FX")] [SerializeField] private AudioSource _steps;
        [SerializeField] private AudioSource _machineOn;
        [SerializeField] private AudioSource _machineOff;
        [SerializeField] private AudioSource _shoot;
        [SerializeField] private AudioSource _buttonClick;
        [SerializeField] private AudioSource _buttonEnter;
        [SerializeField] private AudioSource _textFX;

        [Space(10)] [Header("Music")] [SerializeField]
        private AudioSource _musicMachine;

        [SerializeField] private AudioSource _musicHuman;

        #endregion

        #region Properties

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            GameController.Instance.OnMachineModeChange += SetMachineMusicState;
            GameController.Instance.MachineController.OnMachineDriveChange += SetMachineDriveEffect;
            GameController.Instance.MachineController.WeaponController.OnMachineShoot += ShootFX;
        }

        private void Start()
        {
            _musicHuman.Play();
        }

        private void Update()
        {
            //TODO: MOVER A inputcontroller
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (!_steps.isPlaying)
                    _steps.Play();
            }
            else if (_steps.isPlaying)
                _steps.Stop();
        }

        #endregion

        public void ButtonClickFX()
        {
            _buttonClick.Play();
        }

        public void ButtonEnterFX()
        {
            _buttonEnter.Play();
        }


        public void TextFX()
        {
            _textFX.Play();
        }

        #region Private Methods

        private void SetMachineMusicState(bool machineMode)
        {
            if (machineMode)
            {
                _musicHuman.DOFade(0, 3);
                _musicMachine.DOFade(MAX_VOLUME, 3);
                _musicMachine.Play();
            }
            else
            {
                _musicHuman.DOFade(MAX_VOLUME, 3);
                _musicMachine.DOFade(0, 3);
            }
        }

        private void SetMachineDriveEffect(bool startDriving)
        {
            if (startDriving)
                _machineOn.Play();
            else
                _machineOff.Play();
        }

        private void ShootFX()
        {
            _shoot.Play();
        }

        #endregion
    }
}