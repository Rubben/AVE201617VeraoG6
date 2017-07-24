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
    public class MapFieldsReflex : Mapping
    {
        Dictionary<string, object> dicti = new Dictionary<string, object>();
        ArrayList spec = new ArrayList();
        Type t1;
        Type t2;


       

        public Mapping Match(string src, string dest)
        {
            spec.Add(src);
            spec.Add(dest);
            return this;
        }

        public override object map(object transfrom) { 
            object transTo= Activator.CreateInstance(t2);
            FieldInfo[] prop = t1.GetFields();
            foreach (FieldInfo aux in prop)
            {
                string a = aux.Name;
                FieldInfo transTofield = t2.GetField(a);
                if (transTofield != null && aux.GetValue(transfrom)!=null )
                {
                    if ((aux.FieldType.IsPrimitive && transTofield.FieldType.IsPrimitive) || (aux.FieldType == typeof(string) && transTofield.FieldType == typeof(string))|| transTofield.FieldType == aux.FieldType)
                    {
                      
                        transTofield.SetValue(transTo, aux.GetValue(transfrom));
                        
                    }
                    else
                    {

                        Mapper<object, object> m = AutoMapper.Build<object, object>(aux.FieldType, transTofield.FieldType).Bind();
                        transTofield.SetValue(transTo, m.Map(aux.GetValue(transfrom)));
                    }
                }

            }
            transTo = MapFieldsSetSpec(transfrom, transTo);
            transTo = doFor(transTo);
            return transTo;
        }

        private object doFor(object transTo)
        {
            FieldInfo[] prop = t2.GetFields();
            foreach (FieldInfo aux in prop)
            {
                if (dicti.ContainsKey(aux.Name)) { 
                    object val = dicti[aux.Name];
                    aux.SetValue(transTo, val);
                }
            }
            return transTo;
        }

        private Object MapFieldsSetSpec(object transfrom, object transTo)
        {
            for (int i = 0; i < spec.Count - 1; i = i + 2)
            {
                FieldInfo prop = t1.GetField(spec[i] as string);
                FieldInfo transTofield = t2.GetField(spec[i + 1] as string);
                if (prop != null && transTofield != null)
                {
                    if (prop.FieldType == transTofield.FieldType)
                    {
                        transTofield.SetValue(transTo, prop.GetValue(transfrom));
                    }
                    else
                    {

                        Mapper<object, object> m = AutoMapper.Build<object, object>(prop.FieldType, transTofield.FieldType).Bind(Mapping.FieldsReflex);
                        transTofield.SetValue(transTo, m.Map(prop.GetValue(transfrom)));
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

        public override void For(string nameFrom, Func<object> fnc)
        {
            dicti.Add(nameFrom, fnc());
        }

        public override void mapper(Type TSrc, Type TDest)
        {
            t1 = TSrc;
            t2 = TDest;
        }
    }
}
