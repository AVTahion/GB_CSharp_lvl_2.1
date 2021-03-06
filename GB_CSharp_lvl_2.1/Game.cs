﻿using System;
using System.IO;
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
        private static List<Bullet> _bullets = new List<Bullet>();
        public static BaseObject[] _objs;
        public static List<Image> _images = new List<Image>();
        private static Ship _ship;
        private static FirstAidKit _fak;
        private static Timer _timer = new Timer { Interval = 100 };
        private static ListOfAsteroids asteroidsArr;

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
            _images.Add(Image.FromFile(@"..\..\res\asteroid.png"));
            _images.Add(Image.FromFile(@"..\..\res\nlo.png"));
            _images.Add(Image.FromFile(@"..\..\res\FAK.png"));

            _objs = new BaseObject[30];
            Random rnd = new Random();
            try
            {
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

                asteroidsArr = new ListOfAsteroids(4, Width, Height, _images[7]);
            }
            catch(GameObjectException gex)
            {
                Console.WriteLine("Message:" + gex.Message);
                Console.WriteLine("Source:" + gex.Source);
                Console.WriteLine("TargetSite:" + gex.TargetSite);
                Console.WriteLine("StackTrace:" + gex.StackTrace);
            }

            _ship = new Ship(new Point(10, 400), new Point(0, 10), new Size(100, 40), _images[8]);
            _ship.RegisterLogMsg(new LogMessage(LogToConsole));
            _ship.RegisterLogMsg(new LogMessage(LogToFile));

        }

        /// <summary>
        /// Метод инициализирует графический вывод
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;

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
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Source:" + ex.Source);
                Console.WriteLine("TargetSite:" + ex.TargetSite);
                Console.WriteLine("StackTrace:" + ex.StackTrace);
            }
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Ship.MessageDie += Finish;
            Load();
        }

        /// <summary>
        /// Метод отрисовывает изображение в окне
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.DrawImage(_images[0], 0, 0, Width, Height);
            foreach (BaseObject obj in _objs)
                obj?.Draw();
            foreach (Asteroid obj in asteroidsArr)
                obj?.Draw();
            foreach (Bullet b in _bullets) b.Draw();
            _ship?.Draw();
            _fak?.Draw();
            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 10, 10);
                Buffer.Graphics.DrawString("Score:" + _ship.Score, SystemFonts.DefaultFont, Brushes.White, 100, 10);
            }
            Buffer.Render();
        }

        /// <summary>
        /// Метод обновляет координаты объектов
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (Bullet b in _bullets) b.Update();

            if (asteroidsArr.Length != 0)
            {
                for (var i = asteroidsArr.Length - 1; i >= 0; i--)
                {
                    asteroidsArr[i].Update();
                    if (_ship.Collision(asteroidsArr[i]))
                    {
                        asteroidsArr.AsteroidDestruction(i);
                        var rnd = new Random();
                        _ship?.EnergyLow(rnd.Next(10, 20));
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    else
                    {
                        for (int j = _bullets.Count - 1; j >= 0; j--)
                        {
                            if (_bullets[j].Collision(asteroidsArr[i]))
                            {
                                System.Media.SystemSounds.Hand.Play();
                                asteroidsArr.AsteroidDestruction(i);
                                _bullets.RemoveAt(j);
                                _ship.ScoreChange(10);
                                if (_ship.Energy < 100 && _fak == null)
                                {
                                    var rnd = new Random();
                                    _fak = new FirstAidKit(new Point(Width, rnd.Next(10, Height - 10)), new Point(-10, 0), new Size(50, 50), _images[9]);
                                }
                                break;
                            }
                        }

                    }
                    if (_ship.Energy <= 0) _ship?.Die();
                }
            }
            else
            {
                asteroidsArr.AddAsteroids(asteroidsArr.numbOfAsteroids + 1, Width, Height, _images[7]);
            }
            if (_fak != null)
            {
                if (_ship.Collision(_fak))
                {
                    _ship.Heal(_fak);
                    _fak = null;
                }
            }
            _fak?.Update();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Метод обработки нажатия кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet(new Point(_ship.Rect.X + 100, _ship.Rect.Y + 20), new Point(4, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Метод завершения игры
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.Red, Width / 2 - 200, Height / 2 - 30);
            Buffer.Render();
        }

        /// <summary>
        /// Метод вывода сообщений лога в консоль
        /// </summary>
        /// <param name="msg"></param>
        public static void LogToConsole (string msg)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Метод вывода сообщений лога в файл
        /// </summary>
        /// <param name="msg"></param>
        public static void LogToFile (string msg)
        {
            using (StreamWriter swlog = File.AppendText(@"log.txt"))
            {
                swlog.WriteLine(msg);
            }
        }
    }
}