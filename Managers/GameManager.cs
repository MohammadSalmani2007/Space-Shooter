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
            HandlePlayerShooting();
            UpdateEntityPositions();
            CleanUpOutOfBounds();
            
        }
        public void HandlePlayerShooting()
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

        public void UpdateEntityPositions()
        {
            MainPlayer.Update();
            ApplyBoundryChecking();

            foreach (var bullet in ActiveBullets) bullet.Update();
        }
        private void CleanUpOutOfBounds()
        {
            ActiveBullets.RemoveAll(b => b.Y + b.Height < 0 || b.Y > gameHeight);
        }
        public void Draw(Graphics g)//Draw must be out of Game Form !!
        {
            MainPlayer.Draw(g);//Must be implement in Player.cs/and others...
            foreach (var bullet in ActiveBullets) bullet.Draw(g);
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
