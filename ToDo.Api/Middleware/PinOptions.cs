using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Api.Middleware
{
    public class PinOptions
    {
        public string PinCode { get; set; } = Configuration.Configurations.PinCode;

    }
}
