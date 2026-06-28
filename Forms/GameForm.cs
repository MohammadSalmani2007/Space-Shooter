using AP_Final_Project.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace AP_Final_Project.Forms
{
    public partial class GameForm : Form
    {
        private GameManager gameManager;
        private System.Windows.Forms.Timer gameTimer;
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
            gameManager.Draw(e.Graphics);
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

        }
    }
}
