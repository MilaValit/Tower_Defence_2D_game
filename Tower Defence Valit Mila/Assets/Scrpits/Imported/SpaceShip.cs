using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперёд сила.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;
                

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity => m_MaxLinearVelocity;

        private float m_MaxVelocityBackUp;

        public void HalfMaxLinearVelocity() 
        {
            m_MaxVelocityBackUp = m_MaxLinearVelocity;
            m_MaxLinearVelocity /= 2; 
        }

        public void RestoreMaxLinearVelocity()
        {
            m_MaxLinearVelocity = m_MaxVelocityBackUp;            
        }

        private float m_MaxLinearVelocityOriginal;

        /// <summary>
        /// Максимаьная вращательная скорость. В градусах/сек.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject visualModel;
        [SerializeField] private GameObject invincibilityEffectPrefab;
        [SerializeField] private GameObject speedUpEffectPrefab;

        /// <summary>
        /// Сохранённая ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigid;

        #region Public API
        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        private Timer m_Timer;

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Timer = new Timer(3);
            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;
            m_MaxLinearVelocityOriginal = m_MaxLinearVelocity;
            //InitOffensive();
        }

        private void Update()
        {
            m_Timer.RemoveTime(Time.deltaTime);

            if (m_Timer.IsFinished == true)
            {
                MakeDestructible();
                m_MaxLinearVelocity = m_MaxLinearVelocityOriginal;
            }
        }

        protected override void OnDeath()
        {
            if (Nickname == "Enemy")
            {
                Player.Instance.AddKill();
            }

            //visualModel.SetActive(false);

            /*var Explosion = Instantiate(explosionPrefab, m_Rigid.transform);
                        
            Destroy(Explosion, timeBeforeRespawn); */           
            base.OnDeath();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            //UpdateEnergyRegen();
        }

        #endregion

        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /// <summary>
        /// TODO: заменить временный метод-заглушку. Используется турелями.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }
        /// <summary>
        /// TODO: заменить временный метод-заглушку. Используется турелями.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }


        //[SerializeField] private Turret[] m_Turrets;

        /// <summary>
        /// TODO: заменить временный метод-заглушку. Используется ИИ.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public void Fire(TurretMode mode)
        {
            return;
        }
        /*
        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

       

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }*/

        public void Invincibility(float TimeBonusActive)
        {
            MakeIndestructible();
            m_Timer.Start(TimeBonusActive);
            var invincEff = Instantiate(invincibilityEffectPrefab, transform.position, Quaternion.identity, transform);

            Destroy(invincEff.gameObject, TimeBonusActive);
        }

        public void AddVelocity(float bonusSpeed, float TimeBonusActive)
        {
            m_MaxLinearVelocity += bonusSpeed;
            m_Timer.Start(TimeBonusActive);

            var speedUpEff = Instantiate(speedUpEffectPrefab, transform.position, transform.rotation, transform);

            Destroy(speedUpEff.gameObject, TimeBonusActive);
        }

        public void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }
    }
}