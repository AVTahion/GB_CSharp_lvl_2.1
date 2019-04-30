using System;
using System.Windows.Forms;

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
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };
            //form.Width = 800;
            //form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}