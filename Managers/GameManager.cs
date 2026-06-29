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

        private int gameWidth;
        private int gameHeight;

        private int shotCooldownCounter = 0;
        private int enemySpawnCounter = 0;
        private Random random = new Random();


        public GameManager(int windowWidth, int windowHeight)
        {
            gameWidth = windowWidth;
            gameHeight = windowHeight;

            MainPlayer = new Player(gameWidth/2 - 25, gameHeight - 80);//Assuming a position for player (middle-down of game window)
            ActiveEnemies = new List<Enemy>();
            ActiveBullets = new List<Bullet>();
        }

        public void Update()
        {
            //I transferred position updatings into the UpdateEntityPositions method !
            HandleSpawning();
            HandlePlayerShooting();
            UpdateEntityPositions();
            CheckAllCollisions();
            CleanUpOutOfBounds();
            
        }
        private void HandleSpawning()
        {
            enemySpawnCounter++;
            if(enemySpawnCounter >= 40)//Means: every 40 * 20 msec = 0.8 sec
            {
                int spawnX = random.Next(0 , gameWidth - 70);//40 is enemy's width, later we should pay attention to the all types of enemies
                int spawnY = -70;

                int enemyType = random.Next(4);//We havn't implement HeavyTankEnemy yet ! So it's 4

                switch (enemyType)
                {
                    case 0:
                        ActiveEnemies.Add(new StandardEnemy(spawnX, spawnY));
                        break;
                    case 1:
                        ActiveEnemies.Add(new ScoutEnemy(spawnX, spawnY));
                        break;
                    case 2:
                        ActiveEnemies.Add(new ShooterEnemy(spawnX, spawnY));
                        break;
                    case 3:
                        ActiveEnemies.Add(new TerroristEnemy(spawnX, spawnY));
                        break;

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
                foreach(var enemy in ActiveEnemies.ToList())
                {
                    if (bullet.GetBounds().IntersectsWith(enemy.GetBounds()))
                    {
                        enemy.HP--;
                        ActiveBullets.Remove(bullet);

                        if(enemy.HP <= 0)
                        {
                            ActiveEnemies.Remove(enemy);
                        }
                        break;
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
