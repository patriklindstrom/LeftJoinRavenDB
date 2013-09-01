using System.Collections.Generic;
using System.Linq;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace LeftJoinRavenDB
{
    public class StudentRavenDB : IStudents
    {
        private IQueryable<Student> _list;

        public StudentRavenDB(DocumentStore store)
        {
            using (IDocumentSession session = store.OpenSession())
            {

                IQueryable<StudentRavenDB> students = session.Query<StudentRavenDB>();
            }
        }

        public IQueryable<Student> List
        {
            get { return _list; }
            set { _list = value as IQueryable<Student>; }
        }
    }
}