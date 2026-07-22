using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Text;

namespace Space_Shooter.Characters
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

        public bool IsShieldActive => shieldTimer > 0;
        public bool IsTripleShotActive => tripleShotTimer > 0;
        public bool IsFireRateBoosted => fireRateTimer > 0;

        private int shieldTimer = 0;
        private int tripleShotTimer = 0;
        private int fireRateTimer = 0;

        public float RemainingShieldTime => IsShieldActive ? shieldTimer / 50f : 0;
        public float RemainingTripleShotTime => IsTripleShotActive ? tripleShotTimer / 50f : 0;
        public float RemainingFireRateTime => IsFireRateBoosted ? fireRateTimer / 50f : 0;

        public Player(int x, int y)
            : base(x, y, width: 80, height: 80, speed: 6, hp: 3)
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
        public void ApplyPowerUpEffect(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.HealthPack:
                    if (HP < 5) HP++;
                    break;
                case PowerUpType.Shield:
                    shieldTimer = 250;
                    break;
                case PowerUpType.TripleShot:
                    tripleShotTimer = 500;
                    break;
                case PowerUpType.FireRateBoooster:
                    fireRateTimer = 500;
                    break;
            }
        }

        public override void Update()
        {
            if (IsMovingLeft) X -= Speed;
            if (IsMovingRight) X += Speed;
            if (IsMovingUp) Y -= Speed;
            if (IsMovingDown) Y += Speed;

            if (shieldTimer > 0) shieldTimer--;
            if (tripleShotTimer > 0) tripleShotTimer--;
            if (fireRateTimer > 0) fireRateTimer--;
        }

        public override void Draw(Graphics g, Image image)
        {
            RectangleF rect = new RectangleF(X, Y, Width, Height);
            g.DrawImage(image ,rect );

            if (IsShieldActive)
                g.DrawEllipse(new Pen(Color.Cyan, 3), X - 5, Y - 5, Width + 10, Height + 10);
        }

        public (Rectangle rectVert, Rectangle rectHor) PlayerBounds()
        {
            Rectangle rectHor =  new Rectangle(X, Y + 50, Width, Height - 50);
            Rectangle rectVer = new Rectangle(X + 30, Y, Width - 60, Height);
            return (rectVer, rectHor);

        }
    }
}
