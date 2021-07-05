using System;
using GameManagers;
using UnityEngine;

namespace Controllers
{
    public class AttackController : MonoBehaviour
    {
        private UnitController _unitController;
        private Animator _animator;
        private float _attackTimer;

        private void Start()
        {
            _unitController = GetComponent<UnitController>();
            _attackTimer = 0;     
            _animator = GetComponent<Animator>();
            _animator.SetBool("isAttacking", true);
        }

        private void Update()
        {
            _attackTimer += Time.deltaTime;
            if (_unitController.Target != null && 
                (_unitController.Target.GetComponent<Collider>().ClosestPoint(transform.position) - transform.position)
                .magnitude <= _unitController.Stats.attackRange)
            {
                if (_attackTimer < _unitController.Stats.attackTimeout) return;
                _attackTimer = 0;
                switch (_unitController.Target.tag)
                {
                    case "PlayerUnit":
                    case "EnemyUnit":
                        RTSGameManager.UnitTakeDamage(_unitController,
                            _unitController.Target.GetComponent<UnitController>());
                        break;
                    case "PlayerCastle":
                    case "EnemyCastle":
                        RTSGameManager.CastleTakeDamage(_unitController,
                            _unitController.Target.GetComponent<CastleController>());
                        break;
                }
            }
            else _unitController.SetState(UnitController.UnitStates.Idle);
        }

        private void OnDestroy()
        {
            _animator.SetBool("isAttacking", false);
        }
    }
}