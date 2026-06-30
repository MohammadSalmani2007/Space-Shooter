using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public abstract class Bullet : Entity//We will have child classes for Bullet, later.
    {
        protected Bullet(int x, int y, int width, int height, int speed) 
            : base(x, y, width, height, speed, hP: 1)
        {
        }
    }

    public class PlayerBullet : Bullet
    {
        public PlayerBullet(int x, int y)
            : base(x, y, width: 6, height: 15, speed: 10)
        {
        }

        public override void Update()
        {
            Y -= Speed;//Player bullets move upward
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, X, Y, Width, Height);
        }
    }

    public class EnemyBullet : Bullet
    {
        private double velocityX;
        private double velocityY;

        public double AngleInDegrees {  get; private set; }
        public EnemyBullet(int x, int y, double velX, double velY, double angleInDegrees)
            : base(x, y, width: 8, height: 8, speed: 0)
        {
            velocityX = velX;
            velocityY = velY;
            AngleInDegrees = angleInDegrees;
        }
        public override void Update()
        {
            X += (int)velocityX;
            Y += (int)velocityY;
        }

        public override void Draw(Graphics g)
        {
            var state = g.Save();//We'll change origin coordinates to the bullet's position

            g.TranslateTransform(X + Width / 2, Y + Height / 2);
            g.RotateTransform((float)AngleInDegrees + 90);//We assume that the image of bullet will be upward
            g.FillRectangle(Brushes.Red, -2, -6, 4, 12);

            g.Restore(state);
        }
    }
}
