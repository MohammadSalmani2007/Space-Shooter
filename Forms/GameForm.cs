using Space_Shooter.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace Space_Shooter.Forms
{
    public partial class GameForm : Form
    {
        private GameManager gameManager;
        private System.Windows.Forms.Timer gameTimer;

        private Image Player;
        private Image PlayerBullet;
        private Image EnemyBullet;
        private Image Standard;
        private Image Shooter;
        private Image Terrorist;
        private Image Scout;
        private Image HeavyTank;
        public GameForm()
        {
            InitializeComponent();

            //fixing the Flickering problem
            this.DoubleBuffered = true;
            this.ClientSize = new Size(900, 700);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;


            gameManager = new GameManager(this.ClientSize.Width, this.ClientSize.Height);

            //Conecting the event(s) through event handlers
            this.Paint += new PaintEventHandler(GameForm_Paint);
            this.KeyDown += new KeyEventHandler(GameForm_KeyDown);
            this.KeyUp += new KeyEventHandler(GameForm_KeyUp);

            //Game Loop starts with adding game timer event !
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            gameTimer.Start();
        }
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            gameManager.Update();
            this.Invalidate();//Redraw
        }

        private void GameForm_Paint(object? sender, PaintEventArgs e)
        {
            gameManager.Draw(e.Graphics, Player, PlayerBullet , EnemyBullet, Standard, Shooter, Terrorist, Scout, HeavyTank);
        }

        private void GameForm_KeyDown(object? sender, KeyEventArgs e)
        {
            var player = gameManager.MainPlayer;
            //both arrows and WASD keys
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) player.IsMovingLeft = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) player.IsMovingRight = true;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) player.IsMovingUp = true;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) player.IsMovingDown = true;

            if (e.KeyCode == Keys.Space) player.IsShooting = true;
        }

        private void GameForm_KeyUp(object? sender, KeyEventArgs e)
        {
            var player = gameManager.MainPlayer;
            //both arrows and WASD keys
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) player.IsMovingLeft = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) player.IsMovingRight = false;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) player.IsMovingUp = false;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) player.IsMovingDown = false;

            if (e.KeyCode == Keys.Space) player.IsShooting = false;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            Player = Image.FromFile(@"D:\SpaceShooterGame\Resources\player.PNG");
            PlayerBullet = Image.FromFile(@"D:\SpaceShooterGame\Resources\playerbullet.PNG");
            EnemyBullet = Image.FromFile(@"D:\SpaceShooterGame\Resources\enemybullet.PNG");
            Standard = Image.FromFile(@"D:\SpaceShooterGame\Resources\standard.PNG");
            Shooter = Image.FromFile(@"D:\SpaceShooterGame\Resources\shooter.PNG");
            Terrorist = Image.FromFile(@"D:\SpaceShooterGame\Resources\terrorist.PNG");
            Scout = Image.FromFile(@"D:\SpaceShooterGame\Resources\scout.PNG");
            HeavyTank = Image.FromFile(@"D:\SpaceShooterGame\Resources\heavytank.PNG");
        }
    }
}
