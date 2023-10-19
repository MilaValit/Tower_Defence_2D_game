using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerDefence
{
    public enum Sound
    {        
        Arrow = 0,
        ArrowHit = 1,
        Magic = 2,
        MagicHit = 3,
        Firegun = 4,
        EnemyDie = 5,
        EnemyWin = 6,
        PlayerWin = 7,
        PlayerLose = 8,
        BGM = 9,
    }

    public static class SoundExtentions
    {
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.Play(sound);
        }
    }
}
