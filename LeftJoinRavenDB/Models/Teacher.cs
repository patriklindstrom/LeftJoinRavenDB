using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Util;
using Raven.Client.Linq;

namespace LeftJoinRavenDB.Models
{
    public class Teacher
    { public String Name { get; set; }
      public String Gender { get; set; }
      public int YearsInService { get; set; }
    }

   

    public class Teachers : ITeachers
    {
        public IRavenQueryable<Teacher> List { get; set; }
    }
  public   interface ITeachers
  {
      IRavenQueryable<Teacher> List { get; set; }
         
     }
}
