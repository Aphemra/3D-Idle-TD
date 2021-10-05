using System;
using System.Collections.Generic;
using Code.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Components
{
    public class EnemyAIComponent : MonoBehaviour
    {
        [SerializeField] private EnemyComponent thisEnemyComponent;
        [SerializeField] private TowerComponent currentTarget;
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private float enemyMeleeRange;
        [SerializeField] private float enemyShotRange;
        
        [SerializeField] private Vector3 directionToTranslate;
        
        // 1. Move towards battlefield
        // 2. When battlefield entered, check enemy type
        //      a. If melee, move towards random tower in range and attack
        //      b. If ranged, stop and shoot random tower in range and attack
        // 3. If target is destroyed, find new random tower in range and attack
        
        private void Update()
        {
            EnemyTargeting(thisEnemyComponent.GetEnemyType());
            
            if (Game.TowerManager.GetActiveTowers().Count == 0) ClearLines(); // Clears enemy laser lines when no towers remaining
        }

        private void EnemyTargeting(EnemyType enemyType)
        {
            currentTarget = GetNearestTower();
            
            MoveTowardCurrentTarget(enemyType);

            if (currentTarget.GetDestructionStatus()) return;

            AttackCurrentTarget();
        }
        
        private void DrawLineToCurrentTarget()
        {
            if (currentTarget.GetDestructionStatus())
            {
                ClearLines();
                return;
            }
            
            lineRenderer.SetPosition(0, thisEnemyComponent.GetLazerOrigin().position);
            lineRenderer.SetPosition(1, currentTarget.GetLazerOrigin().position);
        }

        private void ClearLines()
        {
            lineRenderer.SetPosition(0, thisEnemyComponent.GetLazerOrigin().position);
            lineRenderer.SetPosition(1, thisEnemyComponent.GetLazerOrigin().position);
        }

        private void MoveTowardCurrentTarget(EnemyType enemyType)
        {
            if (!IsEnemyInRangeOfTarget(enemyType))
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, (float)thisEnemyComponent.GetMovementSpeed() * Time.deltaTime);
        }

        private void AttackCurrentTarget()
        {
            var tempTarget = currentTarget;
            
            if (IsEnemyInRangeOfTarget(thisEnemyComponent.GetEnemyType()))
            {
                if (thisEnemyComponent.GetEnemyType() == EnemyType.Range)
                    DrawLineToCurrentTarget();
                
                currentTarget.InflictDamage(thisEnemyComponent.GetDamagePerSecondCalculation(currentTarget.GetArmor()));
                if (tempTarget.GetDestructionStatus()) ClearLines();
            }
        }

        private bool IsEnemyInRangeOfTarget(EnemyType enemyType)
        {
            var isInRange = false;
            
            switch (enemyType)
            {
                case EnemyType.Melee:
                {
                    if (Vector3.Distance(transform.position, currentTarget.transform.position) <= enemyMeleeRange)
                        isInRange = true;
                    break;
                }
                case EnemyType.Range:
                {
                    if (Vector3.Distance(transform.position, currentTarget.transform.position) <= enemyShotRange)
                        isInRange = true;
                    break;
                }
            }

            return isInRange;
        }

        private TowerComponent GetNearestTower()
        {
            TowerComponent tempTarget = currentTarget;
            
            foreach (var towerComponent in Game.TowerManager.GetActiveTowers())
            {
                if (IsTowerCloserThanCurrentTarget(towerComponent))
                    tempTarget = towerComponent;
            }

            return tempTarget;
        }
        
        private bool IsTowerCloserThanCurrentTarget(TowerComponent tower)
        {
            var thisPosition = gameObject.transform.position;
            
            if (currentTarget == null || currentTarget.GetDestructionStatus()) return true;

            if (Vector3.Distance(thisPosition, currentTarget.gameObject.transform.position) >
                Vector3.Distance(thisPosition, tower.gameObject.transform.position))
            {
                return true;
            }

            return false;
        }
    }
}
