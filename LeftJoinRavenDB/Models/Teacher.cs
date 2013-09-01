using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftJoinRavenDB.Models
{
    public class Teacher
    { public String Name { get; set; }
      public String Gender { get; set; }
      public int YearsInService { get; set; }
    }

    public class TeachersMock : ITeachers
    {
        public IQueryable<TeachersRavenDB> List { get; set; }
        public  TeachersMock()
        {
            //List = new EnumerableQuery<Teacher>(new List<Teacher>()
            //{
            //    new Teacher { Name = "Mrs Thatcher", Gender = "Female", YearsInService = 12 }
            //,   new Teacher { Name = "Mr Cameron", Gender = "Male", YearsInService = 2 }
            //,   new Teacher { Name = "Mr Walpole", Gender = "Male", YearsInService = 21 }
            //,   new Teacher { Name = "Mr Blair", Gender = "Male", YearsInService = 10 }
            //,   new Teacher { Name = "Mr Major", Gender = "Male", YearsInService = 7 }
            //}

            //);
            
        }
    }

    public class Teachers : ITeachers
    {
     public   IQueryable<TeachersRavenDB> List { get; set; }

    }
  public   interface ITeachers
  {
       IQueryable<TeachersRavenDB> List { get; set; }
         
     }
}
