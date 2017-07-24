using AveTrabalho2.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2.Mapping
{
    public class MapPropertiesReflex : Mapping
    {
        Type t1;
        Type t2;
        ArrayList spec = new ArrayList();
        Dictionary<string, object> dicti = new Dictionary<string, object>();


        public override object map(object transfrom)
        {
            object transTo = Activator.CreateInstance(t2);
            PropertyInfo[] prop = t1.GetProperties();
            foreach (PropertyInfo aux in prop)
            {
                string a = aux.Name;
                PropertyInfo transToProperty = t2.GetProperty(a);
                if (transToProperty != null && aux.GetValue(transfrom) != null )
                {

                    if ((aux.PropertyType.IsPrimitive && transToProperty.PropertyType.IsPrimitive) || (aux.PropertyType==typeof(string) && transToProperty.PropertyType == typeof(string)) || transToProperty.PropertyType == aux.PropertyType)
                    {
                        transToProperty.SetValue(transTo, aux.GetValue(transfrom));
                    }
                    else
                    {
                        Mapper<object, object> m = AutoMapper.Build<object, object>(aux.PropertyType, transToProperty.PropertyType).Bind();
                        transToProperty.SetValue(transTo, m.Map(aux.GetValue(transfrom)));
                    }
                }

            }
            transTo = MapPropertySetSpec(transfrom, transTo);
            transTo = doFor(transTo);
            return transTo;
        }

        private Object MapPropertySetSpec(object transfrom, object transTo)
        {
            for (int i = 0; i < spec.Count - 1; i = i + 2)
            {
                PropertyInfo prop = t1.GetProperty(spec[i] as string);
                PropertyInfo transToProperty = t2.GetProperty(spec[i + 1] as string);
                if (prop != null && transToProperty != null)
                {
                    if (prop.PropertyType == transToProperty.PropertyType)
                    {
                        transToProperty.SetValue(transTo, prop.GetValue(transfrom));
                    }
                    else
                    {
                        Mapper<object, object> m = AutoMapper.Build<object, object>(prop.PropertyType, transToProperty.PropertyType).Bind(Mapping.FieldsReflex);
                        transToProperty.SetValue(transTo, m.Map(prop.GetValue(transfrom)));
                    }
                }
            }


            return transTo;
        }
        public override void addMatch(string nameFrom, string nameDest)
        {
            spec.Add(nameFrom);
            spec.Add(nameDest);
        }

        
        public override void mapper(Type TSrc, Type TDest)
        {
            t1 = TSrc;
            t2 = TDest;
        }

        public override void For(string nameFrom, Func<object> fnc)
        {
            dicti.Add(nameFrom, fnc());
        }

        private object doFor(object transTo)
        {
            PropertyInfo[] prop = t2.GetProperties();
            foreach (PropertyInfo aux in prop)
            {
                if (dicti.ContainsKey(aux.Name)) { 
                    object val = dicti[aux.Name];
                    aux.SetValue(transTo, val);
                }
            }
            return transTo;
        }
    }

}
