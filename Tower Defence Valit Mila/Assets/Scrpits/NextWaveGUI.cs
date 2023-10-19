using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        private EnemyWaveMagager manager;
        private float timeToNextWave;
        void Start()
        {
            manager = FindObjectOfType <EnemyWaveMagager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                timeToNextWave = time;
            };
        }
        public void CallWave()
        {
            manager.ForceNextWave();
        }

        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if (bonus < 0) bonus = 0;
            bonusAmount.text = bonus.ToString();
            timeToNextWave -= Time.deltaTime;            
        }
    }
}
