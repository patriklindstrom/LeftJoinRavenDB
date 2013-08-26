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
                Teachers teachers = session.Load<Teachers>("Teachers/1");
                _list = teachers.List;
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