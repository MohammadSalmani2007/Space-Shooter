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

            this.ClientSize = new Size(800, 600);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;


            gameManager = new GameManager(this.ClientSize.Width, this.ClientSize.Height);

            //Conecting the event(s) through event handlers
            this.Paint += new PaintEventHandler(GameForm_Paint);

            //Game Loop starts with adding game timer event !
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            gameTimer.Start();
        }
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            gameManager.Update();
        }

        private void GameForm_Paint(object? sender, PaintEventArgs e)
        {
            gameManager.Draw(e.Graphics);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
