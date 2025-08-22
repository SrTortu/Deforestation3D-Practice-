using System;
using UnityEngine;
using UnityEngine.AI;

namespace Deforestation.Dinosaurus
{
    public enum DinosaurType
    {
        Pachycephalausaurus,
        Raptor
    }

    public class DinosaurController : Dinosaur
    {
        #region Fields

        [SerializeField] private float _distanceDetection = 50;
        [SerializeField] private float _attackDistance = 10;
        [SerializeField] private Transform _targetRunTransform;
        [SerializeField] private DinosaurType _dinosaurType;

        private Transform _targetAttackTransform;
        [SerializeField] private bool _chase;
        [SerializeField] private bool _attack;
        [SerializeField] private bool _runAway;
         private float _attackColdDown;

        [SerializeField] private float _attackTime = 2;
        [SerializeField] private float _attackDamage = 5;

        #endregion


        private void Start()
        {
            _attackColdDown = _attackTime;

            if (_dinosaurType == DinosaurType.Raptor)
            {
                _targetAttackTransform = TargetController.Instance.PlayerTarget();
            }

            if (_dinosaurType == DinosaurType.Pachycephalausaurus)
            {
                _targetAttackTransform = TargetController.Instance.MachineTarget();
            }
        }

        private void Update()
        {
            float distanceTarget = Vector3.Distance(transform.position, _targetAttackTransform.position);

            //Chase
            if (!_chase && !_attack && distanceTarget <
                _distanceDetection)
            {
                Run(_targetAttackTransform.position);
                Debug.Log(" 1");
                return;
            }

            //chase
            if (_chase || _attack)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(_targetAttackTransform.position, out hit, _attackDistance, 1))
                {
                    Run(hit.position);
                    Debug.Log(" 2");
                }
            }

            //Runaway
            if (_chase && _dinosaurType == DinosaurType.Raptor && GameController.Instance.MachineModeOn &&
                distanceTarget < _distanceDetection)
            {
                Run(_targetRunTransform.position);
                Debug.Log(" 3");
                return;
            }

            //Attack
            if ((_chase || _attack) && distanceTarget < _attackDistance)
            {
                AttackAnim();
                MakeDamage();
                Debug.Log(" 4");
            }

            //Idl
            if (_chase && distanceTarget > _distanceDetection)
            {
                Debug.Log(" 5");
                IdleAnim();
                return;
            }
        }


        private void MakeDamage()
        {
            _attackColdDown -= Time.deltaTime;
            if (_attackColdDown <= 0)
            {
                _attackColdDown = _attackTime;
                if (_dinosaurType == DinosaurType.Raptor)
                {
                    GameController.Instance.PlayerController.GetDamage(10);
                }

                if (_dinosaurType == DinosaurType.Pachycephalausaurus)
                {
                    GameController.Instance.MachineController.GetDamage(100);
                }
            }
        }


        private void IdleAnim()
        {
            _anim.SetBool("Run", false);
            _agent.isStopped = true;
            _chase = false;
            _attack = false;
        }

        private void Run(Vector3 destination)
        {
            _anim.SetBool("Run", true);
            _agent.SetDestination(destination);
            _agent.isStopped = false;
            _chase = true;
            _attack = false;
        }

        private void AttackAnim()
        {
            _anim.SetBool("Run", false);
            _anim.SetTrigger("Attack");
            _agent.isStopped = true;
            _chase = false;
            _attack = true;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _distanceDetection);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }
    }
}