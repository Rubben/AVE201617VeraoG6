
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AveTrabalho2.Mapping
{
    public class MapProperty:Mapping
    {

        private AssemblyBuilder asm = null;
        private TypeBuilder typeBuilder = null;
        private FieldBuilder fieldBuilder = null;
        private ILGenerator ilGenerator = null;
        private Type Src = null;
        private Type Dest = null;
        private ILinterface dynMap = null;
        private Type dynCipierType = null;

        private Boolean complete = false;


        public override void mapper(Type TSrc, Type TDest)
        {
            Src = TSrc;
            Dest = TDest;
            const string asmName = "MapFields";
            asm = CreateAsm(asmName);
            ModuleBuilder mb = asm.DefineDynamicModule(asmName, asmName + ".dll");

            typeBuilder = mb.DefineType("MapFields", TypeAttributes.Public, typeof(object), new Type[] { typeof(ILinterface) });


            fieldBuilder = typeBuilder.DefineField(
                "dest",
                Dest,
                FieldAttributes.Public
                );
            //construtor com parametros
            ConstructorBuilder ctr = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[] { TDest });

            ILGenerator ilctr = ctr.GetILGenerator();
            ilctr.Emit(OpCodes.Ldarg_0);
            ilctr.Emit(OpCodes.Ldarg_1);
            ilctr.Emit(OpCodes.Stfld, fieldBuilder);
            ilctr.Emit(OpCodes.Ret);



            //construtor sem parametros
            ConstructorBuilder ctr1 = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                null);

            ILGenerator ilctr1 = ctr1.GetILGenerator();
            ilctr1.Emit(OpCodes.Ldarg_0);
            ilctr1.Emit(OpCodes.Newobj,
                Dest.GetConstructor(Type.EmptyTypes));
            ilctr1.Emit(OpCodes.Stfld, fieldBuilder);
            ilctr1.Emit(OpCodes.Ret);




            MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                "map",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(object),//return
                new Type[] { typeof(object) }//parametros de entrada
            );

            ilGenerator = methodBuilder.GetILGenerator();
            PropertyInfo[] proper = Src.GetProperties();
            foreach (PropertyInfo prt in proper)
            {
                PropertyInfo prop = Dest.GetProperty(prt.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.PropertyType == prt.PropertyType)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Callvirt, prt.GetMethod);
                    ilGenerator.Emit(OpCodes.Callvirt, prop.SetMethod);
                }
            }

        }


        public override void addMatch(string nameFrom, string nameDest)
        {
            PropertyInfo prt = Src.GetProperty(nameFrom);

            PropertyInfo prop = Dest.GetProperty(nameDest);
            if(prt!=null && prop != null) {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Callvirt, prt.GetMethod);
                ilGenerator.Emit(OpCodes.Callvirt, prop.SetMethod);
            }
            
        }


        public override object map(object src)
        {
            if (!complete)
            {
                callToSave();
                dynMap = (ILinterface)Activator.CreateInstance(dynCipierType);
            }
            
            object ne = dynMap.map(src);
            ne = doFor(ne);
            return ne;

        }

        public object map(object src, object dest)
        {
            if (!complete)
            {
                callToSave();
                
            }
            dynMap = (ILinterface)Activator.CreateInstance(dynCipierType, dest);
            object ne = dynMap.map(src);
            ne = doFor(ne);
            return ne;

        }

        public void callToSave() {

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);
            dynCipierType = typeBuilder.CreateType();
            asm.Save("MapProperties" + ".dll");
            complete = true;
        }


        public AssemblyBuilder CreateAsm(string name)
        {
            AssemblyName aName = new AssemblyName(name);

            AssemblyBuilder ab =
                AssemblyBuilder.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);
            return ab;
        }

        Dictionary<string, Func<object>> dicti = new Dictionary<string, Func<object>>();
        public override void For(string nameFrom, Func<object> fnc)
        {
            dicti.Add(nameFrom, fnc);
        }

        public object doFor(object transTo)
        {
            PropertyInfo prop;
            foreach (KeyValuePair<string, Func<object>> aux in dicti)
            {
                if ((prop = Dest.GetProperty(aux.Key)) != null)
                {
                    prop.SetValue(transTo, aux.Value());
                }
            }
            return transTo;
        }
    }
}
