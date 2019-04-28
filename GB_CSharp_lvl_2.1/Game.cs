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
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        static Game()
        {
        }

        public static BaseObject[] _objs;
        public static List<Image> _images;

        public static void Load()
        {
            _images = new List<Image>();
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
                int n = rnd.Next(-3, -1);
                _objs[i] = new Star(new Point(rnd.Next(10, Width), rnd.Next(10, Height - 20)), new Point(n, 0), new Size(20, 20), _images[rnd.Next(4, 7)]);
            }
            for (int i = _objs.Length * 3 / 4; i < _objs.Length; i++)
            {
                int x = rnd.Next(-10, -1);
                int n = -x + rnd.Next(10, 35);
                _objs[i] = new BaseObject(new Point(rnd.Next(10, Width), rnd.Next(10, Height - 10)), new Point(x, 0), new Size(n, n), _images[rnd.Next(1, 4)]);
            }
        }

        public static void Init(Form form)
        {
            Timer timer = new Timer { Interval = 80 };
            timer.Start();
            timer.Tick += Timer_Tick;

            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
        }

        public static void Draw()
        {
            //Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawImage(_images[0], 0, 0, Width, Height);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}