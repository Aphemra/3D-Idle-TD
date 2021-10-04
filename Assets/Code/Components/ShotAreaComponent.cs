using System;
using UnityEngine;

namespace Code.Components
{
    public class ShotAreaComponent : CoreComponent
    {
        private void OnTriggerEnter(Collider other)
        {
            print("On Trigger Enter: " + other.transform.parent.name);

            if (other.transform.parent.TryGetComponent(out EnemyComponent enemyComponent))
            {
                Game.Events.OnEnemyEnteringBattlefield.Invoke(enemyComponent);
            }
        }
    }
}
