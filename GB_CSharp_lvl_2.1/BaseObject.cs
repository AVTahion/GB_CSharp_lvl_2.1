using System;
using System.Drawing;

namespace GB_CSharp_lvl_2
{
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Image Img;


        public BaseObject(Point pos, Point dir, Size size, Image img)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            Img = img;
        }

        public virtual void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public virtual void Update()
        {
            Random rnd = new Random();
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Pos.X = Game.Width + Size.Width;
                Pos.Y = rnd.Next(10, Game.Height - 10);
            }
        }
    }

    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
        }
    }
}