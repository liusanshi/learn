using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Impl.Test.DomainModel
{
    public class Class
    {
        public Class() { }

        public virtual UInt32 Id { get; set; }
        public virtual string Name { get; set; }

        public string Slogan { get; set; }

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
