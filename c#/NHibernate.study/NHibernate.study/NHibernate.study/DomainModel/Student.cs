using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Study.DomainModel
{
    public class Student
    {
        public virtual UInt32 Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual Class Class { get; set; }
    }
}
