using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class StatsUIEnemy : MonoBehaviour
    {
        [SerializeField] private Image healthBar;        
        [SerializeField] private Destructible enemy;

        public float maxHealth = 100;
        public float currentHealth = 100;       

        public void Start()
        {
            healthBar = GetComponent<Image>();
            enemy = healthBar.transform.root.GetComponent<Destructible>();
        }
        public void UpdateImage()
        {
            healthBar.fillAmount = (float)enemy.CurrentHitPoints / (float)enemy.HitPoints;
        }
    } 
}
