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


        public ScoutEnemy(int x, int y, int wave)
            : base(x, y, width: 35, height: 35, speed: 4, hp: 1, scoreValue: 20, currentWave: wave)
        {
            centerX = x;
        }

        public override void Update()
        {
            Y += Speed;
            angle += 0.15;
            X = centerX + (int)(Math.Sin(angle) * moveRange);
        }

        public override void Draw(Graphics g, Image image)
        {
            RectangleF rect = new RectangleF(X, Y, Width, Height);
            g.DrawImage(image, rect);
        }
    }
}
