using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Resources
{
    [CreateAssetMenu(fileName = "New Tower", menuName = "Resources/Tower")]
    public class TowerResource : SerializedScriptableObject
    {
        public GameObject prefab;
        public int towerSize;
        public double baseCost;
        public double maxHealth;
        public double baseDamage;
        public double baseArmor;
        public double shotSpeed;
        public double baseArmorPenetration;

    }
}
