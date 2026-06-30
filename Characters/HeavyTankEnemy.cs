using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class HeavyTankEnemy : Enemy
    {
        private int fireCoolDownCounter;

        public HeavyTankEnemy(int x, int y)
            : base(x, y, width: 65 , height: 65, speed: 1, hp: 5, scoreValue: 50)
        {
            fireCoolDownCounter = 80;
        }

        public void UpdateAndShoot8Way(List<Bullet> activeBullets)
        {
            Y += Speed;

            fireCoolDownCounter--;
            if(fireCoolDownCounter <= 0)
            {
                Shoot8Way(activeBullets);
                fireCoolDownCounter = 80;
            }
        }
        private void Shoot8Way(List<Bullet> activeBullets)
        {
            int bulletX = X + (Width / 2) - 4;
            int bulletY = Y + (Height / 2) - 4;
            int bulletSpeed = 5;

            for(int i = 0; i < 8; i++)
            {
                double angleInDegrees = i * 45;
                double angleInRadians = angleInDegrees * (Math.PI / 180.0);

                double velX = bulletSpeed * Math.Cos(angleInRadians);//Value and direction
                double velY = bulletSpeed * Math.Sin(angleInRadians);

                activeBullets.Add(new EnemyBullet(bulletX, bulletY, velX, velY, angleInDegrees));
            }
        }
        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.SlateGray, X, Y, Width, Height);
        }
    }
}
