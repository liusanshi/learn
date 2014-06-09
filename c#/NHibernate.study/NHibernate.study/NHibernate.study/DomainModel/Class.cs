using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Study.DomainModel
{
    public class Class
    {
        public Class() { }

        private IList<Student> mStudents;

        public virtual UInt32 Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<Student> Students { get;  set; }

        public void AddStudent(Student stu)
        {
            if (Students == null)
            {
                Students = new List<Student>();
            }
            Students.Add(stu);
            stu.Class = this;
        }

    }
}
