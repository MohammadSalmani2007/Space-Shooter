using System;
using System.Collections.Generic;
using System.Text;

namespace Space_Shooter.Characters
{
    public enum PowerUpType
    {
        TripleShot,
        Shield,
        HealthPack,
        FireRateBoooster
    }
    public class PowerUp : Entity
    {
        public PowerUpType Type { get; private set; }
        public PowerUp(int x, int y, PowerUpType type)
            : base(x, y, width: 25, height: 25, speed: 3, hp: 1)
        {
            Type = type;
        }

        public override void Update()
        {
            Y += Speed;
        }

        public override void Draw(Graphics g, Image image)
        {
            Brush pBrush = Brushes.White;
            if (Type == PowerUpType.TripleShot) pBrush = Brushes.Orange;
            else if (Type == PowerUpType.Shield) pBrush = Brushes.Cyan;
            else if (Type == PowerUpType.HealthPack) pBrush = Brushes.LimeGreen;
            else if (Type == PowerUpType.FireRateBoooster) pBrush = Brushes.Magenta;

            g.FillRectangle(pBrush, X, Y, Width, Height);
            g.DrawRectangle(Pens.White, X, Y, Width, Height);

        }
    }
}
