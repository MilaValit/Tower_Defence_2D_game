using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Life, Mana }
        public UpdateSource source = UpdateSource.Gold;
        private Text m_text;
        private void Start()
        {
            m_text = GetComponent<Text>();
            switch (source) 
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                     TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Mana:
                    TDPlayer.Instance.ManaUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int money)
        {
            m_text.text = money.ToString();
        }

        private void OnDestroy()
        {
            TDPlayer.Instance.GoldUpdateUnsubscribe(UpdateText);
            TDPlayer.Instance.LifeUpdateUnsubscribe(UpdateText);
            TDPlayer.Instance.ManaUpdateUnsubscribe(UpdateText);
        }
    }
}
