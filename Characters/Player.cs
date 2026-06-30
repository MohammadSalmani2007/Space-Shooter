using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class Player : Entity
    {
        public bool IsMovingLeft {  get; set; }
        public bool IsMovingRight { get; set; }
        public bool IsMovingUp {  get; set; }
        public bool IsMovingDown {  get; set; }

        public bool IsShooting { get; set; }

        public int FireRate { get; set; }
        public int Score {  get; set; }
        public bool IsAlive => HP > 0;
        public int Coins {  get; set; } = 0;

        public Player(int x, int y)
            : base(x, y, width: 50, height: 50, speed: 6, hp: 3)
        {
            Score = 0;
            Coins = 0;

            IsMovingLeft = false;
            IsMovingRight = false;
            IsMovingUp = false;
            IsMovingDown = false;

            FireRate = 200;
            IsShooting = false;

        }

        public override void Update()
        {
            if (IsMovingLeft) X -= Speed;
            if (IsMovingRight) X += Speed;
            if (IsMovingUp) Y -= Speed;
            if (IsMovingDown) Y += Speed;

        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.LimeGreen, X, Y, Width, Height);
        }
    }
}
