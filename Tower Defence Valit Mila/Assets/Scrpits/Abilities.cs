using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using SpaceShooter;

namespace TowerDefence
{
    public class Abilities : SingletonBase<Abilities>
    {
        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private Button m_FireButton;
        public void UseFireAbility() => m_FireAbility.Use();

        [SerializeField] private TimeAbility m_TimeAbility;
        [SerializeField] private Button m_TimeButton;       
        public void UseTimeAbility() => m_TimeAbility.Use();

        private void Start()
        {
            m_FireAbility.CheckUpgrade();
            m_TimeAbility.CheckUpgrade();
            TDPlayer.Instance.ManaUpdateSubscribe(m_TimeAbility.CheckCost);
            TDPlayer.Instance.ManaUpdateSubscribe(m_FireAbility.CheckCost);
        }


        private void OnDestroy()
        {
            TDPlayer.Instance.ManaUpdateUnsubscribe(m_TimeAbility.CheckCost);
            TDPlayer.Instance.ManaUpdateUnsubscribe(m_FireAbility.CheckCost);
        }


        [Serializable]
        public class FireAbility
        {
            [SerializeField] public int m_Cost;
            [SerializeField] private int m_Damage;
            [SerializeField] private float m_Radius;
            [SerializeField] private Color m_TargetingColor;
            [SerializeField] private UpgradeAsset m_Upgrade;
            [SerializeField] private Text m_CostText;
            //[SerializeField] private FollowMouse target;
            private bool NeedUpgrade;
            private bool _cooldown;
            /*
            private void Start()
            {
                target.gameObject.SetActive(false);
            }*/


            public void Use() 
            {
                TDPlayer.Instance.ChangeMana(-m_Cost);                
                ClickProtection.Instance.Activate((Vector2 v) => 
                {
                    //target.gameObject.SetActive(true);
                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);
                    
                    foreach (var collider in Physics2D.OverlapCircleAll(position, m_Radius))
                    {
                        if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
                        }
                    }                
                });
                //target.gameObject.SetActive(false);
                CheckCost(TDPlayer.Instance.Mana);
            }

            public void CheckUpgrade()
            {
                if (Upgrades.GetUpgradeLevel(m_Upgrade) == 0)
                {
                    Instance.m_FireButton.interactable = false;
                    m_CostText.text = "X";
                    NeedUpgrade = true;
                }
                else
                {
                    Instance.m_FireButton.interactable = true;
                    m_Radius += m_Radius * Upgrades.GetUpgradeLevel(m_Upgrade);
                    m_Damage += m_Damage * Upgrades.GetUpgradeLevel(m_Upgrade);
                    NeedUpgrade = false;
                }
                m_CostText.text = m_Cost.ToString();
            }

            public void CheckCost(int mana)
            {
                if (NeedUpgrade) return;                
                print($"Fire - {mana}, {m_Cost}");
                if (mana >= m_Cost)
                {
                    Instance.m_FireButton.interactable = true;
                    m_CostText.color = Color.white;
                }
                else if (mana < m_Cost)
                {
                    Instance.m_FireButton.interactable = false;
                    m_CostText.color = Color.red;
                }
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] public int m_Cost;
            [SerializeField] private float m_Cooldown;
            [SerializeField] private float m_Duration;            
            [SerializeField] private float m_DurationUpgradePerLVL;
            [SerializeField] private UpgradeAsset m_Upgrade;
            [SerializeField] private Text m_CostText;
            private bool NeedUpgrade;
            private bool _cooldown;            

            public void Use()
            {
                TDPlayer.Instance.ChangeMana(-m_Cost);
                _cooldown = true;
                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }

                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.HalfMaxLinearVelocity();

                EnemyWaveMagager.OnEnemySpawn += Slow;               

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                        ship.RestoreMaxLinearVelocity();
                    EnemyWaveMagager.OnEnemySpawn -= Slow;
                }

                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    CheckCost(TDPlayer.Instance.Mana);
                    Instance.m_TimeButton.interactable = false;
                    print(m_Cooldown);
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_TimeButton.interactable = true;
                    _cooldown = false;
                    CheckCost(TDPlayer.Instance.Mana);
                }
                Instance.StartCoroutine(TimeAbilityButton());                
            }

            public void CheckUpgrade()
            {
                if (Upgrades.GetUpgradeLevel(m_Upgrade) == 0)
                {
                    Instance.m_TimeButton.interactable = false;
                    m_CostText.text = "X";
                    NeedUpgrade = true;
                }
                else
                {
                    Instance.m_TimeButton.interactable = true;
                    m_Duration += m_DurationUpgradePerLVL * Upgrades.GetUpgradeLevel(m_Upgrade);
                    NeedUpgrade = false;
                }
                m_CostText.text = m_Cost.ToString();
            }

            public void CheckCost(int mana)
            {
                if (NeedUpgrade) return;
                if (_cooldown) return;
                print($"Time - {mana}, {m_Cost}");
                if (mana >= m_Cost)
                {
                    Instance.m_TimeButton.interactable = true;
                    m_CostText.color = Color.white;
                }
                else
                {
                    Instance.m_TimeButton.interactable = false;
                    m_CostText.color = Color.red;
                }
            }
        }        
    }
}
