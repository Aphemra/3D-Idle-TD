using System;
using UnityEngine;

namespace Code.Components
{
    public class ShotAreaComponent : CoreComponent
    {
        private void OnTriggerEnter(Collider other)
        {
            print("On Trigger Enter: " + other.name);
            
            if (other.CompareTag("Enemy"))
                Game.Events.OnEnemyEnteringBattlefield.Invoke(other.gameObject);
        }
    }
}
