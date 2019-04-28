using System;
using System.Drawing;

namespace GB_CSharp_lvl_2
{
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    abstract class BaseObject 
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        /// <summary>
        /// Метод отрисовывает объект в окне приложения
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Метод обновляет координаты объекта
        /// </summary>
        public abstract void Update();
    }

    class Planet : BaseObject
    {
        protected Image Img;

        public Planet(Point pos, Point dir, Size size, Image img) : base(pos, dir, size)
        {
            Img = img;
        }

        public Planet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        /// <summary>
        /// Метод отрисовывает объект в окне приложения
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Метод обновляет координаты объекта
        /// </summary>
        public override void Update()
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

    class Star : Planet
    {
        public Star(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
        }
    }

    class Bullet : Planet, ICollision
    {
        internal new Point Pos;

        public Rectangle Rect => new Rectangle(Pos, Size);
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(Rect);

        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Pos = pos;
        }

        /// <summary>
        /// Метод отрисовывает объект в окне приложения
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Метод обновляет координаты объекта
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + 10;
        }
    }

    class Asteroid : Planet, ICollision
    {
        internal new Point Pos;
        public int Power { get; set; }

        public Rectangle Rect => new Rectangle(Pos, Size);
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(Rect);


        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Pos = pos;
            Power = 1;
        }

        /// <summary>
        /// Метод отрисовывает объект в окне приложения
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Метод обновляет координаты объекта
        /// </summary>
        public override void Update()
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
}