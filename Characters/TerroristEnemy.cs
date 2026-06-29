using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class TerroristEnemy : Enemy
    {
        private bool isLockOn = false;
        private int targetX = 0;
        public TerroristEnemy(int x, int y)
            : base(x, y, width: 40, height: 40, speed: 3, hp: 1, scoreValue: 40)
        {
        }

        public void UpdateWithTarget(int playerX, int playerY)//Exclusive update
        {
            if (!isLockOn)
            {
                Y += Speed;

                if(playerY - Y < 300)
                {
                    isLockOn = true;
                    targetX = playerX;
                    Speed = 6;//As soon as it sees the player, it moves towards it at high speed
                }
            }
            else
            {
                Y += Speed;
                if (X < targetX) X += 3;
                if (X > targetX) X -= 3;
            }
        }

        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.OrangeRed, X, Y, Width, Height);
        }
    }
}
