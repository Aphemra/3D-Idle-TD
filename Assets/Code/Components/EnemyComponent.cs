using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Code.Resources;
using Code.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Components
{
    public class EnemyComponent : CoreComponent
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private double movementSpeed;
        [SerializeField] private double health;
        [SerializeField] private double damage;
        [SerializeField] private double armor;
        [SerializeField] private double shotSpeed;
        [SerializeField] private double armorPenetration;

        [SerializeField] private bool hasEnteredBattlefield = false;

        [SerializeField] private Slider healthSlider;
        
        [SerializeField] private Transform lazerTargetOrigin;

        private double maxHealth;

        private void Update()
        {
            healthSlider.value = (float)(health / maxHealth); // Debug
        }

        private void Awake()
        {
            maxHealth = health;
        }

        private void OnEnable()
        {
            Game.Events.OnEnemyEnteringBattlefield += TriggerEnteredBattlefieldBool;
        }

        private void OnDisable()
        {
            Game.Events.OnEnemyEnteringBattlefield -= TriggerEnteredBattlefieldBool;
        }
        
        public void InitializeEnemyStatistics(EnemyResource enemyResource)
        {
            // Eventually multiply these values by location and wave difficulty multiplier
            
            enemyType = enemyResource.enemyType;
            movementSpeed = enemyResource.baseMovementSpeed;
            health = enemyResource.maxHealth;
            damage = enemyResource.baseDamage;
            armor = enemyResource.baseArmor;
            shotSpeed = enemyResource.shotSpeed;
            armorPenetration = enemyResource.baseArmorPenetration;
        }

        private void TriggerEnteredBattlefieldBool(EnemyComponent enemyComponent)
        {
            enemyComponent.hasEnteredBattlefield = true;
        }

        public bool GetEnteredBattlefieldBool()
        {
            return hasEnteredBattlefield;
        }

        public double GetArmor()
        {
            return armor;
        }

        public double GetMovementSpeed()
        {
            return movementSpeed;
        }

        public EnemyType GetEnemyType()
        {
            return enemyType;
        }

        public Transform GetLazerOrigin()
        {
            return lazerTargetOrigin;
        }
        
        public void InflictDamage(double damageToInflict)
        {
            health = (health - damageToInflict) <= 0 ? 0 : health - damageToInflict;
            
            if (health <= 0) KillThis();
        }

        private void KillThis()
        {
            Game.EnemyManager.RemoveSpawnedEnemy(this);
            Destroy(gameObject);
        }
        
        public double GetDamagePerSecondCalculation(double enemyArmor)
        {
            return damage;
        }

        public void SetEnemyType(EnemyType enemyTypeToSet)
        {
            enemyType = enemyTypeToSet;
        }
    }
}