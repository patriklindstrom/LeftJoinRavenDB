using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeftJoinRavenDB.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LeftJoinRavenDB
{
    public class TeacherStudentStats
    {
        public string TeacherName { get; set; }
      
        public int ClassCount { get; set; }
    }
    public class TeacherCountsByStudent : AbstractMultiMapIndexCreationTask<TeacherStudentStats>
    {
        public TeacherCountsByStudent()
        {
            AddMap<Teacher>(teachers => from teacher in teachers
                select new
                {
                    TeacherName = teacher.Name,                
                    ClassCount = 0
                });

            AddMap<Student>(students => from student in students
                                         select new
                                         {
                                             TeacherName = student.HomeRoomTeacher,
                                             ClassCount = 1
                                         }
                                            );
            Reduce = results => from result in results
                group result by result.TeacherName
                into g
                select new
                {
                    TeacherName = g.Select(x => x.TeacherName).Where(x => x != null).First(),
                    ClassCount = g.Sum(x => x.ClassCount)

                };


            Index(x => x.TeacherName, FieldIndexing.Analyzed);
        }
    }
}
