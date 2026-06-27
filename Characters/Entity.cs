using System;
using System.Collections.Generic;
using System.Text;

namespace AP_Final_Project.Characters
{
    public abstract class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width {  get; set; }
        public int Height { get; set; }
        

        public int Speed {  get; set; }
        public int HP {  get; set; }


        protected Entity(int x, int y, int width, int height, int speed, int hP)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            HP = hP;
        }

        public abstract void Update();

        public virtual void Draw(Graphics g)
        {

        }
    }
}
