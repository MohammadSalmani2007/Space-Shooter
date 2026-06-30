using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class WaveData
    {
        public int WaveNumber { get; set; }
        public int TotalEnemies {  get; set; }
        public int SpawnRate {  get; set; }
        public bool AllowHeavyTank {  get; set; }

        public WaveData(int waveNumber, int totalEnemies, int spawnRate, bool allowHeavyTank)
        {
            WaveNumber = waveNumber;
            TotalEnemies = totalEnemies;
            SpawnRate = spawnRate;
            AllowHeavyTank = allowHeavyTank;
        }
    }
}
