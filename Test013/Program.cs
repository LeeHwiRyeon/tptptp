using SimpleProfiler;
using System;
using System.Reflection;

namespace Test013 {
    class Program {
        static void Main(string[] args)
        {
            var temp = Profiler.Create("Test");
            temp.UseAssemblies(typeof(ABC).Assembly);
            var p = temp.Build();
            Console.WriteLine("Hello World!");
            var abc = new ABC();
            abc.For100();
            abc.For1000();
            abc.For10000();
            abc.For100000();
            abc.For1000000();
            abc.For10000000();
            abc.For1000000();
            p.Print("UpdateFrameTooSlow.md");
            Console.ReadLine();
        }
    }


    public class ABC {
        [SimpleProfile]
        public void For100()
        {
            Console.WriteLine("For100!");
            for (int i = 0; i < 100; i++) ;
        }

        [SimpleProfile]
        public void For1000()
        {
            Console.WriteLine("For1000!");
            for (int i = 0; i < 1000; i++) ;
        }

        [SimpleProfile]
        public void For10000()
        {
            Console.WriteLine("For10000!");
            for (int i = 0; i < 10000; i++) ;
        }

        [SimpleProfile]
        public void For100000()
        {
            Console.WriteLine("For100000!");
            for (int i = 0; i < 100000; i++) ;
        }

        [SimpleProfile]
        public void For1000000()
        {
            Console.WriteLine("For1000000!");
            for (int i = 0; i < 1000000; i++) ;
        }

        [SimpleProfile]
        public void For10000000()
        {
            Console.WriteLine("For10000000!");
            for (int i = 0; i < 10000000; i++) ;
        }
    }
}
