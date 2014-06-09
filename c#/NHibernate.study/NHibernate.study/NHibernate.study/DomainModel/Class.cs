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

        public virtual IList<Student> Students
        {
            get
            {
                if (mStudents == null) mStudents = new List<Student>();
                return mStudents;
            }
            set 
            {
                mStudents = value;
            }
        }
    }
}
