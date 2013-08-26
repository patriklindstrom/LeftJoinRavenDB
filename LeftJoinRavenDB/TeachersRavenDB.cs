using System;
using System.Collections.Generic;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;

namespace LeftJoinRavenDB
{
    public class TeachersRavenDB : ITeachers
    {

        private List<Teacher> _list;

        public TeachersRavenDB(DocumentStore store)
        {
            using (IDocumentSession session =store.OpenSession())
            {
                var list = session.Load<TeachersMock>("TeachersMocks/1");
                _list = (List<Teacher>) list;
            }
        }

        public List<Teacher> List
        {
            get
            {
                return _list;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}