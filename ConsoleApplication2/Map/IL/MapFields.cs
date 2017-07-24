using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;


namespace AveTrabalho2.Mapping
{

    class MapFields : Mapping
    {

        public AssemblyBuilder asm = null;
        public TypeBuilder typeBuilder = null;
        public FieldBuilder fieldBuilder = null;
        public ILGenerator ilGenerator = null;
        private Type Src = null;
        private Type Dest = null;
        public ILinterface dynMap = null;
        public Type dynCipierType = null;

        public Boolean complete = false;


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

            //contrutor com parametro
            ConstructorBuilder ctr = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[] { Dest });

            ILGenerator ilctr = ctr.GetILGenerator();
            ilctr.Emit(OpCodes.Ldarg_0);
            ilctr.Emit(OpCodes.Ldarg_1);
            ilctr.Emit(OpCodes.Stfld, fieldBuilder);
            ilctr.Emit(OpCodes.Ret);


            //contrutor sem parametro
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
            FieldInfo[] fields = Src.GetFields();
            foreach (FieldInfo fsr in fields)
            {
                FieldInfo fdt = Dest.GetField(fsr.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (fdt != null && fdt.FieldType== fsr.FieldType)
                {
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Ldfld, fsr);
                    ilGenerator.Emit(OpCodes.Stfld, fdt);
                }
            }

        }


        public override void addMatch(string nameFrom, string nameDest)
        {
            FieldInfo field = Src.GetField(nameFrom, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo fdt = Dest.GetField(nameDest, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (field != null && fdt != null) {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Ldfld, field);
                ilGenerator.Emit(OpCodes.Stfld, fdt);
            }
            
        }




        public override object map(object src)
        {
            if (!complete) {callToSave();}

            dynMap = (ILinterface)Activator.CreateInstance(dynCipierType);
            object ne = dynMap.map(src);
            ne = doFor(ne);
            return ne;

        }

        public void callToSave()
        {

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);
            dynCipierType = typeBuilder.CreateType();
            asm.Save("MapFields" + ".dll");
            complete = true;
        }


        private AssemblyBuilder CreateAsm(string name)
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
            FieldInfo prop;
            foreach (KeyValuePair<string, Func<object>> aux in dicti) {
                if ((prop = Dest.GetField(aux.Key)) != null) {
                    prop.SetValue(transTo, aux.Value());
                }
            }
            return transTo;
        }
    }
}