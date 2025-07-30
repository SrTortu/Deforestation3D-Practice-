using UnityEngine;
using Deforestation.Machine;
using Deforestation.UI;
using Deforestation.Recolectables;
using Deforestation.Interaction;
using Cinemachine;
using System;

namespace Deforestation
{
    public class GameController : Singleton<GameController>
    {
        #region Properties

        public MachineController MachineController => _machine;
        public Inventory Inventory => _inventory;
        public InteractionSystem InteractionSystem => _interactionSystem;
        public TreeTerrainController TerrainController => _terrainController;
        public Camera MainCamera;

        //Events
        public Action<bool> OnMachineModeChange;
        public Action<float> OnPlayerDamage;
        public Action OnPlayerDeath;

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

        [Header("Machine")] [SerializeField] protected MachineController _machine;
        [Header("UI")] [SerializeField] protected UIGameController _uiController;

        [Header("Trees Terrain")] [SerializeField]
        protected TreeTerrainController _terrainController;

        private bool _machineModeOn;

        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            //UI Update
            _playerController.HealthSystem.OnHealthChanged += _uiController.UpdatePlayerHealth;
            _machine.HealthSystem.OnHealthChanged += _uiController.UpdateMachineHealth;
            MachineModeOn = false;
        }

        // Update is called once per frame
        void Update()
        {
        }

        #endregion

        #region Public Methods

        public void TeleportPlayer(Vector3 target)
        {
            _playerController.enabled = false;
            _playerController.transform.position = target;
            _playerController.enabled = true;
        }

        public void PlayerDamage(float damage)
        {
            OnPlayerDamage?.Invoke(damage);
        }

        public void PlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        internal void MachineMode(bool machineMode)
        {
            MachineModeOn = machineMode;
            //Player
            _playerController.gameObject.SetActive(!machineMode);
            _playerController.enabled = !machineMode;

            //Cursor + UI
            if (machineMode)
            {
                //Start Driving
                if (Inventory.HasResource(RecolectableType.HyperCrystal))
                    _machine.StartDriving(machineMode);

                _playerController.transform.parent = _machineFollow;
                _uiController.HideInteraction();
                Cursor.lockState = CursorLockMode.None;
                //Camera
                _virtualCamera.Follow = _machineFollow;

                _machine.enabled = true;
                _machine.WeaponController.enabled = true;
                _machine.GetComponent<MachineMovement>().enabled = true;
            }
            else
            {
                _machine.enabled = false;
                _machine.WeaponController.enabled = false;
                _machine.GetComponent<MachineMovement>().enabled = false;
                _playerController.transform.parent = null;

                //Camera
                _virtualCamera.Follow = _playerFollow;
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = machineMode;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}