using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2
{

    public interface IMapper<TSrc, TDest>
    {

        TDest Map(TSrc src);
        T Map<T>(TSrc[] src);
        IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src);        
    }
}
