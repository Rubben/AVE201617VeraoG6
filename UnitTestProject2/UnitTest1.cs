using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AveTrabalho2;
using AveTrabalho2.Mapping;
using System.Collections.Generic;

namespace UnitTestProject2
{
    

    public class Teacher
    {
        public Teacher() { }
        public string NameP { get; set; }
        public int IdP { get; set; }
        public string NameF;
        public int IdF;
    }
    public class Student
    {

        public string NameP { get; set; }
        public int NrP { get; set; }
        public string NameF;
        public int NrF;

        public Student() { }


    }
    public class Person
    {
        public string NameP { get; set; }
        public int IdP { get; set; }
        public string NameF;
        public int IdF;

    }
   

    [TestClass]
    public class MapperTests
    {

        public void CreationOfStudent()
        {
            Student​ s1 = new Student();
            s1.NrP = 27721;
            s1.NameP = "ana";

            Assert.IsTrue(s1.NameP.Equals("ana"));

        }


        public void CreationOfPerson()
        {
            Person s = new Person();
            s.IdP = 27721;
            s.NameP = "ze Manel";

            Assert.IsTrue(s.NameP.Equals("ze Manel"));

        }


        public void CreationOfTeacher()
        {
            Teacher s1 = new Teacher();
            s1.IdP = 27721;
            s1.NameP = "Filipa";

            Assert.IsTrue(s1.NameP.Equals("Filipa"));

        }

        [TestMethod]
        public void TestMethodStudentToPersonFieldsReflex()
        {
            Student​ s = new Student();
            s.NrF = 27721;
            s.NameF = "ze Manel";

            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsReflex);
            Person p = (Person)m.Map(s);
            Assert.IsTrue(s.NameF.Equals(p.NameF));

        }

        [TestMethod]
        public void TestMethodMapperAutoMapperReflexUsingProprieties()
        {
            Mapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.PropertiesReflex).Match("NrP", "IdP");
            Student​ s1 = new Student();
            s1.NrP = 27721;
            s1.NameP = "ze Manel";
            Student​ s2 = new Student();
            s2.NrP = 11111;
            s2.NameP = "maria";
            Student​[] stds = { s1, s2 };
            IEnumerable<Person> prsn = m.MapLazy(stds);
            bool allgood = false;
            int i = 0;
            foreach(Person aux in prsn)
            {
                allgood = allgood || stds[i].NameP.Equals(aux.NameP);
                i++;
            }

            Assert.IsTrue(allgood);
        }


        [TestMethod]
        public void TestMethodMapperAutoMapperReflexUsingFields()
        {
            Student​ s1 = new Student();
            s1.NrF = 27721;
            s1.NameF = "ze Manel";
            Student​ s2 = new Student();
            s2.NrF = 11111;
            s2.NameF = "maria";
            Student​[] stds = { s1, s2 };
            Mapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsReflex).Match("NrF", "IdF");
            IEnumerable<Person> prsn = m.MapLazy(stds);
            bool allgood = false;
            int i = 0;
            foreach (Person aux in prsn)
            {
                allgood = allgood || stds[i].NameF.Equals(aux.NameF);
                i++;
            }

            Assert.IsTrue(allgood);
        }

        [TestMethod]
        public void TestMethodMapperAutoMapperILUsingFields()
        {
            Student​ s = new Student();
            s.NrF = 27721;
            s.NameF = "ze Manel";

            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsIl);

            Person p = (Person)m.Map(s);
            Assert.IsTrue(s.NameF.Equals(p.NameF));
        }

        [TestMethod]
        public void TestMethodFor()
        {
            Student​ s = new Student();
            s.NrF = 27721;
            s.NameF = "ze Manel";

            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsIl).For("IdF",()=>11111);

            Person p = (Person)m.Map(s);
            Assert.IsTrue(s.NameF.Equals(p.NameF) && p.IdF== 11111);
        }



        [TestMethod]
        public void TestMethodCollection()
        {
            Student[] stds = { new Student{ NrF = 27721, NameF = "Ze Manel"},
                               new Student{ NrF = 15642, NameF = "Maria Papoila"}};
            Person[] expected = { new Person{ IdF = 27721, NameF = "Ze Manel"},
                                  new Person{ IdF = 15642, NameF = "Maria Papoila"}};
            Mapper<Student, Person> m = AutoMapper.Build<Student, Person>().Match("NrF","IdF");
            List<Person> ps = m.Map<List<Person>>(stds);
            Person[] aux = ps.ToArray();
            Assert.IsTrue(expected[0].IdF==aux[0].IdF                 && expected[1].IdF == aux[1].IdF                && expected[0].NameF == aux[0].NameF                && expected[1].NameF == aux[1].NameF);          
        }


        [TestMethod]
        public void TestMethodMapperAutoMapperILUsingProprieties()
        {
            Student​ s = new Student();
            s.NrP = 27721;
            s.NameP = "ze Manel";


            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.PropertiesIl);

            Person p = (Person)m.Map(s);
            Assert.IsTrue(s.NameP.Equals(p.NameP));
        }



        [TestMethod]
        public void TestMethodTimeILFasterthanReflex()
        {
            Student​ s = new Student();
            s.NrF = 27721;
            s.NameF = "ze Manel";

            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsReflex);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 50000; i++) { Person p = (Person)m.Map(s); }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;


            m = AutoMapper.Build<Student​, Person>().Bind(Mapping.FieldsIl);


            watch = System.Diagnostics.Stopwatch.StartNew();
           

            for (int i = 0; i < 50000; i++) { Person p = (Person)m.Map(s); }
            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;
           
            Assert.IsTrue(elapsedMs1< elapsedMs);



        }
    }
}
