using System;
using UnityEngine;

namespace Code.Components
{
    public class TowerAIComponent : MonoBehaviour
    {
        [SerializeField] private TowerComponent thisTowerComponent;
        
        [SerializeField] private EnemyComponent currentTarget;

        [SerializeField] private LineRenderer lineRenderer;

        // 1. Nearest Enemy First Targeting
        //      a. Has enemy entered battlefield before?
        //      b. Is enemy closest enemy among all enemies in battlefield?
        //      c. Set enemy to current target until dead or no longer closest among all enemies

        private void Update()
        {
            NearestEnemyFirstTargeting();
        }

        private void DrawLineToCurrentTarget()
        {
            if (currentTarget == null)
            {
                lineRenderer.SetPosition(0, thisTowerComponent.GetLazerOrigin().position);
                lineRenderer.SetPosition(1, thisTowerComponent.GetLazerOrigin().position);
                return;
            }
            
            lineRenderer.SetPosition(0, thisTowerComponent.GetLazerOrigin().position);
            lineRenderer.SetPosition(1, currentTarget.GetLazerTargetOrigin().position);
        }
        
        private void NearestEnemyFirstTargeting()
        {
            currentTarget = GetNearestEnemy();

            DrawLineToCurrentTarget();
            
            if (currentTarget == null) return;

            AttackCurrentTarget();
        }

        private EnemyComponent GetNearestEnemy()
        {
            EnemyComponent tempTarget = currentTarget;
            
            foreach (var enemyComponent in Game.EnemyManager.GetSpawnedEnemiesList())
            {
                if (IsEnemyCloserThanCurrentTarget(enemyComponent) && enemyComponent.GetEnteredBattlefieldBool()) tempTarget = enemyComponent;
            }

            return tempTarget;
        }

        private bool IsEnemyCloserThanCurrentTarget(EnemyComponent enemy)
        {
            var thisPosition = gameObject.transform.position;
            
            if (currentTarget == null) return true;

            
            if (Vector3.Distance(thisPosition, currentTarget.gameObject.transform.position) >
                Vector3.Distance(thisPosition, enemy.gameObject.transform.position))
            {
                return true;
            }

            return false;
        }

        private void AttackCurrentTarget()
        {
            currentTarget.InflictDamage(thisTowerComponent.GetDamagePerSecondCalculation(currentTarget.GetArmor()));
        }
    }
}
