using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class ShooterEnemy : Enemy
    {
        private int fireCoolDownCounter;
        private Random random = new Random();


        public ShooterEnemy(int x, int y, int wave)
            : base(x, y, width: 45, height: 45, speed: 3, hp: 2, scoreValue: 30, currentWave: wave)
        {
            fireCoolDownCounter = random.Next(40 , 90);
        }

        public void UpdateAndShoot(List<Bullet> activeBullets)
        {
            Y += Speed;

            fireCoolDownCounter--;

            if (fireCoolDownCounter <= 0)
            {
                int bulletX = X + (Width / 2) - 3;
                int bulletY = Y + Height;

                activeBullets.Add(new EnemyBullet(bulletX, bulletY, velX: 0, velY: 7, angleInDegrees: 90));// 90 + 90 = 180 , so downward
                fireCoolDownCounter = 80;//We can change it to be random
            }

        }

        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.DarkViolet, X, Y, Width, Height);
        }
    }
}
