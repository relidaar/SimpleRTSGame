using System;
using Stats;
using UnityEngine;

namespace Controllers
{
    public class CastleController : MonoBehaviour
    {
        [SerializeField] private CastleStats stats;
        public CastleStats Stats => stats;

        public event Action CastleDestroyed;

        public float Health { get; private set; }

        private void Start()
        {
            Health = stats.health;
        }

        public void TakeDamage(UnitController enemy, float damage)
        {
            if (Health - damage <= 0)
            {
                Health -= damage;
                CastleDestroyed?.Invoke();
            }
            else Health -= damage;
        }
    }
}