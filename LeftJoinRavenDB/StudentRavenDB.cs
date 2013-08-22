using System.Collections.Generic;
using LeftJoinRavenDB.Models;
using Raven.Client.Document;

namespace LeftJoinRavenDB
{
    public class StudentRavenDB : IStudents
    {
        private List<Student> _list;

        public StudentRavenDB(DocumentStore store)
        {
            throw new System.NotImplementedException();
        }

        public List<Student> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}