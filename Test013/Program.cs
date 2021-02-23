using SimpleProfiler;
using System;
using System.Reflection;

namespace Test013 {
    class Program {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var temp = Profiler.Create("Test");
            temp.UseAssemblies(typeof(ABC).Assembly);
            var p = temp.Build();

            var abc = new ABC(0);
            abc.Update();

            abc = new ABC(1);
            abc.Update();

            p.Print("UpdateFrameTooSlow.md");

            Console.ReadLine();
        }
    }

    public class ABC {
        public string Idspace { get; set; }
        public int handle = 0;
        public int Handle;
        public ABC(int i)
        {
            Handle = i;
        }

        [SimpleProfile()]
        public void Update()
        {
            For100();
            For1000();
            For10000();
            For100000();
            For1000000();
            For10000000();
            For1000000();
        }

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
