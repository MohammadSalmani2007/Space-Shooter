using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Managers
{
    public class WaveManager
    {
        public int CurrentWave { get; private set; } = 1;
        public const int MaxWaves = 10;

        public int EnemiesSpawnedInCurrentWave { get; private set; } = 0;
        public bool IsInWaveTransition { get; private set; } = false;

        private int transitionTimer = 0;
        private const int TransitionDelayFrames = 90;

        public int GetTotalEnemiesForCurrentWave()
        {
            return 5 + (CurrentWave - 1) * 2; 
        }

        public int GetSpawnRateForCurrentWave()
        {
            int rate = 50 - (CurrentWave - 1) * 4;
            return Math.Max(rate, 12);
        }
        public bool CanSpawnInCurrentWave()
        {
            return EnemiesSpawnedInCurrentWave < GetTotalEnemiesForCurrentWave();
        }
        public void RegisterSpawn()
        {
            EnemiesSpawnedInCurrentWave++;
        }

        public void UpdateTransition()
        {
            if (!IsInWaveTransition) return;

            transitionTimer++;
            if(transitionTimer >= TransitionDelayFrames)
            {
                IsInWaveTransition = false;
                transitionTimer = 0;
                EnemiesSpawnedInCurrentWave = 0;
            }
        }

        public bool StartNextWave()
        {
            if(CurrentWave < MaxWaves)
            {
                CurrentWave++;
                IsInWaveTransition = true;
                transitionTimer = 0;
                return true;
            }
            return false;
        }
    }
}
