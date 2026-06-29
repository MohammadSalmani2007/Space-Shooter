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
        public EnemyBullet(int x, int y)
            : base(x, y, width: 6, height: 15, speed: 7)
        {

        }
        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, X, Y, Width, Height);
        }
    }
}
