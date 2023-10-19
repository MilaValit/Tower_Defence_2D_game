using UnityEngine;
using System;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        public event Action OnPlayerDead;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;        

        public SpaceShip ActiveShip => m_Ship;
        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;

        private void Start()
        {
            if (m_Ship)
            {
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        private void OnShipDeath()
        {
            m_NumLives--;

                if (m_NumLives > 0)
                {
                    Respawn();
                }
                else
                {
                    LevelSequenceController.Instance.FinishCurrentLevel(false);                    
                }
        }

        private void Respawn()
        {
            if (LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
                //m_CameraController.SetTarget(m_Ship.transform);
                //m_MovementController.SetTargetShip(m_Ship);
            }
        }        

        protected void TakeDamage(int m_damage)
        {
            m_NumLives -= m_damage;
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
            }
        }

        protected void AddLives(int hp)
        {
            m_NumLives += hp;
        }

        #region Score

        public int Score { get; private set; }
        public int TotalScore { get; private set; }
        public int NumKills { get; private set; }
        public int TotalKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
            TotalKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
            TotalScore += num;
        }
        #endregion
    }
}
