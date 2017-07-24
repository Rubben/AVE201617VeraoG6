using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AveTrabalho2.Mapping
{

    public class S
    {
        public string b { get; set;}
        public int a;
        public S s;

        public S()
        {
        
        }
    }

    public class R
    {
        public string b { get; set; }
        public int c;
        public R s;

    }

    public class Example
        {
        public static void Main(string[] args)
        {
            S s = new S();
            s.a = 10;
            s.b = "zau";
            s.s = new S();
            s.s.a = 12;
            s.s.b = "zau2";


            S s1 = new S();
            s1.a = 11;
            s1.b = "zau1";
            s1.s = new S();
            s1.s.a = 13;
            s1.s.b = "zau3";


            S[] s2 = { s, s1 };


            Mapper<S, R> m = AutoMapper.Build<S, R>()
            .Bind()
            .Match("a", "c");
  
            List<R> r1 = m.Map<List<R>>(s2);

            Console.WriteLine("s");
            Console.WriteLine(r1[0].b);
            Console.WriteLine(r1[0].c);
            //Console.WriteLine(r1[0].s.GetType());
            //Console.WriteLine(r1[0].s.b);
            //Console.WriteLine(r1[0].s.c);
            Console.WriteLine("s1");
            Console.WriteLine(r1[1].b);
            Console.WriteLine(r1[1].c);
            //Console.WriteLine(r1[1].s.GetType());
            //Console.WriteLine(r1[1].s.b);
            //Console.WriteLine(r1[1].s.c);


            int k = 0;

            



        }
    }
}
