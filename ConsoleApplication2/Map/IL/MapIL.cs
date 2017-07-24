using AveTrabalho2;
using AveTrabalho2.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Map.IL
{
    //Classe utilizada como defaul quando Bind nao recebe parametros
    //realiza a transformaçao com il dos Fields como das Properties
    //faz chamadas para as class MapFields e MapProperty para realizar as operaçoes

    class MapIL : Mapping
    {
        MapFields mf;
        MapProperty mp;
        public MapIL()  {
            mf = new MapFields();
            mp = new MapProperty();
        }

        public override void addMatch(string nameFrom, string nameDest)
        {
            mf.addMatch(nameFrom, nameDest);
            mp.addMatch(nameFrom, nameDest);
        }

        public override void For(string nameFrom, Func<object> fnc)
        {
            mf.For(nameFrom, fnc);
            mp.For(nameFrom, fnc);
        }

        public override void mapper(Type TSrc, Type TDest)
        {
            mf.mapper(TSrc, TDest);
            mp.mapper(TSrc, TDest);
        }

        public override object map(object transfrom)
        {
            object ne=mf.map(transfrom);


            return mp.map(transfrom, ne);
            
        }
    }
}
