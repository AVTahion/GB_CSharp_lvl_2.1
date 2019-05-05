using System;
using System.Drawing;

/*  Lesson 1
    1. Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон, похожий на полет в звездном пространстве.
    2. * Заменить кружочки картинками, используя метод DrawImage.
    Lesson 2
    2. Переделать виртуальный метод Update в BaseObject в абстрактный и реализовать его в наследниках.
    3. Сделать так, чтобы при столкновении пули с астероидом они регенерировались в разных концах экрана.
    4. Сделать проверку на задание размера экрана в классе Game. Если высота или ширина (Width, Height) больше 1000 или принимает отрицательное значение, выбросить исключение ArgumentOutOfRangeException().
    5. * Создать собственное исключение GameObjectException, которое появляется при попытке  создать объект с неправильными характеристиками (например, отрицательные размеры,
    слишком большая скорость или неверная позиция).

    Александр Кушмилов
*/

namespace GB_CSharp_lvl_2
{
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }
    interface IRespawn
    {
        void Respawn();
    }

    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        public Rectangle Rect => new Rectangle(Pos, Size);
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(Rect);

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

    class Bullet : BaseObject, IRespawn
    {

        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
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

        public void Respawn()
        {
            Pos.X = 0;
        }
    }

    class Asteroid : Planet, IRespawn
    {
        public int Power { get; set; }

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
            Img = Image.FromFile(@"..\..\res\asteroid.png");
        }

        public void Respawn()
        {
            Random rnd = new Random();
            Pos.X = Game.Width + Size.Width;
            Pos.Y = rnd.Next(10, Game.Height - 10);
        }

    }
}
