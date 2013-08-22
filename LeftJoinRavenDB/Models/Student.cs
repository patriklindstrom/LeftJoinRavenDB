using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;

namespace LeftJoinRavenDB.Models
{
    class Student
    {
        public String Name { get; set; }
        public String HomeRoomTeacher { get; set; }
        public Boolean HasBicycle  { get; set; }
    }
}


class StudentsMock :IStudents
{
    public List<Student> List { get; set; }

    public void SetStudents()
    {
        List = new List<Student>
        {
                new Student{Name = "Stymie",HasBicycle = false,HomeRoomTeacher = "Mrs Thatcher"}
            ,   new Student{Name = "Spanky",HasBicycle = true ,HomeRoomTeacher = "Mrs Thatcher"}
            ,   new Student{Name = "Alfalfa",HasBicycle = true ,HomeRoomTeacher = "Mr Blair"}
            ,   new Student{Name = "Darla",HasBicycle = false ,HomeRoomTeacher = "Mr Blair"}
            ,   new Student{Name = "Jane",HasBicycle = false ,HomeRoomTeacher = "Mr Blair"}
            ,   new Student{Name = "Buckwheat",HasBicycle = false ,HomeRoomTeacher = "Mr Major"}
        };
    }
}

internal interface IStudents
{
    void SetStudents();
}