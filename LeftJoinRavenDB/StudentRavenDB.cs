using System.Collections.Generic;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;

namespace LeftJoinRavenDB
{
    public class StudentRavenDB : IStudents
    {
        private List<Student> _list;

        public StudentRavenDB(DocumentStore store)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                var list = session.Load<StudentsMock>("StudentsMocks/1");
            }
        }

        public List<Student> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}