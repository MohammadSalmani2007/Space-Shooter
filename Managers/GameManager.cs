using AP_Final_Project.Characters;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace AP_Final_Project.Managers
{
    public class GameManager//All the logic is here :)
    {
        public Player MainPlayer { get; private set; }
        public List<Enemy> ActiveEnemies { get; private set; }
        public List<Bullet> ActiveBullets { get; private set; }
        public WaveManager WaveManager { get; private set; }

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
        private void HandlePlayerShooting()
        {
            if (shotCooldownCounter > 0) shotCooldownCounter--;

            if(MainPlayer.IsShooting && shotCooldownCounter == 0)
            {
                int bulletX = MainPlayer.X + (MainPlayer.Width / 2) - 3;
                int bullerY = MainPlayer.Y;

                ActiveBullets.Add(new PlayerBullet(bulletX, bullerY));
                shotCooldownCounter = MainPlayer.FireRate / 20;
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
                                ActiveEnemies.Remove(enemy);
                                MainPlayer.Score += enemy.ScoreValue;
                            }
                            break;
                        }
                    }
                }
                if(bullet is EnemyBullet)
                {
                    if (bullet.GetBounds().IntersectsWith(MainPlayer.GetBounds()))
                    {
                        MainPlayer.HP--;
                        ActiveBullets.Remove(bullet);
                    }

                }
            }

            foreach (var enemy in ActiveEnemies.ToList())
            {
                if (enemy.GetBounds().IntersectsWith(MainPlayer.GetBounds()))
                {
                    MainPlayer.HP--;
                    ActiveEnemies.Remove(enemy);
                }
            }
        }
        private void CleanUpOutOfBounds()
        {
            ActiveBullets.RemoveAll(b => b.Y + b.Height < 0 || b.Y > gameHeight);
            ActiveEnemies.RemoveAll(e => e.Y > gameHeight);
        }
        public void Draw(Graphics g)//Draw must be out of Game Form !!
        {
            MainPlayer.Draw(g);//Must be implement in Player.cs/and others...
            foreach (var bullet in ActiveBullets) bullet.Draw(g);
            foreach (var enemy in ActiveEnemies) enemy.Draw(g);
            Font hudFont = new Font("Arial", 14, FontStyle.Bold);
            g.DrawString($"Score: {MainPlayer.Score}", hudFont, Brushes.White, 20, 20);
            g.DrawString($"HP: {MainPlayer.HP}", hudFont, Brushes.Crimson, gameWidth - 100, 20);

            if(WaveManager.CurrentWave <= WaveManager.MaxWaves && !WaveManager.IsInWaveTransition)
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

                SizeF mainSize = g.MeasureString(mainText, gameOverFont);
                SizeF subSize = g.MeasureString(subText, scoreFont);
                g.DrawString(mainText, gameOverFont, Brushes.Red, (gameWidth / 2) - (mainSize.Width / 2), (gameHeight / 2) - 50);
                g.DrawString(subText, scoreFont, Brushes.White, (gameWidth / 2) - (subSize.Width / 2), (gameHeight / 2) + 20);

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
