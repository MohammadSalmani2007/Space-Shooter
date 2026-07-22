using AP_Final_Project.Characters;
using System;
using System.Collections.Generic;
using System.Formats.Nrbf;
using System.Security.Policy;
using System.Text;
using System.Windows.Markup;

namespace AP_Final_Project.Managers
{
    public class GameManager//All the logic is here :)
    {
        public Player MainPlayer { get; private set; }
        public List<Enemy> ActiveEnemies { get; private set; }
        public List<Bullet> ActiveBullets { get; private set; }
        public WaveManager WaveManager { get; private set; }
        public List<Coin> ActiveCoins { get; private set; }
        public List<PowerUp> ActivePowerUps { get; private set; }


        private int gameWidth;
        private int gameHeight;

        private int shotCooldownCounter = 0;
        private int enemySpawnCounter = 0;
        private Random random = new Random();

        public bool IsGameOver { get; private set; } = false;
        public bool IsGameWon { get; private set; } = false;


        public GameManager(int windowWidth, int windowHeight)
        {
            gameWidth = windowWidth;
            gameHeight = windowHeight;

            MainPlayer = new Player(gameWidth/2 - 25, gameHeight - 80);//Assuming a position for player (middle-down of game window)
            ActiveEnemies = new List<Enemy>();
            ActiveBullets = new List<Bullet>();
            WaveManager = new WaveManager();
            ActiveCoins = new List<Coin>();
            ActivePowerUps = new List<PowerUp>();
        }

        public void Update()
        {
            if (IsGameOver) return;//If game is over , we freeze logic
            //I transferred position updatings into the UpdateEntityPositions method !
            HandleSpawning();
            HandlePlayerShooting();
            UpdateEntityPositions();
            CheckAllCollisions();
            CleanUpOutOfBounds();
            if (!MainPlayer.IsAlive)
            {
                IsGameOver = true;
            }
            
        }
        private void HandleSpawning()
        {
            if (WaveManager.IsInWaveTransition)
            {
                WaveManager.UpdateTransition();
                return;
            }

            if (WaveManager.CurrentWave > WaveManager.MaxWaves) return;

            if (!WaveManager.CanSpawnInCurrentWave())
            {
                if(ActiveEnemies.Count == 0)
                {
                    bool hasNext = WaveManager.StartNextWave();
                    if (!hasNext)
                        IsGameWon = true;
                }
                return;
            }
            
            enemySpawnCounter++;
            if(enemySpawnCounter >= WaveManager.GetSpawnRateForCurrentWave())
            {
                int spawnX = random.Next(50 , gameWidth - 70);//40 is enemy's width, later we should pay attention to the all types of enemies
                int spawnY = -70;
                int currentWave = WaveManager.CurrentWave;
                Enemy? newEnemy = null;

                if(currentWave == 10 && random.Next(100) < 25)
                {
                    newEnemy = new HeavyTankEnemy(spawnX, spawnY, currentWave);
                }
                else
                {
                    int roll = random.Next(100);

                    int shooterChance = 10 + (currentWave * 4);
                    int terroristChance = 5 + (currentWave * 3);

                    if (roll < terroristChance)
                    {
                        newEnemy = new TerroristEnemy(spawnX, spawnY, currentWave);
                    }else if(roll < terroristChance + shooterChance)
                    {
                        newEnemy = new ShooterEnemy(spawnX, spawnY, currentWave);
                    }else if ( roll < 85)
                    {
                        newEnemy = new ScoutEnemy(spawnX, spawnY, currentWave);
                    }
                    else
                    {
                        newEnemy = new StandardEnemy(spawnX, spawnY, currentWave);
                    }
                }

                if (newEnemy != null)
                {
                    ActiveEnemies.Add(newEnemy);
                    WaveManager.RegisterSpawn();
                }

                enemySpawnCounter = 0;
            }
        }
        private void HandlePowerUps(Enemy enemy)
        {
            int powerUpX = enemy.X + (enemy.Width / 2) - 10;
            int powerUpY = enemy.Y + (enemy.Height / 2) - 10;
            int random_pu = random.Next(4);
            PowerUpType type;
            switch (random_pu)
            {
                case 0:
                    type = PowerUpType.TripleShot;
                    break;
                case 1:
                    type = PowerUpType.Shield;
                    break;
                case 2:
                    type = PowerUpType.HealthPack;
                    break;
                case 3:
                    type = PowerUpType.FireRateBoooster;
                    break;
                default:
                    type = PowerUpType.HealthPack;
                    break;
            }
            ActivePowerUps.Add(new PowerUp(powerUpX, powerUpY, type));
        }
        private void HandlePlayerShooting()
        {
            if (shotCooldownCounter > 0) shotCooldownCounter--;

            if(MainPlayer.IsShooting && shotCooldownCounter == 0)
            {
                int bulletX = MainPlayer.X + (MainPlayer.Width / 2) - 3;
                int bulletY = MainPlayer.Y;
                int bulletSpeed = 12;

                if (MainPlayer.IsTripleShotActive)
                {
                    int[] angles = { -120 , -90 , -60};
                    
                    foreach(int angle in angles)
                    {
                        double angleInRadians = angle * (Math.PI / 180.0);
                        double velX = bulletSpeed * Math.Cos(angleInRadians);
                        double velY = bulletSpeed * Math.Sin(angleInRadians);

                        ActiveBullets.Add(new PlayerBullet(bulletX, bulletY, velX, velY, angle));
                    }
                }
                else
                {
                    double velX = 0;
                    double velY = -bulletSpeed;
                    ActiveBullets.Add(new PlayerBullet(bulletX, bulletY, velX, velY, 270));
                }

                //shotCooldownCounter = MainPlayer.FireRate / 20;
                int baseCoolDown = Math.Max(5, MainPlayer.FireRate / 20);
                shotCooldownCounter = MainPlayer.IsFireRateBoosted ? baseCoolDown/2 : baseCoolDown;
            }
        }

