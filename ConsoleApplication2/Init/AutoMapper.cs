using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2.Mapping
{
    public class AutoMapper
    {
        public static Mapper<TSrc, TDest> Build<TSrc, TDest>()
        {
            return new Mapper<TSrc, TDest>();
        }

        //utilizado pela reflexao para os casos em que existem objectos dentro de objectos
        public static Mapper<TSrc, TDest> Build<TSrc, TDest>(Type src, Type dest)
        {
            return new Mapper<TSrc, TDest>(src,dest);
        }
    }
}
