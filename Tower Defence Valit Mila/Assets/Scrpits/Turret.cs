using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)            
                m_RefireTimer -= Time.deltaTime;
            else if (Mode == TurretMode.Auto)
            {
                Fire();
            }
        }

        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship)
            {
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                return;

                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }

            TDProjectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<TDProjectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOffFire;

            {
                AudioSource audio = GetComponent<AudioSource>();
                if (audio == null) return;
                audio.Play();

                ///SFX - audio source
            }
        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;
            m_RefireTimer = 0;
            m_TurretProperties = props;
        }

        public void UseTurretAsset(TurretProperties asset)
        {
            m_TurretProperties = asset;
        }
    }
}
