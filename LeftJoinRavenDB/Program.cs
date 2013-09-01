using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;

namespace LeftJoinRavenDB
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("Hello you Left joins fans");
            
           // SimpleMockupJoin();
           DocumentStore store= InitRavenDBStore();
         //  FillRavenDBWithData(store);
           //  CreateRavenDBIndex(store);
            SimpleRavenDBJoin(store);
            //SimpleQueryAbleRavenDBJoin(store);

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
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


            throw new System.NotImplementedException();

        }

        
        private static void FillRavenDBWithData(DocumentStore store)
        { // Run Install-Package RavenDB.Client -Pre
            System.Console.WriteLine("Put data into RavenDB");
            using (IDocumentSession session = store.OpenSession())
            {
                var teachers = new Teachers {List = new TeachersMock().List};
                var students = new Students { List = new StudentsMock().List };
                session.Store(teachers);
                session.Store(students);
                session.SaveChanges();
            }
        }

        public static void SimpleMockupJoin()
        {
            SimpleJoin(new TeachersMock(), new StudentsMock());
        }
        public static void SimpleRavenDBJoin(DocumentStore store)
        {
            SimpleJoin(new TeachersRavenDB(store), new StudentRavenDB(store));
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
