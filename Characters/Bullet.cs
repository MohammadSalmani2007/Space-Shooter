using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public abstract class Bullet : Entity//We will have child classes for Bullet, later.
    {
        protected Bullet(int x, int y, int width, int height, int speed) 
            : base(x, y, width, height, speed, hP: 1)
        {
        }
    }
}
