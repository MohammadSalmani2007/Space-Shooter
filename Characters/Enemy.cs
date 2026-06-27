using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public abstract class Enemy: Entity
    {
        public int ScoreValue {  get; set; }

        protected Enemy(int x, int y, int width, int height, int speed, int hp, int scoreValue)
            : base(x, y, width, height, speed, hp)
        {
            ScoreValue = scoreValue;
        }
    }
}
