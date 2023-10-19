using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        public bool isCompleted;

        void Start()
        {
            FindObjectOfType<EnemyWaveMagager>().OnAllWavesDead += () =>
            {
                isCompleted = true;                
            };
        }

        public bool IsCompleted { get { return isCompleted; } }
    }
}
