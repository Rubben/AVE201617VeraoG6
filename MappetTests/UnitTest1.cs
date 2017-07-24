using Microsoft.VisualStudio.TestTools.UnitTesting;
using AveTrabalho2;
using AveTrabalho2.Mapping;

namespace MapperTest
{
    public class Teacher
    {
        public Teacher() {}
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
    /// <summary>
    ///  MapperTestInit tests the first example for the mapper
    /// </summary>
    [TestClass]
    public class MapperTests<TSrc, TDest>
    {
        public MapperTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreationOfStudent()
        {
            Student​ s1 = new Student();
            s1.NrP = 27721;
            s1.NameP = "ana";
           
            Assert.IsTrue(s1.NameP.Equals("ana"));

        }

        [TestMethod]
        public void CreationOfPerson()
        {
            Person s = new Person();
            s.IdP = 27721;
            s.NameP = "ze Manel";

            Assert.IsTrue(s.NameP.Equals("ze Manel"));

        }

        [TestMethod]
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
            Student​[] stds = { s1,s2 };
            Person[] prsn = (Person[])m.MapLazy(stds);
            bool allgood = false;
            for (int i = 0; i < stds.Length; i++)
            {
                allgood = allgood || stds[i].NameP.Equals(prsn[i].NameP);
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
            Person[] prsn = (Person[])m.MapLazy(stds);
            bool allgood = false;
            for (int i = 0; i < stds.Length; i++)
            {
                allgood = allgood || stds[i].NameF.Equals(prsn[i].NameF);
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
        public void TestMethodMapperAutoMapperILUsingProprieties()
        {
            Student​ s = new Student();
            s.NrP = 27721;
            s.NameP = "ze Manel";


            IMapper<Student​, Person> m = AutoMapper.Build<Student​, Person>().Bind(Mapping.PropertiesIl);

            Person p = (Person)m.Map(s);
            Assert.IsTrue(s.NameP.Equals(p.NameP));
        }
    }
}
