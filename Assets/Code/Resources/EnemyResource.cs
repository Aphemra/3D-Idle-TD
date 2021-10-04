using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Resources
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Resources/Enemy")]
    public class EnemyResource : SerializedScriptableObject
    {
        public GameObject prefab;
        public EnemyType enemyType;
        public double baseMovementSpeed;
        public double maxHealth;
        public double baseDamage;
        public double baseArmor;
        public double shotSpeed;
        public double baseArmorPenetration;
    }

    public enum EnemyType
    {
        Melee, // Attacks by running into towers
        Range  // Attacks by stopping short of towers and shooting from a distance
    }
}
