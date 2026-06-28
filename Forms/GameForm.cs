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
        public GameForm()
        {
            InitializeComponent();

            this.ClientSize = new Size(800, 600);
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;


            gameManager = new GameManager(this.ClientSize.Width, this.ClientSize.Height);
            
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
