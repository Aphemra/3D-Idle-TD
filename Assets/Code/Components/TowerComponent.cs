using System;
using Code.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components
{
    public class TowerComponent : CoreComponent
    {
        [Title("Tower Statistics")]
        [SerializeField] private int groupTowerTier;
        [SerializeField] private double cost;
        [SerializeField] private double health;
        [SerializeField] private double damage;
        [SerializeField] private double armor;
        [SerializeField] private double shotSpeed;
        [SerializeField] private double armorPenetration;

        [SerializeField] private GameObject currentTarget;

        private float shotCounter = 0;
        

        private void OnEnable()
        {
            Game.Events.OnEnemyEnteringBattlefield += GetTarget;
        }

        private void OnDisable()
        {
            Game.Events.OnEnemyEnteringBattlefield -= GetTarget;
        }

        public void InitializeTowerStatistics(TowerResource towerResource)
        {
            groupTowerTier = towerResource.groupTowerTier;
            cost = towerResource.baseCost;
            health = towerResource.maxHealth;
            damage = towerResource.baseDamage;
            armor = towerResource.baseArmor;
            shotSpeed = towerResource.shotSpeed;
            armorPenetration = towerResource.baseArmorPenetration;
        }

        private void Update()
        {
            Attack();
        }

        private void GetTarget(GameObject target)
        {
            if (currentTarget == null)
                currentTarget = target;
            
            if (Vector3.Distance(transform.position, currentTarget.transform.position) <
                Vector3.Distance(transform.position, target.transform.position))
            {
                currentTarget = target;
            }
        }

        private void Attack()
        {
            if (currentTarget == null) return;
            
            if (shotCounter < shotSpeed)
            {
                //print("Not Shooting");
                shotCounter += Time.deltaTime;
                return;
            }

            if (shotCounter >= shotSpeed)
            {
                print("Shooting");
                shotCounter = 0;
                ShootEnemy();
            }
        }

        private void ShootEnemy()
        {
            print(name + " is shooting enemy named " + currentTarget.name);
            // Draw Raycast
            // Call Damage method on enemy script when raycast hits
        }
    }
}
