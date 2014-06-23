using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LL.FrameWork.Implementation.Test.ValidatorsTests
{
    class EntityWithValidationAttribute
    {
        [Required(ErrorMessage="This is a required property")]
        public string RequiredProperty { get; set; }
    }
}
