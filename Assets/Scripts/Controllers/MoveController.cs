using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class MoveController : MonoBehaviour
    {
        private UnitController _unitController;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private void Start()
        {
            _unitController = GetComponent<UnitController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _unitController.Stats.speed;
            _navMeshAgent.destination = _unitController.Destination;
            _animator = GetComponent<Animator>();
            _animator.SetBool("isWalking", true);
            _navMeshAgent.stoppingDistance = _unitController.Target != null
                ? _unitController.Stats.attackRange
                : _unitController.Stats.stoppingDistance;
        }

        private void Update()
        {
            if (_unitController.Target != null && _unitController.Target.position != _navMeshAgent.destination)
            {
                switch (_unitController.Target.tag)
                {
                    case "PlayerUnit":
                    case "EnemyUnit":
                        _navMeshAgent.destination = _unitController.Target.position;
                        break;
                    case "PlayerCastle":
                    case "EnemyCastle":
                        _navMeshAgent.destination = _unitController.Target.GetComponent<Collider>()
                            .ClosestPoint(_unitController.transform.position);
                        break;
                }
            }
            InstantlyTurn(_navMeshAgent.destination);
            if (!PathComplete()) return;
            _unitController.SetState(_unitController.Target != null
                ? UnitController.UnitStates.Attack
                : UnitController.UnitStates.Idle);
        }
        
        private void InstantlyTurn(Vector3 destination) 
        {
            if ((destination - transform.position).magnitude < 0.1f) return; 
             
            var direction = (destination - transform.position).normalized;
            var  qDir= Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * 10);
        }
        
        private bool PathComplete()
        {
            return !_navMeshAgent.pathPending &&
                   _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                   (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
        }

        private void OnDestroy() => _animator.SetBool("isWalking", false);
    }
}