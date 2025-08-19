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

        private Vector3 _doorDestination = Vector3.zero;
        private bool _isDoorOpen = false;

        #endregion

        #region Public Methods

        public void Update()
        {
            if (_type.Equals(MachineInteractionType.Door))
            {
                _doorDestination = _startPoint.position;
            }
        }

        public InteractableInfo GetInfo()
        {
            _interactableInfo.Type = _type.ToString();
            return _interactableInfo;
        }

        public virtual void Interact()
        {
            if (_type == MachineInteractionType.Door)
            {
                if (!_isDoorOpen)
                {
                    _isDoorOpen = true;
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

        public IEnumerator OpenDoor()
        {
            _isDoorOpen = true;
            AudioController.Instance.PlayOpenDoor();
            yield return StartCoroutine(MoveObject(_target));
            yield return new WaitForSeconds(1f);
            AudioController.Instance.PlayOpenDoor();
            yield return StartCoroutine(MoveObject(_startPoint));
            _isDoorOpen = false;
        }

        // Corrutina para mover el objeto hacia un Transform de destino
        private IEnumerator MoveObject(Transform endPointTransform)
        {
            while (transform.position != endPointTransform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPointTransform.position,
                    1f * Time.deltaTime);
                yield return null; 
            }
            transform.position = endPointTransform.position;
        }

        #endregion
    }
}