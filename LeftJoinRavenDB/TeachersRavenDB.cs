using System;
using System.Collections.Generic;
using LeftJoinRavenDB.Models;
using Raven.Client.Document;

namespace LeftJoinRavenDB
{
    public class TeachersRavenDB : ITeachers
    {
        public TeachersRavenDB(DocumentStore store)
        {
            throw new NotImplementedException();
        }

        public List<Teacher> List
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}