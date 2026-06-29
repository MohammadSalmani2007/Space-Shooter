using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class ScoutEnemy : Enemy
    {
        private int centerX;
        private int moveRange = 50;
        private double angle = 0;


        public ScoutEnemy(int x, int y)
            : base(x, y, width: 35, height: 35, speed: 5, hp: 1, scoreValue: 20)
        {
            centerX = x;
        }

        public override void Update()
        {
            Y += Speed;
            angle += 0.15;
            X = centerX + (int)(Math.Sin(angle) * moveRange);
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.DeepSkyBlue, X, Y, Width, Height);
        }
    }
}
