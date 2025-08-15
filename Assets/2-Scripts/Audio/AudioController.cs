using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

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

        [Header("MachineFX")] [SerializeField] private AudioSource _steps;
        [SerializeField] private AudioSource _machineOn;
        [SerializeField] private AudioSource _machineOff;
        [SerializeField] private AudioSource _motorMachine;
        [SerializeField] private AudioSource _motorMachine2;
        [SerializeField] private float _fadeTime = 0.2f;
        [SerializeField] private float _tarjetPitch = 0.2f;
        [SerializeField] private float _pitchAceleration = 0.2f;
        [SerializeField] private float _minPitch = 0f;
        [SerializeField] private float _maxPitch = 2f;
        [SerializeField] private AudioSource _shoot;
        [SerializeField] private AudioSource _outAmmo;
        [SerializeField] private AudioSource _MachineDie;
        [SerializeField] private AudioSource _MachineTakeDamage;

        [Space(20)] [Header("PlayerFx")] [SerializeField]
        private AudioSource _playerDie;

        [SerializeField] private AudioSource _playerTakeDamage;

        [Space(20)] [Header("GameFx")] [SerializeField]
        private AudioSource _textFX;

        [SerializeField] private AudioSource _buttonClick;
        [SerializeField] private AudioSource _buttonEnter;

        [Space(20)] [Header("Music")] [SerializeField]
        private AudioSource _musicMachine;

        [SerializeField] private AudioSource _musicHuman;
        
        private AudioSource currentSource;
        private AudioSource nextSource;
        private bool _isMachineMode = false;


        #endregion

        #region Properties

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            GameController.Instance.OnMachineModeChange += SetMachineMusicState;
            GameController.Instance.MachineController.OnMachineDriveChange += SetMachineDriveEffect;
            GameController.Instance.MachineController.OnMachineDriveChange += PlayMotorMachine;
            GameController.Instance.MachineController.WeaponController.OnMachineShoot += ShootFX;
        }

        private void Start()
        {
            _musicHuman.Play();
            
            currentSource = _motorMachine;
            nextSource = _motorMachine2;
        }

        private void Update()
        {
           
            if (Input.GetKey(KeyCode.W) && GameController.Instance.MachineController.IsMoving())
            {
                _tarjetPitch = Mathf.Min(_tarjetPitch + _pitchAceleration * Time.deltaTime, _maxPitch);
            }
            else
            {
                _tarjetPitch = Mathf.Max(_tarjetPitch - (_pitchAceleration*2) * Time.deltaTime, _minPitch);
            }
            
            currentSource.pitch = _tarjetPitch;
            nextSource.pitch = _tarjetPitch;
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

        public void PlayOutAmmo()
        {
            _outAmmo.Play();
        }

        public void PlayMotorMachine(bool state)
        {
            if (state)
            {
                _isMachineMode = state;
                StartCoroutine(DoCrossfade());
            }
            else
            {
                _isMachineMode = state;
            }
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

        private IEnumerator DoCrossfade()
        {
            while (_isMachineMode)
            {
                AudioSource tempSource = currentSource;
                currentSource = nextSource;
                nextSource = tempSource;

                currentSource.Play();

                float timer = 0f;
                while (timer < _fadeTime)
                {
                    currentSource.volume = Mathf.Lerp(0f, 1f, timer / _fadeTime);
                    nextSource.volume = Mathf.Lerp(1f, 0f, timer / _fadeTime);

                    timer += Time.deltaTime;
                    yield return null;
                }

                currentSource.volume = 1f;
                nextSource.volume = 0f;

                nextSource.Stop();
                nextSource.time = 0;
            }
            
            Debug.Log("Sali");
            
        }
    
        #endregion
    }
}