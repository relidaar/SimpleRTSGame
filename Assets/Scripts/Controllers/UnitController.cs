using System;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Controllers
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private UnitStats stats;
        public UnitStats Stats => stats;
        
        public float Health { get; private set; }

        public Transform Target { get; private set; }
        public Vector3 Destination { get; private set; }

        public event Action UnitDie;
        
        public enum UnitStates
        {
            Attack,
            Move,
            Idle,
            Die
        }

        private Dictionary<UnitStates, MonoBehaviour> _states;

        private void Start()
        {
            Health = stats.health;
            _states = new Dictionary<UnitStates, MonoBehaviour>
            {
                [UnitStates.Attack] = null,
                [UnitStates.Move] = null,
                [UnitStates.Idle] = null,
                [UnitStates.Die] = null
            };
            SetState(UnitStates.Idle);
        }

        public void SetState(UnitStates newState)
        {
            MonoBehaviour GetController(UnitStates x)
            {
                switch (x)
                {
                    case UnitStates.Attack:
                        return gameObject.AddComponent<AttackController>();
                    case UnitStates.Move:
                        return gameObject.AddComponent<MoveController>();
                    case UnitStates.Die:
                        return gameObject.AddComponent<DieController>();
                    case UnitStates.Idle:
                        return null;
                    default:
                        return null;
                }
            }

            foreach (var state in _states.Values) DestroyImmediate(state);
            _states[newState] = GetController(newState);
        }

        public void Move(Vector3 destination)
        {
            Target = null;
            Destination = destination;
            SetState(UnitStates.Move);
        }
        
        public void Attack(Transform target)
        {
            if (target == null) return;
            Target = target;
            Destination = target.position;
            SetState(UnitStates.Move);
        }
        
        public void TakeDamage(UnitController enemy, float damage)
        {
            if (Health - damage <= 0)
            {
                Health -= damage;
                UnitDie?.Invoke();
                SetState(UnitStates.Die);
            }
            else Health -= damage;
        }
    }
}