using System;

/* 1. Построить три  класса(базовый и  2  потомка),  описывающих работников  с почасовой  оплатой(один из  потомков)  и фиксированной оплатой(второй потомок) :
        a) Описать в базовом классе абстрактный метод для расчета среднемесячной заработной платы.Для «повременщиков» формула для расчета такова: «среднемесячная заработная плата = 20.8 * 8 * почасовая ставка»;
        для работников  с фиксированной  оплатой: «среднемесячная заработная плата = фиксированная месячная оплата»;
        b) Создать на базе абстрактного класса массив сотрудников и заполнить его;
        c) * Реализовать интерфейсы для возможности сортировки массива, используя Array.Sort();
        d) * Создать класс, содержащий массив сотрудников, и реализовать возможность вывода данных с использованием foreach.

    Александр Кушмилов 
*/

namespace Lvl_2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[6];
            Random rnd = new Random();

            for (int i = 0; i < employees.Length; i++)
            {
                byte r = (byte)rnd.Next(0, 2);
                if (r == 0)
                    employees[i] = new EmployeeTW(rnd.Next(4, 7));
                else
                    employees[i] = new EmployeeFW(rnd.Next(700, 1200));
                employees[i].AvgSalaryCalc();
            }

            Office office = new Office(employees.Length);
            employees.CopyTo(office.OfficeEmployees, 0);
            foreach (var item in office)
            {
                Console.WriteLine(((Employee)item).AvgSalary);
            }

            Console.WriteLine();
            Array.Sort(employees);

            foreach (var item in employees)
            {
                Console.WriteLine(item.AvgSalary);
            }
            Console.ReadKey();
        }
    }
}
