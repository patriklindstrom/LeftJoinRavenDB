using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LeftJoinRavenDB.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace LeftJoinRavenDB
{
    public class TeachersRavenDB : ITeachers
    {


        private IQueryable<TeachersRavenDB> _list;

        public TeachersRavenDB(DocumentStore store)
        {
            using (IDocumentSession session =store.OpenSession())
            {
                var teachers = session.Query<TeachersRavenDB>().AsQueryable();
                Debug.Assert(teachers != null, "teachers != null");
                _list = teachers;
            }
        }

        public IQueryable<TeachersRavenDB> List
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