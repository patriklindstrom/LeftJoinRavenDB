using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;

namespace LeftJoinRavenDB.Models
{
  public  class Student
    {
        public String Name { get; set; }
        public String HomeRoomTeacher { get; set; }
        public Boolean HasBicycle  { get; set; }
    }
}


public class StudentsMock :IStudents
{
    public IQueryable<Student> List { get; set; }

    public StudentsMock()
    {
        List = new EnumerableQuery<Student>(new List<Student>()
        {
            new Student {Name = "Stymie", HasBicycle = false, HomeRoomTeacher = "Mrs Thatcher"}
            ,
            new Student {Name = "Spanky", HasBicycle = true, HomeRoomTeacher = "Mrs Thatcher"}
            ,
            new Student {Name = "Alfalfa", HasBicycle = true, HomeRoomTeacher = "Mr Blair"}
            ,
            new Student {Name = "Darla", HasBicycle = false, HomeRoomTeacher = "Mr Blair"}
            ,
            new Student {Name = "Jane", HasBicycle = false, HomeRoomTeacher = "Mr Blair"}
            ,
            new Student {Name = "Buckwheat", HasBicycle = false, HomeRoomTeacher = "Mr Major"}
        });
    }
}

public  class  Students:IStudents
{
    public IQueryable<Student> List { get; set; }

}
internal interface IStudents
{
     IQueryable<Student> List { get; set; }
   
}