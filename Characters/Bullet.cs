using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public abstract class Bullet : Entity//We will have child classes for Bullet, later.
    {
        protected Bullet(int x, int y, int width, int height, int speed) 
            : base(x, y, width, height, speed, hp: 1)
        {
        }
    }

    public class PlayerBullet : Bullet
    {
        public PlayerBullet(int x, int y)
            : base(x, y, width: 10, height: 20, speed: 10)
        {
        }

        public override void Update()
        {
            Y -= Speed;//Player bullets move upward
        }

        public override void Draw(Graphics g , Image image)
        {
            RectangleF rect = new RectangleF(X, Y, Width, Height);
            g.DrawImage(image, rect);
        }
    }

    public class EnemyBullet : Bullet
    {
        private double velocityX;
        private double velocityY;

        public double AngleInDegrees {  get; private set; }
        public EnemyBullet(int x, int y, double velX, double velY, double angleInDegrees)
            : base(x, y, width: 10, height: 20, speed: 0)
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

        public override void Draw(Graphics g, Image image)
        {
            var state = g.Save();//We'll change origin coordinates to the bullet's position

            g.TranslateTransform(X + Width / 2, Y + Height / 2);
            g.RotateTransform((float)AngleInDegrees + 90);//We assume that the image of bullet will be upward
            RectangleF rect = new RectangleF(-Width/2, -Height/2, Width, Height);
            g.DrawImage(image, rect);
            g.Restore(state);
        }
    }
}
