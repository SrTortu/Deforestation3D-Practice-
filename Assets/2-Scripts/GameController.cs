using UnityEngine;
using Deforestation.Machine;
using Deforestation.UI;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Cinemachine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Deforestation
{
    public class GameController : Singleton<GameController>
    {
        #region Properties

        public MachineController MachineController => _machine;
        public PlayerController PlayerController => _playerController;
        public Inventory Inventory => _inventory;
        public InteractionSystem InteractionSystem => _interactionSystem;
        public TreeTerrainController TerrainController => _terrainController;
        public Camera MainCamera;
        public Transform LookAtTarget;

        //Events
        public Action<bool> OnMachineModeChange;

        public bool MachineModeOn
        {
            get { return _machineModeOn; }
            private set
            {
                _machineModeOn = value;
                OnMachineModeChange?.Invoke(_machineModeOn);
            }
        }

        #endregion

        #region Fields

        [Header("Player")] [SerializeField] protected PlayerController _playerController;

        [SerializeField] protected Inventory _inventory;
        [SerializeField] protected InteractionSystem _interactionSystem;

        [Header("Camera")] [SerializeField] protected CinemachineVirtualCamera _virtualCamera;
        [SerializeField] protected Transform _playerFollow;
        [SerializeField] protected Transform _machineFollow;
        [SerializeField] private Transform _machineFollowBackUp;
        [SerializeField] private Cinemachine3rdPersonFollow _transposer;

        [Header("Machine")] [SerializeField] protected MachineController _machine;
        [SerializeField] protected Transform _playerRespawn;

        [Header("UI")] [SerializeField] protected UIGameController _uiController;

        [Header("Trees Terrain")] [SerializeField]
        protected TreeTerrainController _terrainController;

        private bool _machineModeOn;

        #endregion

        #region Unity Callbacks

        // Start is called before the first frame pdate
        void Start()
        {
            //UI Update
            _playerController.HealthSystem.OnHealthChanged += _uiController.UpdatePlayerHealth;
            _machine.HealthSystem.OnHealthChanged += _uiController.UpdateMachineHealth;
            _playerController.HealthSystem.OnDeath += PlayerDeath;
            _machine.HealthSystem.OnDeath += PlayerDeath;
            MachineModeOn = false;
            _transposer = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region Public Methods

        public void TeleportPlayer(Vector3 target)
        {
            _playerController.enabled = false;
            _playerController.transform.position = target;
            _playerController.enabled = true;
        }


        public void PlayerDeath()
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }

        public void PlayGameTime()
        {
            Time.timeScale = 1f;
        }

        internal void MachineMode(bool machineMode)
        {
            MachineModeOn = machineMode;
            _machine.enabled = machineMode;
            //Player
            _playerController.gameObject.SetActive(!machineMode);
            _playerController.enabled = !machineMode;
            _interactionSystem.enabled = false;

            //Cursor + UI
            if (machineMode)
            {
                //Start Driving
                if (Inventory.HasResource(RecolectableType.HyperCrystal))
                {
                    _machine.StartDriving(machineMode);
                }

                _playerController.transform.parent = _playerRespawn;
                _uiController.HideInteraction();
                Cursor.lockState = CursorLockMode.None;
                //Camera
                _virtualCamera.Follow = _machineFollow;
                StartCoroutine(CameraLookAt(LookAtTarget));
                _transposer.Damping = new Vector3(3, 3, 3);


                _machine.enabled = true;
                _machine.WeaponController.enabled = true;
                _machine.GetComponent<MachineMovement>().enabled = true;
            }
            else
            {
                CameraUnLookAt();
                _machine.WeaponController.enabled = false;
                _machine.GetComponent<MachineMovement>().enabled = false;
                _playerController.transform.parent = null;
                _interactionSystem.enabled = true;
                _machine.enabled = false;
                _transposer.Damping = Vector3.zero;
                //Camera
                _virtualCamera.Follow = _playerFollow;
                _playerController.transform.rotation = Quaternion.identity;
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = machineMode;
        }

        #endregion


        public IEnumerator EndGame()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(0);
        }

        public IEnumerator CameraLookAt(Transform target)
        {
            HealthSystem healthTarget = target.GetComponent<HealthSystem>();
            while (!healthTarget.IsDead)
            {
                _machineFollow.LookAt(target);
                yield return null;
            }

            CameraUnLookAt();
        }

        public void CameraUnLookAt()
        {
            _machineFollow.position = _machineFollowBackUp.position;
            _machineFollow.rotation = _machineFollowBackUp.rotation;
        }
    }
}