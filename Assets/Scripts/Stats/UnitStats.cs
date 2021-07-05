using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "AI/UnitStats")]
    public class UnitStats : ScriptableObject
    {
        public float health;
        public float attackDamage;
        public float attackRange;
        public float attackTimeout;
        public float speed;
        public float stoppingDistance;
    }
}
