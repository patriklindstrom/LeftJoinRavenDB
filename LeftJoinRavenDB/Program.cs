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
         // FillRavenDBWithData(store);
          // CreateRavenDBIndex(store);
            MapReduceJoin(store);
          // SimpleRavenDBJoin(store);
            
            //SimpleRavenDBJoin(store);
            //SimpleQueryAbleRavenDBJoin(store);

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
        }

        private static void MapReduceJoin(DocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                var tss = session.Query<TeacherStudentStats, TeacherCountsByStudent>()
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


            IndexCreation.CreateIndexes(typeof(TeacherCountsByStudent).Assembly, store);

        }

        
        private static void FillRavenDBWithData(DocumentStore store)
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
            
            
            var teacherStudenList = from tList in teachers.ListRavenQueryableTeachers
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
