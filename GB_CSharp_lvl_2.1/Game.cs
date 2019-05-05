using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace GB_CSharp_lvl_2
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }       // Ширина игрового поля
        public static int Height { get; set; }      // Высота игрового поля
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        public static BaseObject[] _objs;
        public static List<Image> _images = new List<Image>();

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        static Game()
        {
        }

        /// <summary>
        /// Метод загружает ресурсы и инициализирует объекты
        /// </summary>
        public static void Load()
        {
            _images.Add(Image.FromFile(@"..\..\res\Background_1.png"));
            _images.Add(Image.FromFile(@"..\..\res\Planet_1.png"));
            _images.Add(Image.FromFile(@"..\..\res\Planet_2.png"));
            _images.Add(Image.FromFile(@"..\..\res\Planet_3.png"));
            _images.Add(Image.FromFile(@"..\..\res\Star_1.png"));
            _images.Add(Image.FromFile(@"..\..\res\Star_2.png"));
            _images.Add(Image.FromFile(@"..\..\res\Star_3.png"));

            _objs = new BaseObject[30];
            Random rnd = new Random();
            for (int i = 0; i < _objs.Length * 3 / 4; i++)
            {
                int n = rnd.Next(-2, -1);
                _objs[i] = new Star(new Point(rnd.Next(10, Width), rnd.Next(10, Height - 20)), new Point(n, 0), new Size(20, 20), _images[rnd.Next(4, 7)]);
            }
            for (int i = _objs.Length * 3 / 4; i < _objs.Length; i++)
            {
                int x = rnd.Next(-5, -1);
                int n = -x + rnd.Next(10, 35);
                _objs[i] = new Planet(new Point(rnd.Next(10, Width), rnd.Next(10, Height - 10)), new Point(x, 0), new Size(n, n), _images[rnd.Next(1, 4)]);
            }

            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[3];

            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(25, 100);
                _asteroids[i] = new Asteroid(new Point(Width, 190), new Point(-r / 5, r), new Size(r, r));
            }
        }

        /// <summary>
        /// Метод инициализирует графический вывод
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;

            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            try
            {
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                if (Width > 1000 || Width <640 || Height < 480 || Height > 1000)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
        }

        /// <summary>
        /// Метод отрисовывает изображение в окне
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.DrawImage(_images[0], 0, 0, Width, Height);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid obj in _asteroids)
                obj.Draw();
            _bullet.Draw();
            Buffer.Render();
        }

        /// <summary>
        /// Метод обновляет координаты объектов
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            foreach (Asteroid a in _asteroids)
            {
                a.Update();
                if (a.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _bullet.Respawn();
                    a.Respawn();
                }
            }
            _bullet.Update();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}