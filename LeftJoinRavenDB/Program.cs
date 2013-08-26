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
           FillRavenDBWithData(store);
           //  CreateRavenDBIndex(store);
            SimpleRavenDBJoin(store);

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
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
                session.Store(new TeachersMock());
                session.Store(new StudentsMock());
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
