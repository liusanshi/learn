using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Impl.Test.DomainModel
{
    public class Student
    {
        public UInt32 Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        //public int ClassId { get; set; }
        public virtual Class Class { get; set; }
    }
}
