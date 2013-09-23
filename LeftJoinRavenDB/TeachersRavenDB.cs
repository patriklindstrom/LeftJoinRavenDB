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
        private IRavenQueryable<Teacher> _list;

        public TeachersRavenDB(DocumentStore store)
        {
            using (IDocumentSession session =store.OpenSession())
            {
                var teachers = session.Query<Teacher>();
                Debug.Assert(teachers != null, "teachers != null");
                ListRavenQueryableTeachers = teachers;
            }
        }


        public IRavenQueryable<Teacher> ListRavenQueryableTeachers { get; set; }

        public IRavenQueryable<Teacher> List
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}