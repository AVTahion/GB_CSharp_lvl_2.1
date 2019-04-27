using System;
using System.Windows.Forms;
// Создаем шаблон приложения, где подключаем модули
namespace GB_CSharp_lvl_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}