        private void UpdateEntityPositions()
        {
            MainPlayer.Update();
            ApplyBoundryChecking();

            foreach (var enemy in ActiveEnemies)
            {
                if (enemy is ShooterEnemy shooter)
                {
                    shooter.UpdateAndShoot(ActiveBullets);
                }
                else if (enemy is TerroristEnemy terrorist)
                {
                    terrorist.UpdateWithTarget(MainPlayer.X, MainPlayer.Y);
                }else if(enemy is HeavyTankEnemy heavyTank)
                {
                    heavyTank.UpdateAndShoot8Way(ActiveBullets);
                }
                else
                    enemy.Update();
            }

            foreach (var bullet in ActiveBullets) bullet.Update();
            foreach (var coin in ActiveCoins) coin.Update();
            foreach( var pu in ActivePowerUps) pu.Update();
        }
        private void CheckAllCollisions()
        {
            foreach(var bullet in ActiveBullets.ToList())//ToList is important because we want to remove some items
            {
                if (bullet is PlayerBullet)
                {
                    foreach (var enemy in ActiveEnemies.ToList())
                    {
                        if (bullet.GetBounds().IntersectsWith(enemy.GetBounds()))
                        {
                            enemy.HP--;
                            ActiveBullets.Remove(bullet);

                            if (enemy.HP <= 0)
                            {
                                MainPlayer.Score += enemy.ScoreValue;
                                int random_spawn = random.Next(100);
                                if(random_spawn < 50)
                                {
                                    int coinX = enemy.X + (enemy.Width / 2) - 10;
                                    int coinY = enemy.Y + (enemy.Height / 2) - 10;
                                    int generateValue = (random.Next(100) < 20) ? 5 : 1;

                                    ActiveCoins.Add(new Coin(coinX, coinY, generateValue));
                                }
                                if(random_spawn >= 50 && random_spawn <= 80)
                                {
                                    HandlePowerUps(enemy);
                                }
                                ActiveEnemies.Remove(enemy);
                            }
                            break;
                        }
                    }
                }
                if(bullet is EnemyBullet)
                {
                    if (bullet.GetBounds().IntersectsWith(MainPlayer.PlayerBounds().rectVert) ||
                        bullet.GetBounds().IntersectsWith(MainPlayer.PlayerBounds().rectHor))
                    {
                        if (!MainPlayer.IsShieldActive)
                        {
                            MainPlayer.HP--;
                        }
                        ActiveBullets.Remove(bullet);
                    }

                }
            }

            foreach (var enemy in ActiveEnemies.ToList())
            {
                if (enemy.GetBounds().IntersectsWith(MainPlayer.PlayerBounds().rectVert) ||
                    enemy.GetBounds().IntersectsWith(MainPlayer.PlayerBounds().rectHor))
                {
                    if (!MainPlayer.IsShieldActive)
                    {
                        MainPlayer.HP--;
                    }
                    ActiveEnemies.Remove(enemy);
                }
            }
            foreach(var coin in ActiveCoins.ToList())
            {
                if (coin.GetBounds().IntersectsWith(MainPlayer.GetBounds()))
                {
                    MainPlayer.Coins += coin.Value;
                    ActiveCoins.Remove(coin);

                }
            }
            foreach (var pu in ActivePowerUps.ToList())
            {
                if (pu.GetBounds().IntersectsWith(MainPlayer.GetBounds()))
                {
                    MainPlayer.ApplyPowerUpEffect(pu.Type);
                    ActivePowerUps.Remove(pu);
                }
            }
        }
        private void CleanUpOutOfBounds()
        {
            ActiveBullets.RemoveAll(b => b.Y + b.Height < 0 || b.Y > gameHeight);
            ActiveEnemies.RemoveAll(e => e.Y > gameHeight);
            ActiveCoins.RemoveAll(c => c.Y > gameHeight);
            ActivePowerUps.RemoveAll(pu => pu.Y > gameHeight);
        }
        public void Draw(Graphics g ,Image Player,Image PlayerBullet, Image EnemyBullet, Image Standard,
                         Image Shooter, Image Terrorist, Image Scout, Image HeavyTank)//Draw must be out of Game Form !!
        {
            MainPlayer.Draw(g, Player);//Must be implement in Player.cs/and others...
            foreach (var bullet in ActiveBullets)
            {
                if (bullet is PlayerBullet playerbullet)
                    bullet.Draw(g, PlayerBullet);
                if (bullet is EnemyBullet enemybullet)
                    bullet.Draw(g, EnemyBullet);
            }
            foreach (var enemy in ActiveEnemies) 
            {
                if(enemy is StandardEnemy standard)
                    enemy.Draw(g , Standard);
                if (enemy is ShooterEnemy shooter)
                    enemy.Draw(g, Shooter);
                if (enemy is TerroristEnemy terrorist)
                    enemy.Draw(g, Terrorist);
                if (enemy is ScoutEnemy scout)
                    enemy.Draw(g, Scout);
                if (enemy is HeavyTankEnemy heavyTank)
                    enemy.Draw(g, HeavyTank);
            }
            foreach (var coin in ActiveCoins) coin.Draw(g , Player);
            foreach(var pu in ActivePowerUps) pu.Draw(g , Player);
            Font hudFont = new Font("Arial", 14, FontStyle.Bold);
            g.DrawString($"Score: {MainPlayer.Score}", hudFont, Brushes.White, 20, 20);
            g.DrawString($"Coins: {MainPlayer.Coins}", hudFont, Brushes.Gold, 20, 50);
            g.DrawString($"HP: {MainPlayer.HP}", hudFont, Brushes.Crimson, gameWidth - 100, 20);

            int puY = 80;
            if (MainPlayer.IsShieldActive)
            {
                g.DrawString($"🛡️ Shield: {MainPlayer.RemainingShieldTime:F1}s", hudFont, Brushes.Cyan, 20, puY);
                puY += 25;
            }
            if (MainPlayer.IsTripleShotActive)
            {
                g.DrawString($"🔱 Triple: {MainPlayer.RemainingTripleShotTime:F1}s", hudFont, Brushes.Orange, 20, puY);
                puY += 25;
            }
            if (MainPlayer.IsFireRateBoosted)
            {
                g.DrawString($"⚡ FireRate: {MainPlayer.RemainingFireRateTime:F1}s", hudFont, Brushes.Magenta, 20, puY);
                puY += 25;
            }

            if (WaveManager.CurrentWave <= WaveManager.MaxWaves && !WaveManager.IsInWaveTransition)
            {
                g.DrawString($"WAVE: {WaveManager.CurrentWave} / {WaveManager.MaxWaves}", hudFont, Brushes.Gold, (gameWidth / 2) - 60, 20);
            }

            if(WaveManager.IsInWaveTransition && !IsGameOver && !IsGameWon!)
            {
                Font transitionFont = new Font("Arial", 42, FontStyle.Bold);
                Font readyFont = new Font("Arial", 18, FontStyle.Regular);

                string waveText = $"WAVE {WaveManager.CurrentWave}";
                string readyText = "GET READY...";

                SizeF waveSize = g.MeasureString(waveText, transitionFont);
                SizeF readySize = g.MeasureString(readyText, readyFont);

                g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0 , 0 , gameWidth, gameHeight);
                g.DrawString(waveText, transitionFont, Brushes.Gold, (gameWidth / 2) - (waveSize.Width / 2), (gameHeight / 2) - 60);
                g.DrawString(readyText, readyFont, Brushes.White, (gameWidth / 2) - (readySize.Width / 2), (gameHeight / 2) + 20);
            }
            if (IsGameOver)
            {
                Font gameOverFont = new Font("Arial", 36, FontStyle.Bold);
                Font scoreFont = new Font("Arial", 16, FontStyle.Regular);
                string mainText = "GAME OVER";
                string subText = $"Final Score: {MainPlayer.Score}";
                string coinText = $"Coins: {MainPlayer.Coins}";

                SizeF mainSize = g.MeasureString(mainText, gameOverFont);
                SizeF subSize = g.MeasureString(subText, scoreFont);
                SizeF coinSize = g.MeasureString(coinText, scoreFont);
                g.DrawString(mainText, gameOverFont, Brushes.Red, (gameWidth / 2) - (mainSize.Width / 2), (gameHeight / 2) - 50);
                g.DrawString(subText, scoreFont, Brushes.White, (gameWidth / 2) - (subSize.Width / 2), (gameHeight / 2) + 20);
                g.DrawString(coinText, scoreFont, Brushes.LightGoldenrodYellow, (gameWidth / 2) - (coinSize.Width / 2), (gameHeight / 2) + 50);

            }
            if (IsGameWon)
            {
                Font winFont = new Font("Arial", 36, FontStyle.Bold);
                string winText = "VICTORY! ALL WAVES CLEARED";
                SizeF winSize = g.MeasureString(winText, winFont);

                g.FillRectangle(new SolidBrush(Color.FromArgb(180, 0, 0, 0)), 0, 0, gameWidth, gameHeight);
                g.DrawString(winText, winFont, Brushes.Lime, (gameWidth / 2) - (winSize.Width / 2), (gameHeight / 2) - 30);
            }
        }

        private void ApplyBoundryChecking()//Locking the player in the game window.
        {
            if (MainPlayer.X < 0) MainPlayer.X = 0;
            if(MainPlayer.X > gameWidth - MainPlayer.Width) MainPlayer.X = gameWidth - MainPlayer.Width;
            if (MainPlayer.Y < 0) MainPlayer.Y = 0;
            if(MainPlayer.Y > gameHeight - MainPlayer.Height) MainPlayer.Y = gameHeight - MainPlayer.Height;
        }
    }
}
