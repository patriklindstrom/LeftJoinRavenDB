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
          
            IStudents students = new StudentsMock();


            var teacherStudenList = from tList in teachers.List
                           join sList in students.List
                           on tList.Name equals sList.HomeRoomTeacher into joinedList
                                     from sList in joinedList.DefaultIfEmpty(new Student(){Name = "-"})
                           select new
                           {
                             TeacherName = tList.Name,  
                             StudentName =  sList.Name 
                                
                           };
            foreach (var row in teacherStudenList)
            {
                System.Console.WriteLine("{0}\t{1}",row.TeacherName,row.StudentName);
            }
            System.Console.WriteLine("Hello you Left joins fans");

            System.Console.WriteLine("Enter To Exit..");
            System.Console.ReadLine();
            ;
        }
    }
}
