using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.MicroKernel.Registration;
using Castle.DynamicProxy;
using Castle.Core;
using Castle.Windsor.Diagnostics;
using Castle.MicroKernel;

namespace Castle.Windsor.Study
{
    class Program
    {
        static void Main(string[] args)
        {
#if true1
            WindsorContainer container = new WindsorContainer(new XmlInterpreter());
#else     
            WindsorContainer container = new WindsorContainer();

            var depend = new Dictionary<string, object>() { 
            {"rate" , 0.23m}, 
            {"holidays", new DateTime[]{new DateTime(2014,06,06,16,09,01),new DateTime(2014,06,06,17,09,01),new DateTime(2014,06,06,18,09,01)}},
            {"aliases", new Dictionary<string,string>(){{"aaa","A1"},{"bbb","A2"},{"ccc","A3"}}}
            };
            container.Register(Component.For<TaxCalculator>().DependsOn(depend));

            container.Register(Component.For<RMSInterceptor>().Named("RMSInterceptor.Service"));

            container.Register(Component.For<IRMS>().ImplementedBy<SimpleRMS>().Named("SimpleRMS.Service").Interceptors("RMSInterceptor.Service")/*.LifeStyle.PerWebRequest*/);
            container.Register(Component.For<IRMS>().ImplementedBy<SecondRMS>().Named("SecondRMS.Service").Interceptors("RMSInterceptor.Service")/*.LifeStyle.PerWebRequest*/);
#endif
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

            IRMS rms = container.Resolve<IRMS>("SecondRMS.Service");
            Console.WriteLine(rms.GetRole("ceshi"));

            //容器跟踪
            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            IAllServicesDiagnostic diagnostic = host.GetDiagnostic<IAllServicesDiagnostic>();

            foreach (var item in diagnostic.Inspect())
            {
                Console.WriteLine(item.Key.ToString());

                foreach (var s in item)
                {
                    Console.WriteLine("\t{0},{1}", s.ToString(), s.CurrentState);
                }
            } ;

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

    public interface IRMS
    {
        string GetRole(string userid);

        bool HasRight(string userid);

        bool HasRole(string userid);
    }

    //[Interceptor("RMSInterceptor.Service")]
    public class SimpleRMS : IRMS
    {
        public string GetRole(string userid)
        {
            Console.WriteLine("SimpleRMS - GetRole : " + userid);

            return "Administrator";
        }

        public bool HasRight(string userid)
        {
            Console.WriteLine("SimpleRMS - HasRight : " + userid);
            return true;
        }

        public bool HasRole(string userid)
        {
            Console.WriteLine("SimpleRMS - HasRole : " + userid);
            return true;
        }
    }

    //[Interceptor("RMSInterceptor.Service")]
    public class SecondRMS : IRMS
    {
        public string GetRole(string userid)
        {
            Console.WriteLine("SecondRMS - GetRole : " + userid);

            return "Administrator";
        }

        public bool HasRight(string userid)
        {
            Console.WriteLine("SecondRMS - HasRight : " + userid);
            return true;
        }

        public bool HasRole(string userid)
        {
            Console.WriteLine("SecondRMS - HasRole : " + userid);
            return true;
        }
    }

    public class RMSInterceptor : StandardInterceptor
    {
        protected override void PreProceed(IInvocation invocation)
        {
            base.PreProceed(invocation);

            Console.WriteLine("method: {0} 调用之前", invocation.Method.Name);
        }

        protected override void PerformProceed(IInvocation invocation)
        {
            base.PerformProceed(invocation);

            invocation.ReturnValue = invocation.ReturnValue + "--RMSInterceptor";

            Console.WriteLine("method: {0} 调用中", invocation.Method.Name);
        }

        protected override void PostProceed(IInvocation invocation)
        {
            base.PostProceed(invocation);

            Console.WriteLine("method: {0} 调用之后", invocation.Method.Name);
        }
    }

}
