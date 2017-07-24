using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2.Mapping
{
    public class Mapper<TSrc, TDest> : IMapper<TSrc, TDest>
    {
        Type klassSrc;
        Type klassDest;
        Mapping mapThis;
        bool state = false;


        public Mapper<TSrc, TDest> Bind(Mapping m)
        {
            state = true;
            mapThis=m;
            mapThis.mapper(klassSrc, klassDest);
            return this;
        }

        public Mapper<TSrc, TDest> Bind()
        {
            state = true;
            mapThis = Mapping.MapIL;
            mapThis.mapper(klassSrc, klassDest);
            return this;
        }




        public Mapper()
        {
            this.klassSrc =  typeof(TSrc);
            this.klassDest = typeof(TDest);
        }

        public Mapper(Type src, Type dest)
        {
            this.klassSrc = src;
            this.klassDest = dest;
        }

        public Mapper<TSrc, TDest> Match(string nameFrom, string nameDest)
        {
            if (!state)
            {
                Bind();
            }
            mapThis.addMatch(nameFrom, nameDest);
            return this;
        }

        public Mapper<TSrc, TDest> For(string nameFrom, Func<object> fuc)
        {
            if (!state)
            {
                Bind();
            }
            mapThis.For(nameFrom, fuc);
            return this;
        }


        public IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src) {
            foreach (TSrc curr in src) {
                yield return Map(curr);
            }
            
            yield break;
        }

        public TDest Map(TSrc src)
        {
            if (!state)
            {
                Bind();
            }
            TDest aux=  (TDest)mapThis.map(src);
            return aux;
        }

        public T Map<T>(TSrc[] src)
        {
            Type z = typeof(T);
            List<TDest> a1 = new List<TDest>();


            for (int i = 0; i < src.Length; i++)
            {
                a1.Add((TDest)Map(src[i]));

            }
            return (T)Convert.ChangeType(a1, typeof(T));
        }
    }
}
