using System;
using TMPro;
using UnityEngine;

namespace Test.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] TMP_Text timeWatch;
        [SerializeField] TMP_Text killCounter;
        [SerializeField] TMP_Text enemyCounter;

        internal void UpdateKillCounter(int killCount)
        {
            killCounter.text = $"{killCount}";
        }

        internal void UpdateRemainingEnemiesCounter(int remainingEnemies)
        {
            enemyCounter.text = $"{remainingEnemies}";
        }

        internal void UpdateTimeWatch(TimeSpan timeSpan)
        {
            timeWatch.text = $"{timeSpan.Minutes}:{timeSpan.Seconds.ToString("00")}";
        }
    }
}