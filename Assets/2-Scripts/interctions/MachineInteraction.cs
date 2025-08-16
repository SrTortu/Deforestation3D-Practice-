using UnityEngine;
using System;
using System.Collections;
using Deforestation.Audio;
using DG.Tweening;

namespace Deforestation.Interaction
{
    public enum MachineInteractionType
    {
        Door,
        Stairs,
        Machine
    }

    public class MachineInteraction : MonoBehaviour, IInteractable
    {
        #region Properties

        #endregion

        #region Fields

        [SerializeField] protected MachineInteractionType _type;
        [SerializeField] protected Transform _target;
        [SerializeField] protected Transform _startPoint;
        [SerializeField] protected InteractableInfo _interactableInfo;

        private bool _isOpenDoor = false;
        #endregion

        #region Public Methods
        

        public InteractableInfo GetInfo()
        {
            _interactableInfo.Type = _type.ToString();
            return _interactableInfo;
        }

        public virtual void Interact()
        {
            if (_type == MachineInteractionType.Door)
            {
                if (!_isOpenDoor)
                {
                    _isOpenDoor = true;
                    StartCoroutine(OpenDoor());
                }
            }

            if (_type == MachineInteractionType.Stairs)
            {
                //Teleport Player
                GameController.Instance.TeleportPlayer(_target.position);
            }

            if (_type == MachineInteractionType.Machine)
            {
                GameController.Instance.MachineMode(true);
            }
        }

        IEnumerator OpenDoor()
        {
            transform.DOMove(_target.transform.position, 3f);
            AudioController.Instance.PlayOpenDoor();
            yield return new WaitForSeconds(3f);
            AudioController.Instance.PlayOpenDoor();
            transform.DOMove(_startPoint.position, 3f);
            yield return new WaitForSeconds(3f);
            _isOpenDoor = false;

        }

        #endregion
    }
}