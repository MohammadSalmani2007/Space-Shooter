using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class Player : Entity
    {
        public bool IsMovingLeft {  get; set; }
        public bool IsMovingRight { get; set; }
        public bool IsMovingUp {  get; set; }
        public bool IsMovingDown {  get; set; }


        public int Score {  get; set; }
        public int Coins {  get; set; }

        public Player(int x, int y)
            : base(x, y, width: 50, height: 50, speed: 6, hP: 3)
        {
            Score = 0;
            Coins = 0;


            IsMovingLeft = false;
            IsMovingRight = false;
            IsMovingUp = false;
            IsMovingDown = false;
        }

        public override void Update()
        {
            if (IsMovingLeft) X -= Speed;
            if (IsMovingRight) X += Speed;
            if (IsMovingUp) Y -= Speed;
            if (IsMovingDown) Y += Speed;

            if (X < 0) X = 0;
            if (X > 800 - Width) X = 800 - Width;
            if (Y < 300) Y = 300;
            if (Y > 600 - Height) Y = 600 - Height;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.LimeGreen, X, Y, Width, Height);
        }
    }
}
