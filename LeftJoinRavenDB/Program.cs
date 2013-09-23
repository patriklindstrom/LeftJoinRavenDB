using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace LeftJoinRavenDB
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("Hello you Left joins fans");
            
           // SimpleMockupJoin();
           DocumentStore store= InitRavenDBStore();
           System.Console.WriteLine("Done Init RavenDB Start Join Workshop");
        // FillRavenDBWithOneToManyData(store);
          //  FillRavenDBWithSelfJoinData(store);
           CreateRavenDBIndex(store);
           // MapReduceJoin(store);
          // SimpleRavenDBJoin(store);
           SimpleRavenDBCountSelfJoin(store);
            //SimpleRavenDBJoin(store);
            //SimpleQueryAbleRavenDBJoin(store);

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
        }


        private static void SimpleRavenDBSelfJoin(DocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var wt = session.Query<ComparePageTextElement, LeftJoinPageTextTranslationsCount>()
                    
                    ;
                Console.WriteLine("Write BaseElement and CompareElement");
                // System.Console.WriteLine("{0}\t{1}", wt.BaseElement.Page, wt.CompareElement.Page);
                foreach (var row in wt)
                {
                    System.Console.WriteLine("{0}\t{1}\t{2}\t{3}", 
                        row.Page,row.Token,row.WebtextBase,row.WebtextCompare
                        );
                }
            }
        }

        private static void SimpleRavenDBCountSelfJoin(DocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var wt = session.Query<ComparePageTextElementCount, LeftJoinPageTextTranslationsCount>()
                    //.Where(x => x.TeacherName.StartsWith("Mrs Thatcher"))
                    ;
                Console.WriteLine("Write BaseElement and CompareElement");
                // System.Console.WriteLine("{0}\t{1}", wt.BaseElement.Page, wt.CompareElement.Page);
                foreach (var row in wt)
                {
                    System.Console.WriteLine("{0}\t{1}\t{2}",
                        row.Page, row.Token, row.Count
                        );
                }
            }
        }

        private static void MapReduceJoin(DocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var tss = session.Query<TeacherStudentStats, TeacherStudentLeftJoinIndex>()
                    .Where(x => x.TeacherName.StartsWith("Mrs Thatcher"))
                    ;
                foreach (var row in tss)
                {
                    System.Console.WriteLine("{0}\t{1}", row.TeacherName, row.ClassCount);
                }
            }
        }

        private static void SimpleQueryAbleRavenDBJoin(DocumentStore store)
        {

            //IQueryable<Teacher> teachers;
            //IQueryable<Student> students;

            //using (IDocumentSession session = store.OpenSession())
            //{

            //    //students = session.Load<Students>("Students/1").List.AsQueryable();
            //    //teachers = session.Load<Teachers>("Teachers/1").List.AsQueryable();

            //}

            //var teacherStudenList = from tList in teachers
            //                        join sList in students
            //                        on tList.Name equals sList.HomeRoomTeacher into joinedList
            //                        from sList in joinedList.DefaultIfEmpty(new Student() { Name = "-" })
            //                        select new
            //                        {
            //                            TeacherName = tList.Name,
            //                            StudentName = sList.Name
            //                        };
            //foreach (var row in teacherStudenList)
            //{
            //    System.Console.WriteLine("{0}\t{1}", row.TeacherName, row.StudentName);
            //}
            throw new NotImplementedException();
        }

        private static DocumentStore InitRavenDBStore()
        { // Install-Package RavenDB.Client -Pre
            System.Console.WriteLine("Init RavenDB");
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenHQ"

            };
            documentStore.Initialize();
            
            return documentStore;
        }

        private static void CreateRavenDBIndex(DocumentStore store)
        {
            //http://ravendb.net/docs/2.5/client-api/querying/static-indexes/defining-static-index
            IndexCreation.CreateIndexes(typeof(TeacherStudentLeftJoinIndex).Assembly, store);
            IndexCreation.CreateIndexes(typeof(LeftJoinPageTextTranslationsCount).Assembly, store);
        }

        private static void FillRavenDBWithSelfJoinData(DocumentStore store)
        {
            System.Console.WriteLine("Put selfjoin data in RavenDB");
            using (IDocumentSession session = store.OpenSession())
            {
                //Language english  https://en.wikipedia.org/wiki/Aniara
                session.Store(new PageTextElement { Page = "home", Token = "Welcome", Webtext = "Welcome to Aniara", Language = "en", Translator = "robot", CreationTime = DateTime.Now });
                session.Store(new PageTextElement { Page = "home", Token = "RulesOfBoarding", Webtext = "Do not break line", Language = "en", Translator = "robot", CreationTime = DateTime.Now });
                session.Store(new PageTextElement { Page = "home", Token = "PriceModel", Webtext = "Based on weight and oxygen consumption", Language = "en", Translator = "robot", CreationTime = DateTime.Now });
                //Here comes the one line that is missing in other language should be visible in left join
                session.Store(new PageTextElement { Page = "home", Token = "RebateModel", Webtext = "Truly Unique talent cant reduce price with 50% ", Language = "en", Translator = "robot", CreationTime = DateTime.Now });
                //Language swedish
                session.Store(new PageTextElement { Page = "home", Token = "Welcome", Webtext = "Välkommen till Aniara", Language = "sv", Translator = "robot", CreationTime = DateTime.Now });
                session.Store(new PageTextElement { Page = "home", Token = "RulesOfBoarding", Webtext = "Träng dig ej i kön", Language = "sv", Translator = "robot", CreationTime = DateTime.Now });
                session.Store(new PageTextElement { Page = "home", Token = "PriceModel", Webtext = "Priset baseras på vikt och syreförbrukning", Language = "sv", Translator = "robot", CreationTime = DateTime.Now });

                session.SaveChanges();
            }
        }
 
        private static void FillRavenDBWithOneToManyData(DocumentStore store)
        { // Run Install-Package RavenDB.Client -Pre
            System.Console.WriteLine("Put data into RavenDB");
            using (IDocumentSession session = store.OpenSession())
            {
                session.Store(new Teacher {Name = "Mrs Thatcher", Gender = "Female", YearsInService = 12});
                session.Store(new Teacher {Name = "Mr Cameron", Gender = "Male", YearsInService = 2});
                session.Store(new Teacher {Name = "Mr Walpole", Gender = "Male", YearsInService = 21});
                session.Store(new Teacher {Name = "Mr Blair", Gender = "Male", YearsInService = 10});
                session.Store(new Teacher {Name = "Mr Major", Gender = "Male", YearsInService = 7});

                session.Store(new Student {Name = "Stymie", HasBicycle = false, HomeRoomTeacher = "Mrs Thatcher"});
                session.Store(new Student {Name = "Spanky", HasBicycle = true, HomeRoomTeacher = "Mrs Thatcher"});
                session.Store(new Student {Name = "Alfalfa", HasBicycle = true, HomeRoomTeacher = "Mr Blair"});
                session.Store(new Student {Name = "Darla", HasBicycle = false, HomeRoomTeacher = "Mr Blair"});
                session.Store(new Student {Name = "Jane", HasBicycle = false, HomeRoomTeacher = "Mr Blair"});
                session.Store(new Student {Name = "Buckwheat", HasBicycle = false, HomeRoomTeacher = "Mr Major"});
                session.SaveChanges();
            }
        }

        public static void SimpleMockupJoin()
        {
           
        }
        public static void SimpleRavenDBJoinFail(DocumentStore store)
        {

            using (IDocumentSession session = store.OpenSession())
            {
                var teacherlist = session.Query<Teacher>();
                var studentlist = session.Query<Student>();
                Debug.Assert(teacherlist != null, "teachers list != null");




            foreach (var row in teacherlist)
            {
                System.Console.WriteLine("{0}\t", row.Name);
            }
            foreach (var row in studentlist)
            {
                System.Console.WriteLine("{0}\t", row.Name);
            }
            var combinedList = from teacher in teacherlist
                               join student in studentlist on teacher.Name equals student.HomeRoomTeacher
                               select new
                               {
                                   TeachersName = teacher.Name
                                   ,
                                   StudentName = student.Name
                               };

            foreach (var row in combinedList)
            {
                System.Console.WriteLine("{0}\t{1}", row.TeachersName, row.StudentName);
            }
            } // end session           
        }
        public static void SimpleRavenDBJoin(DocumentStore store)
        {
            // http://ayende.com/blog/89089/ravendb-multi-maps-reduce-indexes
            // http://ravendb.net/docs/client-api/querying/using-linq-to-query-ravendb#page
            using (IDocumentSession session = store.OpenSession())
            {
                var teacherlist = session.Query<Teacher>();
                var studentlist = session.Query<Student>();
                Debug.Assert(teacherlist != null, "teachers list != null");



                foreach (var row in session.Query<Teacher>())
                {
                    System.Console.WriteLine("{0}\t", row.Name);
                }
                foreach (var row in session.Query<Student>())
                {
                    System.Console.WriteLine("{0}\t", row.Name);
                }
                var combinedList = from teacher in session.Query<Teacher>()
                                   join student in session.Query<Student>() on teacher.Name equals student.HomeRoomTeacher
                                   select new
                                   {
                                       TeachersName = teacher.Name
                                       ,
                                       StudentName = student.Name
                                   };

                foreach (var row in combinedList)
                {
                    System.Console.WriteLine("{0}\t{1}", row.TeachersName, row.StudentName);
                }
            } // end session           
        }

        public static void SimpleJoin(ITeachers teachers, IStudents students)
        {
            
            
            var teacherStudenList = from tList in teachers.List
                                    join sList in students.List
                                    on tList.Name equals sList.HomeRoomTeacher into joinedList
                                    from sList in joinedList.DefaultIfEmpty(new Student() { Name = "-" })
                                    select new
                                    {
                                        TeacherName = tList.Name,
                                        StudentName = sList.Name
                                    };
            foreach (var row in teacherStudenList)
            {
                System.Console.WriteLine("{0}\t{1}", row.TeacherName, row.StudentName);
            }
            
        }


    }

  
}
