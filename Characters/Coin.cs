using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class Coin : Entity
    {
        public int Value {  get; private set; }
        public Coin(int x, int y, int value) 
            : base(x, y, width: value == 5 ? 26 : 18, height: value == 5 ? 26 : 18, speed: 2, hp: 1)
        {
            Value = value;
        }

        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g)
        {
            if(Value == 5)
            {
                g.FillEllipse(Brushes.Gold, X, Y, Width, Height);
                g.DrawEllipse(Pens.DarkGoldenrod, X + 2, Y + 2, Width - 4, Height - 4);

            }
            else
            {
                g.FillEllipse(Brushes.Silver, X, Y, Width, Height);
                g.DrawEllipse(Pens.Gray, X + 1, Y + 1, Width - 2, Height - 2);
            }
        }
    }
}
