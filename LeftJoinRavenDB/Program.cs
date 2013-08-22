using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;

namespace LeftJoinRavenDB
{
    class Program
    {
        static void Main(string[] args)
        { ITeachers teachers  = new TeachersMock();
            teachers.SetTeachersList();
            IStudents students = new StudentsMock();
            students.SetStudents();

            System.Console.WriteLine("Hello you Left joins fans");

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
        }
    }
}
