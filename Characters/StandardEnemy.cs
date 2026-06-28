using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public class StandardEnemy : Enemy
    {
        public StandardEnemy(int x, int y)
            : base (x, y , width: 40, height: 40, speed: 3, hp: 1, scoreValue: 10)//Assuming values to move the project forward 
        {
        }

        public override void Update()
        {
            Y += Speed;//Moving downward
        }

        public override void Draw(Graphics g)
        {

        }
    }
}
