using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance 
        { get 
            { 
                return Player.Instance as TDPlayer;
            }
        }
        private event Action<int> OnGoldUpdate;
        public event Action<int> OnLifeUpdate;
        private event Action<int> OnManaUpdate;

        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_gold);
        }        
        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }
        public void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act(Instance.m_mana);
            print($"Following your mana!!!");
        }

        public void GoldUpdateUnsubscribe(Action<int> act)
        {
            OnGoldUpdate -= act;
            act(Instance.m_gold);
        }       
        public void LifeUpdateUnsubscribe(Action<int> act)
        {
            OnLifeUpdate -= act;
            act(Instance.NumLives);
        }
        public void ManaUpdateUnsubscribe(Action<int> act)
        {
            OnManaUpdate -= act;
            act(Instance.m_mana);
        }

        [SerializeField] private int m_gold = 0;
        [SerializeField] private int m_mana = 0;
        public int Mana => m_mana;
        [SerializeField] private Tower m_towerPrefab;

        public void ChangeGold(int change)
        {
            m_gold += change;
            OnGoldUpdate(m_gold);
        }
        public void ChangeMana(int change)
        {
            m_mana += change;
            OnManaUpdate(m_mana);
            print($"Update Mana!");
        }

        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }
        //TODO: верим в то, что золота на постройку достаточно
        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(m_towerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);

            /*tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.sprite;
            tower.GetComponentInChildren<Turret>().UseTurretAsset(towerAsset.turretProperties);*/
            Destroy(buildSite.gameObject);
        }

        [SerializeField] private UpgradeAsset healthUpgrade;
        [SerializeField] private UpgradeAsset goldUpgrade;        

        private void Start()
        {            
            var levelHealth = Upgrades.GetUpgradeLevel(healthUpgrade);
            if (levelHealth > 0)
            {
                AddLives(levelHealth * 5);
            }
            
            var levelGold = Upgrades.GetUpgradeLevel(goldUpgrade);
            if (levelGold != 0)
            {
                m_gold += levelGold * 20;
            }
        }
    }
}
