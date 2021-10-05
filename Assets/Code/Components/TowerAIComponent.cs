using System;
using UnityEngine;

namespace Code.Components
{
    public class TowerAIComponent : MonoBehaviour
    {
        [SerializeField] private TowerComponent thisTowerComponent;
        [SerializeField] private EnemyComponent currentTarget;
        [SerializeField] private LineRenderer lineRenderer;

        private void Update()
        {
            NearestEnemyFirstTargeting();
        }

        private void DrawLineToCurrentTarget()
        {
            if (currentTarget == null)
            {
                ClearLines();
                return;
            }
            
            lineRenderer.SetPosition(0, thisTowerComponent.GetLazerOrigin().position);
            lineRenderer.SetPosition(1, currentTarget.GetLazerOrigin().position);
        }

        private void ClearLines()
        {
            lineRenderer.SetPosition(0, thisTowerComponent.GetLazerOrigin().position);
            lineRenderer.SetPosition(1, thisTowerComponent.GetLazerOrigin().position);
        }
        
        private void NearestEnemyFirstTargeting()
        {
            currentTarget = GetNearestEnemy();

            if (currentTarget == null)
            {
                ClearLines();
                return;
            }
            
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
            DrawLineToCurrentTarget();
            currentTarget.InflictDamage(thisTowerComponent.GetDamagePerSecondCalculation(currentTarget.GetArmor()));
        }
    }
}
