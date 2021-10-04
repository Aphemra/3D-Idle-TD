using System;
using UnityEngine;

namespace Code.Components
{
    public class EnemyAIComponent : MonoBehaviour
    {
        [SerializeField] private TowerComponent currentTarget;

        [SerializeField] private Vector3 directionToTranslate;
        
        // 1. Move towards battlefield
        // 2. When battlefield entered, check enemy type
        //      a. If melee, move towards random tower in range and attack
        //      b. If ranged, stop and shoot random tower in range and attack
        // 3. If target is destroyed, find new random tower in range and attack

        private void Update()
        {
            if (!GetComponent<EnemyComponent>().GetEnteredBattlefieldBool() && Game.EnemyManager.spawnEnemies)
                MoveToBattlefield();
        }

        private void MoveToBattlefield()
        {
            transform.Translate(directionToTranslate * Time.deltaTime);
        }
    }
}
