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
    Lesson 3
    1. Добавить космический корабль, как описано в уроке.
    2. Доработать в игру «Астероиды»:
        - ведение журнала в консоль с помощью делегатов;
        - * ведение журнала в файл.
    3. Разработать аптечки, которые добавляют энергию.
    4. Добавить подсчет очков за сбитые астероиды.

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

    public delegate void Message();

    public delegate void LogMessage(string msg);

    class GameObjectException : Exception
    {
        public GameObjectException(string message) : base(message)
        {
        }
    }

    abstract class BaseObject : ICollision, IRespawn
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected LogMessage ListOfLog;

        public Rectangle Rect => new Rectangle(Pos, Size);
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(Rect);

        protected BaseObject(Point pos, Point dir, Size size)
        {
            if (pos.X < 0 || pos.X > Game.Width || pos.Y < 0 || pos.Y > Game.Height)
            {
                throw new GameObjectException("Координаты объекта вне игрового экрана");
            }
            Pos = pos;
            if (dir.X > 20 || dir.Y > 20 || dir.X < -20 || dir.Y < -20)
            {
                throw new GameObjectException("Штраф за привышение скорости объектом!");
            }
            Dir = dir;
            if (size.Height <= 0 || size.Width <= 0)
            {
                throw new GameObjectException("Недопустимые размеры объекта");
            }
            Size = size;
        }

        /// <summary>
        /// Реализация IRespawn
        /// </summary>
        public virtual void Respawn()
        {
            Random rnd = new Random();
            Pos.X = Game.Width + Size.Width;
            Pos.Y = rnd.Next(10, Game.Height - 10);
        }

        /// <summary>
        /// Метод отрисовывает объект в окне приложения
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Метод обновляет координаты объекта
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Метод регистрации на делегат
        /// </summary>
        /// <param name="logMessage"></param>
        public void RegisterLogMsg(LogMessage logMessage)
        {
            ListOfLog += logMessage;
        }
    }

    class Planet : BaseObject
    {
        protected Image Img;

        public Planet(Point pos, Point dir, Size size, Image img) : base(pos, dir, size)
        {
            Img = img;
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
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Respawn();
            }
        }
    }

    class Star : Planet
    {
        public Star(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
        }
    }

    class Bullet : BaseObject
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

        /// <summary>
        /// Реализация IRespawn
        /// </summary>
        public override void Respawn()
        {
            Pos.X = 0;
        }
    }

    class Asteroid : Planet
    {
        public int Power { get; set; }

        public Asteroid(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
            Power = 1;
        }
    }

    class Ship : Planet
    {
        private int _energy = 100;
        public int Energy
        {
            set
            {
                _energy = value;
                if (_energy > 100) _energy = 100;
            }
            get => _energy;
        }
        internal int Score { get; set; }

        /// <summary>
        /// Метод уменьшения энергии корабля
        /// </summary>
        /// <param name="n"></param>
        public void EnergyLow(int n)
        {
            Energy -= n;
            ListOfLog?.Invoke($"{DateTime.Now}: Ship Energy {Energy + n} => {Energy}");
        }

        public Ship(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
        }

        public override void Update()
        {
        }

        /// <summary>
        /// Метод изменения счета игры
        /// </summary>
        /// <param name="v"></param>
        internal void ScoreChange(int v)
        {
            Score += v;
            ListOfLog?.Invoke($"{DateTime.Now}: Score {Score - v} => {Score}");
        }

        /// <summary>
        /// Метод расчета координат корабля при движении вверх
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        /// <summary>
        /// Метод расчета координат корабля при движении вниз
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }

        /// <summary>
        /// Метод востановления энергии
        /// </summary>
        /// <param name="fak"></param>
        public void Heal(FirstAidKit fak)
        {
            if (Energy < 100)
            {
                Energy += fak.Power;
                ListOfLog?.Invoke($"{DateTime.Now}: Ship Energy {Energy - fak.Power} => {Energy}");
            }
        }

        public static event Message MessageDie;
    }

    class FirstAidKit : Planet
    {
        public int Power { get; set; }

        public FirstAidKit(Point pos, Point dir, Size size, Image img) : base(pos, dir, size, img)
        {
            Power = 10;
        }
    }
}

