using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Castle.Windsor.Study
{
    class Program
    {
        static void Main(string[] args)
        {
            WindsorContainer container = new WindsorContainer(new XmlInterpreter());
            TaxCalculator calculator = container.Resolve<TaxCalculator>();
            decimal gross = 100;
            decimal tax = calculator.CalculateTax(gross);
            Console.WriteLine("Gross: {0}, Tax: {1}", gross, tax);

            if (calculator.Holidays != null)
            {
                Console.WriteLine("数组：");
                foreach (var item in calculator.Holidays)
                {
                    Console.WriteLine(item.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            if (calculator.Aliases != null)
            {
                Console.WriteLine("字典：");
                foreach (var item in calculator.Aliases)
                {
                    Console.WriteLine("{0} : {1}", item.Key, item.Value);
                }
            }

            Console.ReadKey();
        }
    }

    [Castle.Core.Singleton] //在这个地方设置 类实例的生命周期
    public class TaxCalculator
    {
        private decimal _rate = 0.125M;
        private DateTime[] _holidays;
        private Dictionary<string, string> _aliases;
        public decimal Rate
        {
            set { _rate = value; }
            get { return _rate; }
        }
        public DateTime[] Holidays
        {
            get { return _holidays; }
            set { _holidays = value; }
        }
        public Dictionary<string, string> Aliases
        {
            get { return _aliases; }
            set { _aliases = value; }
        }
        public decimal CalculateTax(decimal gross)
        {
            return Math.Round(_rate * gross, 2);
        }
    }
}
