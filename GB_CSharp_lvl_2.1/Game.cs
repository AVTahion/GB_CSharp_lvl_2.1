using System;
using System.Windows.Forms;
using System.Drawing;

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

        public static void Load()
        {
            _objs = new BaseObject[30];
            Random rnd = new Random();
            for (int i = 0; i < _objs.Length / 2; i++)
            {
                int x = rnd.Next(-30, 0);
                int n = -x + rnd.Next(5, 15);
                _objs[i] = new BaseObject(new Point(rnd.Next(10, Width), rnd.Next(10, Width - 10)), new Point(x, 0), new Size(n, n));
            }
            for (int i = _objs.Length / 2; i < _objs.Length; i++)
                _objs[i] = new Star(new Point(rnd.Next(10, Width), rnd.Next(10, Width - 10)), new Point(-3, 0), new Size(5, 5));
        }

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
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
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