using System;
using System.Collections;

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
    /// <summary>
    /// Базовый класс сотрудника
    /// </summary>
    abstract class Employee : IComparable
    {
        public double AvgSalary { get; set; }

        /// <summary>
        /// Метод расчитывает среднюю заработную плату
        /// </summary>
        internal abstract void AvgSalaryCalc();

        int IComparable.CompareTo(object obj)
        {
            Employee temp = obj as Employee;
            if (temp != null)
            {
                if (AvgSalary > temp.AvgSalary)
                    return 1;
                if (AvgSalary < temp.AvgSalary)
                    return -1;
                else
                    return 0;
            }
            else
                throw new ArgumentException("Параметр не является объектом типа Employee!");
        }
    }

    /// <summary>
    /// Класс сотрудника с почасовой оплатой
    /// </summary>
    class EmployeeTW : Employee
    {
        protected double HourlyRate { get; set; }

        public EmployeeTW(double _hourlyRate)
        {
            HourlyRate = _hourlyRate;
        }

        /// <summary>
        /// Метод расчитывает среднюю заработную плату сотрудника с почасовой оплатой
        /// </summary>
        internal override void AvgSalaryCalc()
        {
            AvgSalary = 20.8 * 8 * HourlyRate;
        }
    }

    /// <summary>
    /// Класс сотрудника с фиксированной оплатой
    /// </summary>
    class EmployeeFW : Employee
    {
        protected double Salary { get; set; }

        public EmployeeFW(double _salary)
        {
            Salary = _salary;
        }

        /// <summary>
        /// Метод расчитывает среднюю заработную плату сотрудника с фиксированной оплатой
        /// </summary>
        internal override void AvgSalaryCalc()
        {
            AvgSalary = Salary;
        }
    }

    /// <summary>
    /// Класс содержащий массив сотрудников и интерфейсом IEnumerable
    /// </summary>
    class Office : IEnumerable
    {
        internal Employee[] OfficeEmployees;

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < OfficeEmployees.Length; i++)
                yield return OfficeEmployees[i];
        }

        public Office(int n)
        {
            OfficeEmployees = new Employee[n];
        }
    }
}
