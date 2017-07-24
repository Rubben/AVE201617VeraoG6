using ConsoleApplication2.Map.IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2.Mapping
{
    abstract public class Mapping
    {
        public static Mapping MapIL = new MapIL();
        public static Mapping FieldsReflex = new MapFieldsReflex();
        public static Mapping PropertiesReflex = new MapPropertiesReflex();

        public static Mapping FieldsIl = new MapFields();
        public static Mapping PropertiesIl = new MapProperty();

        public Mapping()
        {
            
        }

  

        abstract public void addMatch(string nameFrom, string nameDest);
        abstract public object map(object transfrom);
        abstract public void mapper(Type TSrc, Type TDest);
        abstract public void For(string nameFrom, Func<object> fuc);
    }

   
}
